using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static float MoveSpeed = 0;

    void Update() {
        transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed, Space.World);
    }
}
