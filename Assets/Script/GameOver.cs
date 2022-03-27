using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text pointsText;

    void Start()
    {
        int score = CollectibleControler.coinCount;
        gameObject.SetActive(true);
        pointsText.text = score.ToString();
    }

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString();
    }

    public void Restart()
    {
        //CollectibleControler.coinCount = 0;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //CollectibleControler.coinCount = 0;
        SceneManager.LoadScene(1);
    }
}
