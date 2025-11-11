using UnityEngine;

[ExecuteAlways] // So it updates in Edit Mode too
[RequireComponent(typeof(SpriteRenderer))]
public class CardSizeManagement : MonoBehaviour
{
    public RectTransform targetRect; // Assign the parent RectTransform here

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer cardImage;

    // Base dimensions (you can also compute this automatically in Awake)
    [SerializeField] private Vector2 baseCardSize;// = new Vector2(2.4f, 3.3333f);
    [SerializeField] private Vector2 baseImageSize;// = new Vector2(2f, 1.6f);

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

        if (cardImage != null)
        {
            // Compute the scaling ratio compared to the base card size
            float widthRatio = rectSize.x / baseCardSize.x;
            float heightRatio = rectSize.y / baseCardSize.y;

            // Apply the same proportional scaling to the image
            cardImage.drawMode = SpriteDrawMode.Sliced;
            cardImage.size = new Vector2(
                baseImageSize.x * widthRatio,
                baseImageSize.y * heightRatio
            );
        }
    }
}
