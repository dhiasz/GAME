using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public PlayerMovements playerMovements;

    // Combo system
    private int attackStep = 0;
    private float lastClickTime = 10f;
    public float comboResetTime = 5f;

    private bool isComboWindowOpen = false;
    private bool inputBuffered = false;

    // Damage system
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public float[] attackDamages = new float[] { 10f, 12f, 14f, 16f };

    void Update()
    {
        
        // Reset combo if terlalu lama tidak menyerang
        if (Time.time - lastClickTime > comboResetTime)
        {
            attackStep = 0;
        }

        // Klik mouse kiri
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time;

            if (attackStep == 0)
            {
                StartCombo();
            }
            else if (isComboWindowOpen)
            {
                inputBuffered = true;
            }
        }
    }

    private void StartCombo()
    {
        attackStep = 1;
        PlayAttackAnimation(attackStep);
    }

    private void PlayAttackAnimation(int step)
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Attack4");

        animator.SetTrigger($"Attack{step}");

        isComboWindowOpen = false;
        inputBuffered = false;
    }

    // Animation Event: called in the middle of the animation
    public void OpenComboWindow()
    {
        isComboWindowOpen = true;
        inputBuffered = false;
        Debug.Log("Combo Window OPEN");
    }

    // Animation Event: called at the end of the animation
    public void CloseComboWindow()
    {
        isComboWindowOpen = false;
        Debug.Log("Combo Window CLOSED");

        if (inputBuffered)
        {
            Debug.Log("Combo Continued");
            attackStep++;
            if (attackStep > attackDamages.Length)
                attackStep = 1;

            PlayAttackAnimation(attackStep);
        }
        else
        {
            Debug.Log("Combo Ended");
            attackStep = 0;
        }
    }

    // Animation Event: called when attack hits
    public void DealDamage()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            healthBar healthBar = enemy.GetComponent<healthBar>();
            if (healthBar != null)
            {
                int index = Mathf.Clamp(attackStep - 1, 0, attackDamages.Length - 1);
                float damage = attackDamages[index];
                healthBar.takeDamage(damage);
            }
        }
    }

}

