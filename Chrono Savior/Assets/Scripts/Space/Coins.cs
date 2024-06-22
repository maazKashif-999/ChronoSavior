using UnityEngine;

public class Coin : MonoBehaviour
{
    float speed = 1.5f; // Speed at which the coin moves must be equal to background

    private void Update()
    {
        // Move the coin to the left
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
