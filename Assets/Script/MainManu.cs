using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Debugging");

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Debugging");
        Application.Quit();
    }
}

