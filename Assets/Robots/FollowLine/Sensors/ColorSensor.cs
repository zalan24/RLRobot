using UnityEngine;
using System.Collections;

public class ColorSensor : Sensor {

    public bool onlyfade = true;
    public int numintevalls = 2;
    public float seedistance = 0.1f;
    public Transform point;

    Color getColor()
    {
        RaycastHit inf = new RaycastHit();
        Physics.Raycast(point.position, new Vector3(0, -1, 0), out inf, seedistance);
        if (inf.collider)
        {
            Platform p = inf.collider.gameObject.GetComponent<Platform>();
            if (p) return p.col;
        }
        return new Color(0,0,0);
    }

    public override int getState()
    {
        Color c = getColor();
        int s1, s2, s3;
        s1 = (int)(c.r * (numintevalls-1) + 0.5f);
        s2 = (int)(c.g * (numintevalls - 1) + 0.5f);
        s3 = (int)(c.b * (numintevalls - 1) + 0.5f);
        if (onlyfade)
        {
            return (s1 + s2 + s3) / 3;
        }
        else
        {
            return s1 * numintevalls * numintevalls + s2 * numintevalls + s3;
        }
    }

    public override int getStateNum()
    {
        if (onlyfade) return numintevalls;
        else return numintevalls * numintevalls * numintevalls;
    }
}
