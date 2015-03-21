using UnityEngine;
using System.Collections;

public abstract class AdvancedRobot : MonoBehaviour
{

    public AdvancedBrain brain;
    public float Gamma = 0.3f;
    //public double K = 0;

    protected AdvancedBrain.Array S, S2, A;
    protected double R = 0;
    protected int[] Numstates;
    protected int[] Numactions;

    protected virtual void Start()
    {
        genSubsStates();
        genSubsActions();
        S = new AdvancedBrain.Array(0);
        S2 = new AdvancedBrain.Array(0);
        A = new AdvancedBrain.Array(0);
        brain.Init(Numstates, Numactions);
    }

    protected virtual void Update()
    {
        S2 = S;
        S = getState();
        R = getReward(S2, A, S);
        //Debug.Log(R);
        //brain.Learn(this, S2, A, S, R);
    }

    public virtual int getNumSubsStates()
    {
        return Numstates.Length;
    }

    public virtual int getNumSubsActions()
    {
        return Numactions.Length;
    }

    public abstract void genSubsActions();

    public abstract void genSubsStates();

    public virtual AdvancedBrain.Array getState()
    {
        return new AdvancedBrain.Array(0);
    }

    public virtual int getStateNum()
    {
        int n = 1;
        for (int i = 0; i < Numstates.Length; ++i) n *= Numstates[i];
        return n;
    }

    public virtual int getActionNum()
    {
        int n = 1;
        for (int i = 0; i < Numactions.Length; ++i) n *= Numactions[i];
        return n;
    }

    public abstract int getStateNum(int sub);

    public abstract int getActionNum(int sub);

    protected virtual double getReward(AdvancedBrain.Array state, AdvancedBrain.Array action, AdvancedBrain.Array state2)
    {
        return 0;
    }

    /*protected virtual double F(double q, int n)
    {
        return q - n * K;
    }*/

    protected virtual AdvancedBrain.Array getAction(AdvancedBrain.Array state)
    {
        if (Random.Range(0.0f, 1.0f) < Gamma)
        {
            AdvancedBrain.Array ret = new AdvancedBrain.Array();
            ret.subs = new int[getNumSubsActions()];
            for (int i = 0; i < getNumSubsActions(); ++i) ret.subs[i] = Random.Range(0, getActionNum(i));
            return ret;
        }
        return brain.getBestAction(state);
    }

}
