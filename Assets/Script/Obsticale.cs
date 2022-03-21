using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Obsticale : MonoBehaviour
{
    private PlayerMove player;

    private HealthBar healthBar;

    private void Start() { 
        player = GameObject.FindObjectOfType<PlayerMove>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        Debug.Log(player.currentHealth);

    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Hips")
        {
            TakeDamage(5);

        }
    }

    void TakeDamage(int damage)
    {
        print("Before" + player.currentHealth);

        player.currentHealth -= damage;
        Debug.Log("After" + player.currentHealth);

        healthBar.SetHealth(player.currentHealth);

        if (player.currentHealth <= 0)
        {
            SceneManager.LoadScene(2);

        }

    }
}
