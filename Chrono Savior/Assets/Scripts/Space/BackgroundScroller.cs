using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
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
            Vector2 newPos = new Vector2(startPos.x - scrollSpeed * Time.deltaTime, startPos.y);
            transform.position = newPos;
        }
    }
}
