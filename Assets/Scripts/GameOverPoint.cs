using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPoint : MonoBehaviour
{
    public Text PointsText;

    void Start()
    {
        int score = CollectableController.coinCount;
        gameObject.SetActive(true);
        PointsText.text = score.ToString();
    }

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        PointsText.text = score.ToString();
    }
}
