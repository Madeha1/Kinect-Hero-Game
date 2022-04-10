using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    public static float moveSpeed = 6;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * moveSpeed, Space.World);

    }
}
