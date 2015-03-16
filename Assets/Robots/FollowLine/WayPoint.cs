using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

    public GameObject linepref;
    public GameObject cylinderpref;
    public WayPoint[] connect;
    public Vector3 dimensions = new Vector3(1, 1, 1);


	void Start () {
        foreach (WayPoint w in connect)
        {
            Platform line = (Instantiate(linepref) as GameObject).GetComponent<Platform>();
            line.transform.position = (w.transform.position + transform.position) / 2;
            Vector3 d;
            d = w.transform.position - transform.position;
            line.transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 1),d);
            line.transform.localScale = new Vector3(dimensions.z, dimensions.y, dimensions.x * d.magnitude);
        }

        Platform cyl = (Instantiate(cylinderpref) as GameObject).GetComponent<Platform>();
        cyl.transform.position = transform.position;
        cyl.transform.localScale = new Vector3(dimensions.z,dimensions.y*0.7f,dimensions.z);

        gameObject.SetActive(false);
	}
}
