using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandRotation : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject objectWithBodySourcViewScipt;

    public int X = 0;
    public int Y = -90;
    public int Z = 0;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourcViewScipt.GetComponent<BodySourceView>().user)
        {
            //float HandAround = objectWithBodySourcViewScipt.GetComponent<BodySourceView>().giroManoDer;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHand = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        rightHand.transform.rotation = Quaternion.Slerp(transform.rotation, targetHand, Time.deltaTime * 5.0f);
    }
}
