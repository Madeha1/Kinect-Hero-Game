using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private int RotateSpeed = 1;
    private AudioSource coinFX;

    void Start() {
        var sounds = GameObject.FindObjectsOfType<AudioSource>();
        coinFX = sounds[2];
    }

    void Update()
    {
        transform.Rotate(0, RotateSpeed, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hips")
        {
            coinFX.Play();
            CollectableController.coinCount += 1;
            this.gameObject.SetActive(false);

        }
    }
}