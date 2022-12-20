using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableController : MonoBehaviour
{
    public static int coinCount;
    public GameObject coinCountDisplay;
    public Text highScore;

    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HightScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        coinCountDisplay.GetComponent<Text>().text = "" + coinCount;

        if (coinCount > PlayerPrefs.GetInt("HightScore", 0))
        {
            PlayerPrefs.SetInt("HightScore", coinCount);
            highScore.text = coinCount.ToString();
        }

    }
}
