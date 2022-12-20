using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    private Player _player;
    private AudioSource _hitRock;
    private int damage = 5;


    private void Start() { 
        _player = GameObject.FindObjectOfType<Player>();

        var sounds = GameObject.FindObjectsOfType<AudioSource>();
        _hitRock = sounds[1];

        //var sounds = GameObject.FindObjectsOfType<AudioSource>();
        //_hitRock = sounds[1];

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hips") {
            _hitRock.Play();
            TakeDamage(damage);
        }
    }

    void TakeDamage(int damage)
    {
        _player.decreaseHealth(damage);
        
    }
}
