using UnityEngine;
using System.Collections;

public class TrapDoor : MonoBehaviour
{
    public float delay = 2f;
    public Animator animator;
    public string activateTrigger = "Activate";

    private void Start()
    {
        InvokeRepeating(nameof(ActivateTrap), 0f, delay);
    }

    private void ActivateTrap()
    {
        if (animator != null)
        {
            animator.SetTrigger(activateTrigger);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthBar hb = other.GetComponent<healthBar>();
            if (hb != null && !IsDead(hb))
            {
                // Kasih damage sebesar maxHealth agar langsung mati
                hb.takeDamage(hb.maxHealth);
            }
        }
    }

    private bool IsDead(healthBar hb)
    {
        return hb.health <= 0;
    }
}