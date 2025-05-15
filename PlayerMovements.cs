using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(0, value);
    }

    private Animator animator; // Animator karakter

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        if (moveAction == null)
        {
            Debug.LogError("Move action not found! Check your Input Actions settings.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Atur animasi
        UpdateAnimation(inputDirection);
    }

    private void UpdateAnimation(Vector2 input)
    {
        if (animator == null) return;

        float threshold = 0.1f;

        animator.SetBool("isMovingForward", input.y > threshold);
        animator.SetBool("isMovingBackward", input.y < -threshold);
        animator.SetBool("isMovingRight", input.x > threshold);
        animator.SetBool("isMovingLeft", input.x < -threshold);
    }


    public void SetSpeed(float newSpeed)
    {
        MoveSpeed = newSpeed;
    }
}
