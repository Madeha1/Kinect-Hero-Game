using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsticale : MonoBehaviour
{
    public CollectibleControler gameOver;
    public Change speed;
    public PlayerMove player;

    public HealthBar healthBar;


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Hips")
        {
            TakeDamage(5);

        }
    }

    void TakeDamage(int damage)
    {
        player.currentHealth -= damage;
        Debug.Log(player.currentHealth);

        healthBar.SetHealth(player.currentHealth);

        if (player.currentHealth <= 0)
        {
            print(player.currentHealth);
            speed.moveSpeed = 0;
            gameOver.GameOver();
        }

    }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }
}
