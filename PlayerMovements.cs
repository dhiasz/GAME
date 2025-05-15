using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    public float moveSpeed = 5f; // Kecepatan gerakan

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        if (moveAction == null)
        {
            Debug.LogError("Move action not found! Check your Input Actions settings.");
        }
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>(); // Menggunakan Vector2
        Debug.Log("Input: " + inputDirection); // Debugging untuk melihat nilai input

        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y); // Gunakan x dan y
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
