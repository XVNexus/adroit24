using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed as needed

    // Ensure there's only one Update method
    void Update()
    {
        // Get input from WASD keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the character
        GetComponent<Rigidbody>().MovePosition(transform.position + movement * speed * Time.deltaTime);
    }
}
