using UnityEngine;
using System.Collections;

public class FollowLine : Robot {


    public bool control;
    public WheelCollider WheelTR, WheelTL, WheelBR, WheelBL;
    public float wheelforce = 1;
    public Sensor[] sensors;

    int numstates = 0;
    int[] numstatess;

    protected override void Start()
    {
        numstatess = new int[sensors.Length];
        numstates = 1;
        for (int i = 0; i < sensors.Length; ++i)
        {
            numstatess[i] = sensors[i].getStateNum();
            numstates *= numstatess[i];
        }
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            control = !control;
        }
        if (control)
        {
            float r, l;
            r = l = 0;
            if (Input.GetKey(KeyCode.A)) {
                l = -1;
                r = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                r = -1;
                l = 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (r == 0 && l == 0)
                {
                    l = r = 1;
                }
                else if (r == 1) l = 0.25f;
                else r = 0.25f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (r == 0 && l == 0)
                {
                    l = r = 1;
                }
                else if (r == 1) l = 0.25f;
                else r = 0.25f;
                r *= -1;
                l *= -1;
                float sw = r;
                r = l;
                l = sw;
            }
            WheelTR.motorTorque = r;
            WheelTL.motorTorque = l;
            WheelBR.motorTorque = r;
            WheelBL.motorTorque = l;
        }
    }

    public override int getState()
    {
        int state = 0;
        int numposs = 1;
        for (int i = 0; i < sensors.Length; ++i)
        {
            state += numposs * sensors[i].getState();
            numposs *= numstatess[i];
        }
        return state;
    }

    public override int getStateNum()
    {
        return numstates;
    }

    public override int getActionNum(int state)
    {
        return 1;
    }

    protected override double getReward(int state, int action, int state2)
    {
        return 0;
    }


}
