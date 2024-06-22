using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    float scrollSpeed = 0.3f;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        startPos = new Vector2(startPos.x - scrollSpeed*Time.deltaTime,startPos.y);
        transform.position = startPos;
    }
}
