using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;           // Warna merah, update langsung
    public Slider easeHealthSlider;       // Warna oranye, efek smooth

    public float maxHealth = 100f;
    public float health;

    public float lerpSpeed = 2f;          // Kecepatan transisi smooth bar

    void Start()
    {
        health = maxHealth;

        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;

        healthSlider.value = maxHealth;
        easeHealthSlider.value = maxHealth;
    }

    void Update()
    {
        // Update healthSlider langsung
        healthSlider.value = health;

        // Smooth update easeHealthSlider (jika lebih besar)
        if (easeHealthSlider.value > health)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, Time.deltaTime * lerpSpeed);
        }
        else
        {
            easeHealthSlider.value = health;
        }

        // Uji coba damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(10);
        }
    }

    void takeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
