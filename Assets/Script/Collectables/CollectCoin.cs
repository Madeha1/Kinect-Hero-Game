﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public AudioSource coinFX;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hips")
        {
            coinFX.Play();
            CollectibleControler.coinCount += 1;
            this.gameObject.SetActive(false);

        }

    }
}
