using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedBrain : MonoBehaviour {

    public double Alpha = 0.1;
    public double Beta = 0.7;
    public int numstepsbeforelearn = 100;

    bool inited = false;
    int[] Numstates;
    int[] Numactions;
    int lowercomplexitybound = 1;
    int uppercomplexitybound = 9999999;
    int actionlowercomplexitybound = 1;
    int actionuppercomplexitybound = 9999999;
    Qtable[] Q;
    LearnInput[] Learninput;
    int learnum = 0;

    public struct Qvalue
    {
        public double x;
        public int num;
        public Qvalue(double x2, int num2)
        {
            x = x2;
            num = num2;
        }
    }

    struct LearnInput
    {
        public LearnInput(AdvancedBrain.Array s, AdvancedBrain.Array a, AdvancedBrain.Array s2, double re)
        {
            state = new AdvancedBrain.Array(s);
            action = new AdvancedBrain.Array(a);
            state2 = new AdvancedBrain.Array(s2);
            reward = re;
        }
        public AdvancedBrain.Array state;
        public AdvancedBrain.Array action;
        public AdvancedBrain.Array state2;
        public double reward;
    }

    public struct Qtablepos
    {
        public int s, a;
        public Qtablepos(int s2, int a2)
        {
            s = s2;
            a = a2;
        }
    }

    public struct Qtable
    {
        public double[][] t;
        //public int[][] num;
        public double w;
        public List<int> subs;
        public List<int> suba;

        public Qtablepos getpos(int[] numstates, int[] numactions, AdvancedBrain.Array S, AdvancedBrain.Array A)
        {
            int nump = 1;
            int s = 0;
            for (int i = 0; i < subs.Count; ++i)
            {
                s += S.subs[subs[i]] * nump;
                nump *= numstates[subs[i]];
            }
            nump = 1;
            int a = 0;
            for (int i = 0; i < suba.Count; ++i)
            {
                a += A.subs[suba[i]] * nump;
                nump *= numactions[suba[i]];
            }
            return new Qtablepos(s,a);
        }

        public double get(int[] numstates, int[] numactions, AdvancedBrain.Array S, AdvancedBrain.Array A)
        {
            
            //return new Qvalue(t[s][a],num[s][a]);
            Qtablepos pos = getpos(numstates, numactions, S, A);
            return t[pos.s][pos.a];
        }
    }

    public class Array
    {
        public int[] subs;
        public Array()
        {
        }
        public Array(Array a)
        {
            subs = new int[a.subs.Length];
            for (int i = 0; i < a.subs.Length; ++i) subs[i] = a.subs[i];
        }
        public Array(int [] s)
        {
            subs = new int[s.Length];
            for (int i = 0; i < s.Length; ++i) subs[i] = s[i];
        }
        public Array(int s1)
        {
            subs = new int[1];
            subs[0] = s1;
        }
        public Array(int s1, int s2)
        {
            subs = new int[2];
            subs[0] = s1;
            subs[1] = s2;
        }
        public Array(int s1, int s2, int s3)
        {
            subs = new int[3];
            subs[0] = s1;
            subs[1] = s2;
            subs[2] = s3;
        }
        public Array(int s1, int s2, int s3, int s4)
        {
            subs = new int[4];
            subs[0] = s1;
            subs[1] = s2;
            subs[2] = s3;
            subs[3] = s4;
        }
        public Array(int s1, int s2, int s3, int s4, int s5)
        {
            subs = new int[5];
            subs[0] = s1;
            subs[1] = s2;
            subs[2] = s3;
            subs[3] = s4;
            subs[4] = s5;
        }
    }

    int NumP(int n, int k)
    {
        if (k <= 0 || k >= n || n <= 0) return 1;
        return NumP(n-1,k) + NumP(n-1,k-1);
    }

    int R2(bool[] inp, bool[] inp2, int x, int x2, int cpxa, int n)
    {
        if (x2 == inp2.Length)
        {
            if (cpxa > 0) return n;
            Q[n] = new Qtable();
            Q[n].w = 1.0 / Q.Length;
            int nums = 1;
            for (int i = 0; i < x; ++i)
            {
                if (inp[i])
                {
                    Q[n].subs.Add(i);
                    nums *= Numstates[i];
                }
            }
            int numa = 1;
            for (int i = 0; i < x2; ++i)
            {
                if (inp2[i])
                {
                    Q[n].suba.Add(i);
                    numa *= Numactions[i];
                }
            }
            Q[n].t = new double[nums][];
            //Q[n].num = new int[nums][];
            for (int i = 0; i < nums; ++i)
            {
                Q[n].t[i] = new double[numa];
                //Q[n].num[i] = new int[numa];
                for (int j = 0; j < numa; ++j)
                {
                    Q[n].t[i][j] = 0;
                    //Q[n].num[i][j] = 0;
                }
            }
            n++;
            return n;
        }
        else
        {
            if (cpxa > 0)
            {
                inp2[x2] = false;
                n = R2(inp, inp2, x, x2 + 1, cpxa, n);
                inp[x] = true;
                n = R2(inp, inp2, x, x2 + 1, cpxa, n);

                inp[x] = false;
            }
            else
            {
                inp[x] = false;
                n = R2(inp, inp2, x, x2 + 1, cpxa, n);
            }
        }
        return n;
    }

    int R(bool[] inp, int x, int cpx, int n)
    {
        if (x == inp.Length)
        {
            if (cpx > 0) return n;
            bool[] inp2 = new bool[Numactions.Length];
            for (int j = 0; j < inp2.Length; ++j) inp2[j] = false;
            for (int i = actionlowercomplexitybound; i <= actionuppercomplexitybound; ++i)
            {
                n = R2(inp,inp2,x, 0,i,n);
            }
            return n;
        }
        else
        {
            if (cpx > 0)
            {
                inp[x] = false;
                n = R(inp, x + 1, cpx, n);
                inp[x] = true;
                n = R(inp, x + 1, cpx-1, n);

                inp[x] = false;
            }
            else
            {
                inp[x] = false;
                n = R(inp,x+1,cpx,n);
            }
        }
        return n;
    }

    public void Init(int[] numstates, int[] numaction)
    {
        if (inited) return;
        inited = true;
        Numstates = new int[numstates.Length];
        for (int i = 0; i < numstates.Length; ++i) Numstates[i] = numstates[i];
        Numactions = new int[numaction.Length];
        for (int i = 0; i < numaction.Length; ++i) Numactions[i] = numaction[i];
        if (uppercomplexitybound > numstates.Length) uppercomplexitybound = numstates.Length;
        if (lowercomplexitybound > uppercomplexitybound) lowercomplexitybound = uppercomplexitybound;
        if (actionuppercomplexitybound > numaction.Length) actionuppercomplexitybound = numaction.Length;
        if (actionlowercomplexitybound > actionuppercomplexitybound) actionlowercomplexitybound = actionuppercomplexitybound;

        int numw = 0;
        for (int i = lowercomplexitybound; i <= uppercomplexitybound; ++i) numw += NumP(numstates.Length,i);
        int numw2 = 0;
        for (int i = actionlowercomplexitybound; i <= actionuppercomplexitybound; ++i) numw2 += NumP(numaction.Length, i);
        Q = new Qtable[numw*numw2];
        int n = 0;
        bool[] inp = new bool[numstates.Length];
        for (int j = 0; j < inp.Length; ++j) inp[j] = false;
        for (int i = lowercomplexitybound; i <= uppercomplexitybound; ++i)
        {
            n = R(inp,0,i, n);
        }
    }

    public double getQ(AdvancedBrain.Array S, AdvancedBrain.Array A)
    {
        double s = 0;
        for (int i = 0; i < Q.Length; ++i) s += Q[i].get(Numstates, Numactions, S,A) * Q[i].w;
        return s;
    }

    protected AdvancedBrain.Array getBestAction(AdvancedBrain.Array S, AdvancedBrain.Array A, int x)
    {
        if (x >= Numactions.Length) return new AdvancedBrain.Array(A);
        AdvancedBrain.Array ret = new AdvancedBrain.Array(0);
        double max = -100000;
        double mx = 0;
        AdvancedBrain.Array a;
        for (int i = 0; i < Numactions[i]; ++i)
        {
            A.subs[x] = i;
            a = getBestAction(S,A,x+1);
            mx = getQ(S, a);
            if (mx > max)
            {
                max = mx;
                ret = new AdvancedBrain.Array(a);
            }
        }
        return ret;
    }

    public AdvancedBrain.Array getBestAction(AdvancedBrain.Array S)
    {
        AdvancedBrain.Array A = new AdvancedBrain.Array();
        A.subs = new int[Numactions.Length];
        return getBestAction(S,A,0);
    }

    protected double Combine(double alpha, double x, double y)
    {
        return (1.0 - alpha) * y + alpha * x;
    }

    public void Learn(AdvancedBrain.Array state, AdvancedBrain.Array action, AdvancedBrain.Array state2, double reward)
    {
        Learninput[learnum++] = new LearnInput(state, action, state2, reward);
        if (learnum >= numstepsbeforelearn)
        {
            while (learnum > 0)
            {
                learnum--;
                //int maxa = 0;
                state = Learninput[learnum].state;
                state2 = Learninput[learnum].state2;
                reward = Learninput[learnum].reward;
                action = Learninput[learnum].action;
                for (int i = 0; i < Q.Length; ++i)
                {
                    AdvancedBrain.Array a2 = getBestAction(state2);
                    Qtablepos pos = Q[i].getpos(Numstates, Numactions, state, action);
                    double mx = Q[i].get(Numstates,Numactions,state2,a2);
                    double newv = Combine(Beta,reward,mx);
                    Q[i].t[pos.s][pos.a] = Combine(Alpha, newv, Q[i].t[pos.s][pos.a]);
                    //Q[state][action] = Combine(Alpha, reward + Beta * mx, Q[state][action]);
                }
            }
        }
    }
}
