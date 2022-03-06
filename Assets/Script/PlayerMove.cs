using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1;
    private Vector3 direction = Vector3.forward;
    
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

    void Update()
    {
        //transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);

    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("Finish");
            SceneManager.LoadScene(2);

        }

        if (other.gameObject.tag == "Obstacle")
        {
            TakeDamage(5);

        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            print(currentHealth);
            speed.moveSpeed = 0;
            gameOver.GameOver();
        }
    }
}
