using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    private float scrollSpeed = 0.05f;
    private Renderer renderer;

    private void Start() {
        renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        renderer.material.mainTextureOffset += new Vector2(scrollSpeed*Time.deltaTime,0);
    }
}
