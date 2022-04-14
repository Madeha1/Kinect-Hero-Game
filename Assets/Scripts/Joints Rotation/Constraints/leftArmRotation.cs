using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftArmRotation : MonoBehaviour
{

    public GameObject leftArm;
    public GameObject objectWithBodySourcView;

    public int X = 180;
    public int Y = 90;
    public int Z = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the cube by converting the angles into a quaternion.
        Quaternion targetHand = Quaternion.Euler(X, Y, Z);

        // Dampen towards the target rotation
        leftArm.transform.rotation = Quaternion.Slerp(transform.rotation, targetHand, Time.deltaTime * 5.0f);
    }
}
