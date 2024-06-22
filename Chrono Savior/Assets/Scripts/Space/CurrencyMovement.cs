using UnityEngine;

public class Coin : MonoBehaviour
{
    float speed = 1.5f; // Speed at which the coin moves must be equal to background
    private float screenEdgeX;

    private void Start()
    {
        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    private void Update()
    {
        // Move the coin to the left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Check if the coin has moved off the left side of the screen
        if (transform.position.x < screenEdgeX)
        {
            Destroy(gameObject);
        }
    }
}
