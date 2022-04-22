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
        int score = CollectableControler.coinCount;
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
        CollectableControler.coinCount = 0;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        CollectableControler.coinCount = 0;
        SceneManager.LoadScene(1);
    }
}
