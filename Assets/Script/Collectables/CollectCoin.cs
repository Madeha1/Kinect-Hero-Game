using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    //public AudioSource coinFX;
    private AudioSource coinFX;

    private CollectibleControler collectibleControler;


    private void Start()
    {
        coinFX = GameObject.FindObjectOfType<AudioSource>();
        collectibleControler = GameObject.FindObjectOfType<CollectibleControler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hips") {
            print("coins" + collectibleControler.coinCount);
            coinFX.Play();
            collectibleControler.coinCount += 1;
            this.gameObject.SetActive(false);

        }
        //this.gameObject.SetActive(false);
    }
}
