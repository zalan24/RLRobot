using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

    public RLBrain brain;

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

    protected virtual int getAction(int state)
    {
        return 0;
    }

}
