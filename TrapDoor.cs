using UnityEngine;
using System.Collections;

public class TrapDoor : MonoBehaviour
{
    public float rotateAngle = 90f;         // Derajat rotasi jebakan
    public float rotateSpeed = 200f;        // Kecepatan rotasi
    public float returnDelay = 1f;          // Tunggu sebelum kembali
    private bool isActivated = false;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        targetRotation = initialRotation * Quaternion.Euler(Vector3.up * rotateAngle); // Rotasi di sumbu Y (kiri ke kanan)
    }

    void Update()
    {
        StartCoroutine(ActivateTrap());
    }

    IEnumerator ActivateTrap()
    {
        isActivated = true;

        // Putar ke target
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        // Kembali ke awal
        while (Quaternion.Angle(transform.localRotation, initialRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, initialRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        isActivated = false;
    }
}
