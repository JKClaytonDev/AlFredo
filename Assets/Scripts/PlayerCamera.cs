using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float zoom = 30;
    bool frame1;
    public float fireOffset;
    Vector3 orth = new Vector3();
    public GameObject p1;
    public GameObject p2;
    private void Update()
    {
        Vector3 finalScaledPos = new Vector3() ;

            transform.position -= orth;
            Vector3 pos1 = (p1.transform.position+ p2.transform.position)/2;
            pos1.x = pos1.x / Mathf.Abs(pos1.x) * 6.3f;
            pos1.z = pos1.z / Mathf.Abs(pos1.z) * 6.5f;
        pos1.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, pos1, 100 * Time.deltaTime)+(Vector3.right*fireOffset*Mathf.Sin(Time.realtimeSinceStartup*50)*0.1f);
        fireOffset = Mathf.Max(0, fireOffset - Time.deltaTime * 10);
    }

}
