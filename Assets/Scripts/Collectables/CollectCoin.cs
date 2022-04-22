using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    private AudioSource coinFX;
    private void Start()
    {
        coinFX = GameObject.FindObjectOfType<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hips") {
            coinFX.Play();
            CollectableControler.coinCount += 1;
            this.gameObject.SetActive(false);

        }
    }
}
