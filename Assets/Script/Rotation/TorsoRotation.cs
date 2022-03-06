using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoRotation : MonoBehaviour
{
    public GameObject torso;
    public GameObject objectWithBodySourceView;

    public int X = 0;
    public float Y = 180;
    public int Z = 0;

    // Update is called once per frame
    void Update()
    {
        if (objectWithBodySourceView.GetComponent<BodySourceView>().user)
        {
            Y = objectWithBodySourceView.GetComponent<BodySourceView>().chest;
        }

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetSpine = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        torso.transform.rotation = Quaternion.Slerp(transform.rotation, targetSpine, Time.deltaTime * 5.0f);
    }
}