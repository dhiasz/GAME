using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Orang; // Objek pemain
    public Vector3 offset = new Vector3(0, 3, -5); // Jarak kamera dari pemain
    public float rotationSpeed = 2f; // Sensitivitas mouse

    private float yaw = 0f; // Rotasi horizontal
    private float pitch = 0f; // Rotasi vertikal

    private void LateUpdate()
    {
        if (Orang == null) return;

        // Mengikuti posisi pemain
        transform.position = Orang.position + offset;

        // Mendapatkan input mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // Batasi rotasi vertikal agar tidak terbalik

        // Rotasi kamera berdasarkan input mouse
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
