using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmRotation : MonoBehaviour
{

    public GameObject LeftArm;
    public GameObject Body;

    public int X = 180;
    public int Y = 90;
    public int Z = 0;

    void Update()
    {
        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHand = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        LeftArm.transform.rotation = Quaternion.Slerp(transform.rotation, targetHand, Time.deltaTime * 5.0f);
    }
}
