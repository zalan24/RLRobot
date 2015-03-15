using UnityEngine;
using System.Collections;

public class BasicMove : Robot
{

    public HingeJoint upper, lower;
    public GameObject body;
    public float velocity = 100;
    public bool control = true;

    public int NumIntervalls = 64;

    float speed = 0;
    Vector3 lastpos = Vector3.zero;

    Rigidbody lowerb, upperb, bodyb;
    Vector3 upperp, lowerp, bodyp;
    Quaternion upperq, lowerq, bodyr;

    protected override void Start()
    {
        base.Start();
        lowerb = lower.GetComponent<Rigidbody>();
        upperb = upper.GetComponent<Rigidbody>();
        bodyb = body.GetComponent<Rigidbody>();
        upperp = upper.transform.position;
        lowerp = lower.transform.position;
        upperq = upper.transform.rotation;
        lowerq = lower.transform.rotation;
        bodyp = body.transform.position;
        bodyr = body.transform.rotation;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            control = !control;
        }
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
            m.targetVelocity = velocity * (a1 - 1);
            upper.motor = m;
            m = lower.motor;
            m.targetVelocity = velocity * (a2 - 1);
            lower.motor = m;
        }
        if (body.transform.position.x > 200 || body.transform.position.x < -200 || body.transform.rotation.eulerAngles.z > 90 && body.transform.rotation.eulerAngles.z < 270)
        {
            Reset();
        }
    }

    void Reset()
    {
        lastpos = bodyp;
        body.transform.rotation = bodyr;
        body.transform.position = lastpos;
        bodyb.velocity = Vector3.zero;
        bodyb.angularVelocity = Vector4.zero;
        upper.transform.position = upperp;
        lower.transform.position = lowerp;
        upper.transform.rotation = upperq;
        lower.transform.rotation = lowerq;
        lowerb.velocity = Vector3.zero;
        upperb.velocity = Vector3.zero;
        lowerb.angularVelocity = Vector3.zero;
        upperb.angularVelocity = Vector3.zero;
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
        return NumIntervalls * NumIntervalls;
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
