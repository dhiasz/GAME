using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;       // Slider UI utama (langsung)
    public Slider easeHealthSlider;   // Slider UI efek smoothing (optional)

    public float maxHealth = 100f;
    public float health;

    public float lerpSpeed = 2f;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        health = maxHealth;

        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;

        if (easeHealthSlider != null)
            easeHealthSlider.maxValue = maxHealth;

        if (healthSlider != null)
            healthSlider.value = maxHealth;

        if (easeHealthSlider != null)
            easeHealthSlider.value = maxHealth;

        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("Animator not found on " + gameObject.name);
    }

    void Update()
    {
        if (healthSlider != null)
            healthSlider.value = health;

        if (easeHealthSlider != null)
        {
            if (easeHealthSlider.value > health)
                easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, Time.deltaTime * lerpSpeed);
            else
                easeHealthSlider.value = health;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(50);
        }
    }

    public void takeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage, health now: {health}");

        if (health <= 0 && !isDead)
        {
            Die();
        }

    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");  // Pastikan ada trigger "Die" di Animator
        }

        // Contoh: nonaktifkan movement script (jika ada)
        var playerMovements = GetComponent<PlayerMovements>();
        if (playerMovements != null)
            playerMovements.enabled = false;

        var navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;

        Debug.Log($"{gameObject.name} is dead!");

        // Jika ingin hapus objek setelah delay
        Destroy(gameObject, 3f);
    }
}
