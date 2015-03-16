using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public Color col;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = col;
    }

}
