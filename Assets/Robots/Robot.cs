using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

    public RLBrain brain;
    public float Gamma = 0.8f;
    public double K = 0;

    protected int S=0, S2=0, A=0;
    protected double R=0;

	protected virtual void Start ()
    {
        brain.Init(this);
	}

    protected virtual void Update()
    {
        S2 = S;
        S = getState();
        R = getReward(S2,A,S);
        //Debug.Log(R);
        brain.Learn(this,S2,A,S,R);
	}

    public virtual int getState()
    {
        return 0;
    }

    public virtual int getStateNum()
    {
        return 0;
    }

    public virtual int getActionNum(int state)
    {
        return 0;
    }

    protected virtual double getReward(int state, int action, int state2)
    {
        return 0;
    }

    protected virtual double F(double q, int n)
    {
        return q - n * K;
    }

    protected virtual int getAction(int state)
    {
        int maxa = 0;
        int X = getActionNum(state);
        if (Random.Range(0.0f, 1.0f) < Gamma) return Random.Range(0, X);
        for (int i = 1; i < X; ++i)
        {
            if (F(brain.Q[state][i], brain.Qnum[state][i]) > F(brain.Q[state][maxa], brain.Qnum[state][maxa])) maxa = i;
        }
        return maxa;
    }

}
