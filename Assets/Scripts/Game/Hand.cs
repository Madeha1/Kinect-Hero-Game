using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hand : MonoBehaviour
{
    public Transform mHandMesh;

    private void Update()
    {
        mHandMesh.position = Vector3.Lerp(mHandMesh.position, transform.position, Time.deltaTime * 15.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("test");

        if (collision.gameObject.CompareTag("startGame")) {
            SceneManager.LoadScene(5);
        }
        else if(collision.gameObject.CompareTag("Help"))
        {
            SceneManager.LoadScene(4);
        }
        else if (collision.gameObject.CompareTag("ExitGame"))
        {
            Application.Quit();
        }
        else if (collision.gameObject.CompareTag("MainMenu") || collision.gameObject.CompareTag("back"))
        {
            SceneManager.LoadScene(0);
        }
        else if (collision.gameObject.CompareTag("easy"))
        {
            Change.moveSpeed = 6;
            SceneManager.LoadScene(1);
        }
        else if (collision.gameObject.CompareTag("medium"))
        {
            Change.moveSpeed = 8;
            SceneManager.LoadScene(1);
        }
        else if (collision.gameObject.CompareTag("hard"))
        {
            Change.moveSpeed = 10;
            SceneManager.LoadScene(1);
        }
        else
        {
            return;
        }
    }
}
