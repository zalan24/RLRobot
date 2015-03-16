using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

	protected virtual void Start () {
	
	}
	
	protected virtual void Update () {
	
	}

    public virtual int getState()
    {
        return 0;
    }

    public virtual int getStateNum()
    {
        return 0;
    }
}
