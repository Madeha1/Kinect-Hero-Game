using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hand : MonoBehaviour
{
    public Transform HandMesh;
    private Dictionary<string, int> _buttonScenes = new Dictionary<string, int>()//button name with scene numbers
    { 
        { "startGame",  5 },
        { "Help", 4 },
        { "ExitGame", -1 },
        { "back", 0 },
        { "MainMenu", 0 },
        { "easy", 1 },
        { "medium", 1 },
        { "hard", 1 },
    };
    private void Update()
    {
        HandMesh.position = Vector3.Lerp(HandMesh.position, transform.position, Time.deltaTime * 15.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var item in _buttonScenes)
        {
            if(collision.gameObject.CompareTag(item.Key))
            {
                int sceneNumber = item.Value;
                if (sceneNumber != -1) {
                    SceneManager.LoadScene(sceneNumber);
                }
                else
                {
                    Application.Quit();
                }
            }
        } 
       return;
    }
}
