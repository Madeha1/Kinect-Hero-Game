using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hand : MonoBehaviour
{
    public Transform HandMesh;

    public GameObject MainMenuButtons;
    public GameObject HowToPlay;

    private Dictionary<string, int> _buttonScenes = new Dictionary<string, int>()//button name with scene numbers
    { 
        { "startGame",  4 },
        { "Help", 3 },
        { "ExitGame", -1 },
        { "back", 0 },
        { "MainMenu", 0 },
        { "easy", 1 },
        { "medium", 1 },
        { "hard", 1 },
        { "Next", 5 },
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
                    if(item.Key == "easy")
                    {
                        Environment.MoveSpeed = 10;
                    }
                    if (item.Key == "medium")
                    {
                        Environment.MoveSpeed = 12;
                    }
                    if (item.Key == "hard")
                    {
                        Environment.MoveSpeed = 14;
                    }
                    if (item.Key == "Next")
                    {
                        MainMenuButtons.SetActive(true);
                        HowToPlay.SetActive(false);

                    }
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
