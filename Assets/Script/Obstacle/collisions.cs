using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisions : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public CollectibleControler gameOver;
    public Change speed;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    private void OnTriggerEnter(Collider other)
    {
        TakeDamage(30);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            speed.moveSpeed = 0;
            gameOver.GameOver();
        }
    }
}
