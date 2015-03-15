using UnityEngine;
using System.Collections;
using System.IO;

public class RLBrain : MonoBehaviour {

    public double[][] Q;
    public int[][] Qnum;
    public double Alpha = 0.1;
    public double Beta = 1.0;
    public string Filename = "RLBrain.txt";
    public int numstepsbeforelearn = 100;
    public double K = 1;

    bool inited = false;
    string readstring = "";
    int readn = 0;

    double F(double q, int n)
    {
        //if (n == 0) return 100000000000;
        return q + K / n;
    }

    struct LearnInput
    {
        public LearnInput(Robot r2, int s, int a, int s2, double re)
        {
            r = r2;
            state = s;
            action = a;
            state2 = s2;
            reward = re;
        }
        public Robot r;
        public int state;
        public int action;
        public int state2;
        public double reward;
    }
    LearnInput[] Learninput;
    int learnum = 0;

    protected double Combine(double alpha, double q, double lastq)
    {
        return (1.0 - alpha) * lastq + alpha * q;
    }

	public void Init(Robot r)
    {
        if (inited) return;
        Learninput = new LearnInput[numstepsbeforelearn];
        learnum = 0;
        inited = true;
        int X,Y;
        X = r.getStateNum();
        Q = new double[X][];
        Qnum = new int[X][];
        for (int i = 0; i < X; ++i)
        {
            Y = r.getActionNum(i);
            Q[i] = new double[Y];
            Qnum[i] = new int[Y];
            for (int j = 0; j < Y; ++j)
            {
                Q[i][j] = 0;
                Qnum[i][j] = 1;
            }
        }
        Load();
    }

    public void Learn(Robot r, int state, int action, int state2, double reward)
    {
        Learninput[learnum++] = new LearnInput(r,state,action,state2,reward);
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
                r = Learninput[learnum].r;
                Qnum[state][action]++;
                int X = r.getActionNum(state2);
                //double mx = F(Q[state2][0], Qnum[state2][0]);
                double mx = Q[state2][0];
                double d;
                for (int i = 1; i < X; ++i)
                {
                    //d = F(Q[state2][i], Qnum[state2][i]);
                    d = Q[state2][i];
                    if (d > mx)
                    {
                        //maxa = i;
                        mx = d;
                    }
                }
                Q[state][action] = Combine(Alpha, reward + Beta * mx, Q[state][action]);
            }
        }
    }

    void OnDestroy()
    {
        Save();
    }

    void Save()
    {
        /*if (File.Exists(Filename))
        {
            Debug.Log(Filename + " already exists.");
            return;
        }*/
        var sr = File.CreateText(Filename);
        sr.WriteLine(Q.Length);
        string S;
        for (int i = 0; i < Q.Length; ++i)
        {
            sr.WriteLine(Q[i].Length);
            S = "";
            for (int j = 0; j < Q[i].Length; ++j)
            {
                S += Q[i][j] + " ";
            }
            sr.WriteLine(S);
        }
        sr.Close();
    }

    double ReadNumber(StreamReader sr)
    {
        char c;
        string S = "";
        if (readstring.Length == readn)
        {
            readn = 0;
            readstring = sr.ReadLine();
            //Debug.Log("Line: " + readstring);
        }
        while (readn < readstring.Length)
        {
            c = readstring[readn++];
            if (c >= '0' && c <= '9' || c == '.' || c == '-')
            {
                S += c;
            }
            else /*if (S.Length > 0 || c == 0)*/ break;
        }
        //Debug.Log(S.Length + ": " + S);
        return double.Parse(S);
    }

    void Load()
    {
        if (!File.Exists(Filename)) return;
        readstring = "";
        readn = 0;
        int x, y;
        var sr = File.OpenText(Filename);
        x = (int)ReadNumber(sr);
        if (x == Q.Length) for (int i = 0; i < x; ++i)
        {
            y = (int)ReadNumber(sr);
            if (y == Q[i].Length) for (int j = 0; j < y; ++j)
            {
                Q[i][j] = ReadNumber(sr);
            }
        }
        sr.Close();
    }
}
