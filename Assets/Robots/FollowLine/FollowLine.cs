using UnityEngine;
using System.Collections;

public class FollowLine : Robot {


    public bool control;
    public WheelCollider WheelTR, WheelTL, WheelBR, WheelBL;
    public float wheelforce = 1;
    public Sensor[] sensors;
    public double resetreward = -1000;
    public int numactions = 3;
    public float MaxTimeoutline = 1;
    public float ResetTimeoutline = 5;
    public int numtimestate = 20;

    int numstates = 0;
    int[] numstatess;

    Vector3 startp;
    Quaternion startq;
    bool flipped = false;
    float timeoncolor = 0;
    int colorstate = 0;
    Rigidbody rigid;
    bool back = false;

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
        startp = transform.position;
        startq = transform.rotation;
        rigid = GetComponent<Rigidbody>();
    }

    void Reset()
    {
        transform.position = startp;
        transform.rotation = startq;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        timeoncolor = 0;
    }

    float getSpeed()
    {
        return Vector3.Dot(rigid.velocity,transform.forward);
    }

    protected override void Update()
    {
        base.Update();
        int s = sensors[0].getState();
        if (colorstate == s)
        {
            timeoncolor += Time.deltaTime;
        }
        else
        {
            if (timeoncolor > MaxTimeoutline) back = true;
            colorstate = s;
            timeoncolor = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            control = !control;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        else if (transform.rotation.eulerAngles.x > 90 && transform.rotation.eulerAngles.x < 270 || transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            Reset();
        }
        else if (timeoncolor > ResetTimeoutline)
        {
            Reset();
            flipped = true;
        }
        int a1, a2;
        float r, l;
        if (control)
        {
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
            WheelTR.motorTorque = r * wheelforce;
            WheelTL.motorTorque = l * wheelforce;
            WheelBR.motorTorque = r * wheelforce;
            WheelBL.motorTorque = l * wheelforce;
            a1 = (int)((r + 1.0f) / 2.0f * (numactions - 1) + 0.5f);
            a2 = (int)((l + 1.0f) / 2.0f * (numactions - 1) + 0.5f);
            A = a1 * numactions + a2;
        }
        else
        {
            A = getAction(S);
            a1 = A / numactions;
            a2 = A % numactions;
            r = 2.0f * a1 / (numactions - 1) - 1.0f;
            l = 2.0f * a2 / (numactions - 1) - 1.0f;
            WheelTR.motorTorque = r * wheelforce;
            WheelTL.motorTorque = l * wheelforce;
            WheelBR.motorTorque = r * wheelforce;
            WheelBL.motorTorque = l * wheelforce;
        }
    }

    int getTimestate()
    {
        return (int)(Mathf.Clamp01(timeoncolor / MaxTimeoutline) * (numtimestate - 1) + 0.5f);
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
        state += numposs * getTimestate();
        return state;
    }

    public override int getStateNum()
    {
        return numstates * numtimestate;
    }

    public override int getActionNum(int state)
    {
        return numactions * numactions;
    }

    protected override double getReward(int state, int action, int state2)
    {
        if (back)
        {
            back = false;
            return 1;
        }
        if (flipped)
        {
            flipped = false;
            return resetreward;
        }
        if (timeoncolor > MaxTimeoutline)
        {
            return resetreward*0.05f;
        }
        double v = getSpeed() * 0.6f;
        if (v < 0) v *= 5;
        return v;
    }


}
