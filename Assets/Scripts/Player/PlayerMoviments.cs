using UnityEngine;

public class PlayerMoviments : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento
    public float rotationSpeed = 720f;  // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Movimiento del jugador
        float moveDirection = Input.GetAxis("Vertical"); // W y S (o flechas) controlan hacia adelante y atrás
        float rotationDirection = Input.GetAxis("Horizontal"); // A y D (o flechas) controlan la rotación

        // Movimiento hacia adelante y atrás
        transform.Translate(Vector3.forward * moveDirection * moveSpeed * Time.deltaTime);

        // Rotación izquierda y derecha
        transform.Rotate(Vector3.up * rotationDirection * rotationSpeed * Time.deltaTime);
    }
}
