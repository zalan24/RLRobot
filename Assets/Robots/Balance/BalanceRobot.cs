using UnityEngine;
using System.Collections;

public class BalanceRobot : Robot {

    public HingeJoint arm;
    public bool control = true;
    public int NumIntervalls = 64;
    public GameObject End;
    public Rigidbody Body;
    public float V = 10;
    public double lifereward = 0.1f;
    public double fallpunish = -10;

    int laststate = 0;
    int state = 0;
    Rigidbody armb;
    Vector3 posb,posa;
    Quaternion qa;

    double reward;

    protected override void Start()
    {
        base.Start();
        reward = lifereward;
        armb = arm.GetComponent<Rigidbody>();
        posb = Body.transform.position;
        posa = arm.transform.position;
        qa = arm.transform.rotation;
        float f = arm.gameObject.transform.localRotation.eulerAngles.z;
        laststate = (int)(f * NumIntervalls / 360.0f);
    }
	
	protected override void Update()
    {
        base.Update();
        laststate = state;
        float f = arm.gameObject.transform.localRotation.eulerAngles.z;
        state = (int)(f * NumIntervalls / 360.0f);
        Body.WakeUp();
        armb.WakeUp();
	    if (control)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Body.AddForce(new Vector3(-V, 0, 0));
                A = 0;
            } else if (Input.GetKey(KeyCode.D))
            {
                Body.AddForce(new Vector3(V, 0, 0));
                A = 2;
            } else
            {
                A = 1;
            }
        }
        else
        {
            A = getAction(S);
            Body.AddForce(new Vector3(V*(A-1), 0, 0));
        }


        if (Body.transform.position.x > 200 || Body.transform.position.x < -200)
        {
            ResetState();
        }
        if (End.transform.position.y < Body.transform.position.y)
        {
            ResetState();
            reward = fallpunish;
        }
	}

    void ResetState()
    {
        Body.transform.position = posb;
        arm.transform.position = posa;
        arm.transform.rotation = qa;
        Body.velocity = Vector3.zero;
        armb.velocity = Vector3.zero;
        armb.angularVelocity = Vector3.zero;
        float f = arm.gameObject.transform.localRotation.eulerAngles.z;
        laststate = (int)(f * NumIntervalls / 360.0f);
    }


    public override int getState()
    {

        return state * NumIntervalls + laststate;
        //return state;
    }

    public override int getStateNum()
    {
        return NumIntervalls * NumIntervalls;
        //return NumIntervalls;
    }

    public override int getActionNum(int state)
    {
        return 3;
    }

    protected override double getReward(int state, int action, int state2)
    {
        double r = reward;
        reward = lifereward;
        return r;
    }
}
