using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightShoulderRotation: MonoBehaviour
{
    public GameObject RightShoulder;
    public GameObject Body;

    public int X = 0;
    public int Y = 180;
    public float Z = 0;

    // Update is called once per frame
    void Update()
    {
        if (Body.GetComponent<BodySourceView>().user)
        {
            Z = Body.GetComponent<BodySourceView>().RightShoulderY;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetShoulder = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        RightShoulder.transform.rotation = Quaternion.Slerp(transform.rotation, targetShoulder, Time.deltaTime * 5.0f);
    }
}
