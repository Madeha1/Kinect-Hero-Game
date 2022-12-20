using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int MaxHealth = 100;
    public HealthBar _healthBar;
    private int _currentHealth;
    private GameObject _redScreen;

    void Start()
    {
        _currentHealth = MaxHealth;
        _healthBar.SetMaxHealth(MaxHealth);
        _redScreen = GameObject.FindWithTag("redScreen");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("Finish");
            SceneManager.LoadScene(2);
        }
    }

    public void decreaseHealth(int damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);
        var color = _redScreen.GetComponent<Image>().color;
        color.a = 0.8f;
        _redScreen.GetComponent<Image>().color = color;

        if (_currentHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    void Update()
    {
        if (_redScreen != null)
        {
            if (_redScreen.GetComponent<Image>().color.a > 0)
            {
                var color = _redScreen.GetComponent<Image>().color;
                color.a -= 0.004f;
                _redScreen.GetComponent<Image>().color = color;

            }
        }
    }
}
