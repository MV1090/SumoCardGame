using UnityEngine;

[ExecuteAlways] // So it updates in Edit Mode too
[RequireComponent(typeof(SpriteRenderer))]
public class CardSizeManagement : MonoBehaviour
{
    public RectTransform targetRect; // Assign the parent RectTransform here

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetRect == null || spriteRenderer == null)
            return;

        // Get the RectTransform’s size in world units
        Vector2 rectSize = targetRect.rect.size;
        Vector3 lossyScale = targetRect.lossyScale;
        rectSize.Scale(new Vector2(lossyScale.x, lossyScale.y));

        // Assign it to the SpriteRenderer’s size
        spriteRenderer.drawMode = SpriteDrawMode.Sliced; // or Tiled
        spriteRenderer.size = rectSize;
    }
}
