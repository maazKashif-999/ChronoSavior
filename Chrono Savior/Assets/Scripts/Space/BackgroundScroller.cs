using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    private float scrollSpeed = 0.3f;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (transform != null)
        {
            // Calculate new position
            Vector3 newPos = new Vector3(transform.position.x - scrollSpeed * Time.deltaTime, startPos.y, 0);
            // Update the position
            transform.position = newPos;
        }
    }
}
