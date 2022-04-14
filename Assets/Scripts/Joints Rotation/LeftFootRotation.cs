using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFootRotation : MonoBehaviour
{
    public GameObject leftAnkle;
    public GameObject objectWithBodySourceView;

    public int X = 0;
    public float Y = 90;
    public float Z = -90;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourceView.GetComponent<BodySourceView>().user)
        {
            Y = objectWithBodySourceView.GetComponent<BodySourceView>().leftAnkleY;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetAnkle = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        leftAnkle.transform.rotation = Quaternion.Slerp(transform.rotation, targetAnkle, Time.deltaTime * 5.0f);
    }
}
