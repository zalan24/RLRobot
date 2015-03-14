using UnityEngine;
using System.Collections;

public class RLBrain : MonoBehaviour {

    public double[][] Q;
    public double Alpha = 0.1;
    public double Beta = 1.0;

    protected double Combine(double alpha, double q, double lastq)
    {
        return (1.0 - alpha) * lastq + alpha * q;
    }

	public void Init(Robot r)
    {
        int X,Y;
        X = r.getStateNum();
        Q = new double[X][];
        for (int i = 0; i < X; ++i)
        {
            Y = r.getActionNum(i);
            Q[i] = new double[Y];
            for (int j = 0; j < Y; ++j) Q[i][j] = 0;
        }
    }

    public void Learn(Robot r, int state, int action, int state2, double reward)
    {
        int maxa = 0;
        int X = r.getActionNum(state2);
        for (int i = 1; i < X; ++i)
        {
            if (Q[state2][i] > Q[state2][maxa]) maxa = i;
        }
        Q[state][action] = Combine(Alpha,reward + Beta*Q[state2][maxa],Q[state][action]);
    }
}
