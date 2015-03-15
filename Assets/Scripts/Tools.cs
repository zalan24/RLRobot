using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {


    public Robot[] robots;
    public float step = 0.05f;

    float Gamma = 0;

	void Start () {
        if (robots.Length == 0) Destroy(gameObject);
        Gamma = robots[0].Gamma;
	}
	
	void OnGUI () {
        float x, y;
        x = Screen.width;
        y = Screen.height;
        Rect r = new Rect(0,0,0.05f*x,0);
        r.height = r.width;
        r.x = r.width * 0.1f;
        r.y = y - r.height * 1.1f;
        if (GUI.Button(r, "-"))
        {
            Gamma -= step;
            if (Gamma < 0) Gamma = 0;
            foreach (Robot ro in robots)
            {
                ro.Gamma = Gamma;
            }
        }
        r.x += r.width * 1.1f;
        r.width = r.width * 2;
        GUI.Box(r, "Gamma\n"+Gamma);
        r.x += r.width * 1.1f;
        r.width = r.height;
        if (GUI.Button(r, "+"))
        {
            Gamma += step;
            if (Gamma > 1) Gamma = 1;
            foreach (Robot ro in robots)
            {
                ro.Gamma = Gamma;
            }
        }
	}
}
