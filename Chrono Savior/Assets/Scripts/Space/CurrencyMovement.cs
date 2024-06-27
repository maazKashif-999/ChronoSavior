using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.5f; // Speed at which the coin moves

    private float screenEdgeX;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenEdgeX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }
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
