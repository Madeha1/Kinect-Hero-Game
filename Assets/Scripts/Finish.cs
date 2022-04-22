using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Hips")
        {
            Debug.Log("Finish");
            SceneManager.LoadScene(3);
        }
    }
}
