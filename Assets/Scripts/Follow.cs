using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

    public GameObject gm;

    Vector3 posdif;

    void Start()
    {
        posdif = transform.position - gm.transform.position;
    }

	void Update () {
        transform.position = gm.transform.position + posdif;
	}
}
