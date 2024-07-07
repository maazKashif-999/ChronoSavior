using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    private float scrollSpeed = 0.05f;
    private Renderer bgRenderer;

    private void Start() {
        bgRenderer = GetComponent<Renderer>();
    }
    void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(scrollSpeed*Time.deltaTime,0);
    }
}
