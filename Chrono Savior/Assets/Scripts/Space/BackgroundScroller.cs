using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    private float scrollSpeed = 0.05f;
    private Renderer bgRenderer;

    private void Start()
    {
        bgRenderer = GetComponent<Renderer>();
        if (bgRenderer == null)
        {
            Debug.LogError("Renderer component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (bgRenderer != null && bgRenderer.material != null)
        {
            bgRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
        }
        else if (bgRenderer == null)
        {
            Debug.LogError("Renderer component is null in Update.");
        }
        else
        {
            Debug.LogError("Material is null in Update.");
        }
    }
}
