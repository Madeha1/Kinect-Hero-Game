using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Obsticale : MonoBehaviour
{
    private PlayerMove player;
    private HealthBar healthBar;
    private GameObject redScreen;

    private void Start() { 
        player = GameObject.FindObjectOfType<PlayerMove>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        redScreen = GameObject.FindWithTag("redScreen");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hips")
        {
            TakeDamage(5);
            gotHurt();
        }
    }

    void TakeDamage(int damage)
    {
        player.currentHealth -= damage;
        healthBar.SetHealth(player.currentHealth);

        if (player.currentHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    void gotHurt()
    {
        var color = redScreen.GetComponent<Image>().color;
        color.a = 0.8f;
        redScreen.GetComponent<Image>().color = color;
    }

    void Update()
    {
        if(redScreen != null)
        {
            if(redScreen.GetComponent<Image>().color.a > 0)
            {
                var color = redScreen.GetComponent<Image>().color;
                color.a -= 0.004f;
                redScreen.GetComponent<Image>().color = color;

            }
        }
    }
}
