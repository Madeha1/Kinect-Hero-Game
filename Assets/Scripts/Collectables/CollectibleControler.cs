using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleControler : MonoBehaviour
{
    public static int coinCount;
    public GameObject coinCountDisplay;
    public Text highScore;

    //for game over 
    public GameOver gameOver;


    public void GameOver()
    {
        gameOver.Setup(coinCount);
    }

    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HightScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        coinCountDisplay.GetComponent<Text>().text = "" + coinCount;

        if(coinCount > PlayerPrefs.GetInt("HightScore", 0))
        {
            PlayerPrefs.SetInt("HightScore", coinCount);
            highScore.text = coinCount.ToString();
        }
        
    }
}
