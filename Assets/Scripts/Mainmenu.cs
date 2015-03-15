using UnityEngine;
using System.Collections;

public class Mainmenu : MonoBehaviour {

    public int numscenes = 2;

	void OnGUI()
    {
        Rect r = new Rect(0,0,Screen.width*0.1f,0);
        r.height = r.width / 4;
        r.x = (Screen.width - r.width) / 2;
        r.y = r.height * 0.5f;
        for (int i = 1;i <= numscenes; ++i)
        {
            r.y += r.height * 1.3f;
            if (GUI.Button(r,"Scene: "+i))
            {
                Application.LoadLevel(i);
            }
        }
    }
}
