using UnityEngine;
using System.Collections;

public class BasicRobot : Robot {

    public HingeJoint upper, lower;
    public GameObject body;
    public float velocity = 100;
    public float Gamma = 0.8f;
    public bool control = true;

    public int NumIntervalls = 64;

    float speed = 0;
    Vector3 lastpos = Vector3.zero;

    Rigidbody lowerb, upperb, bodyb;

    protected override void Start()
    {
        base.Start();
        lowerb = lower.GetComponent<Rigidbody>();
        upperb = upper.GetComponent<Rigidbody>();
        bodyb = body.GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
        int a1, a2;
        speed = (body.transform.position.x - lastpos.x) / Time.deltaTime;
        lastpos = body.transform.position;
        lowerb.WakeUp();
        upperb.WakeUp();
        bodyb.WakeUp();
        if (control)
        {
            if (Input.GetKey(KeyCode.W))
            {
                JointMotor m = upper.motor;
                m.targetVelocity = -velocity;
                upper.motor = m;
                a1 = 0;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                JointMotor m = upper.motor;
                m.targetVelocity = velocity;
                upper.motor = m;
                a1 = 2;
            }
            else
            {
                JointMotor m = upper.motor;
                m.targetVelocity = 0;
                upper.motor = m;
                a1 = 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                JointMotor m = lower.motor;
                m.targetVelocity = -velocity;
                lower.motor = m;
                a2 = 0;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                JointMotor m = lower.motor;
                m.targetVelocity = velocity;
                lower.motor = m;
                a2 = 2;
            }
            else
            {
                JointMotor m = lower.motor;
                m.targetVelocity = 0;
                lower.motor = m;
                a2 = 1;
            }
            A = a2 * 3 + a1;
        }
        else
        {
            A = getAction(S);
            a1 = A % 3;
            a2 = A / 3;
            JointMotor m = upper.motor;
            m.targetVelocity = velocity * (a1-1);
            upper.motor = m;
            m = lower.motor;
            m.targetVelocity = velocity * (a2 - 1);
            lower.motor = m;
        }
	}

    protected override int getAction(int state)
    {
        int maxa = 0;
        int X = getActionNum(state);
        if (Random.Range(0.0f, 1.0f) < Gamma) return Random.Range(0, X);
        for (int i = 1; i < X; ++i)
        {
            if (brain.Q[state][i] > brain.Q[state][maxa]) maxa = i;
        }
        return maxa;
    }

    int getstatex()
    {
        float f = upper.gameObject.transform.localRotation.eulerAngles.z;
        int ret = (int)(f * NumIntervalls / 360.0f);
        return ret;
    }

    int getstatey()
    {
        float f = lower.gameObject.transform.localRotation.eulerAngles.z;
        int ret = (int)(f * NumIntervalls / 360.0f);
        return ret;
    }

    public override int getState()
    {
        return getstatey() * NumIntervalls + getstatex();
    }

    public override int getStateNum()
    {
        return NumIntervalls*NumIntervalls;
    }

    public override int getActionNum(int state)
    {
        return 9;
    }

    protected override double getReward(int state, int action, int state2)
    {
        return speed;
    }
}
