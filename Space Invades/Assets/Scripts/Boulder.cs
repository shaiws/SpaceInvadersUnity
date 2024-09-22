using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Boulder : MonoBehaviour
{
    public Texture2D hitEffect;
    private Texture2D originalImage;
    private SpriteRenderer boulderRenderer;
    private BoxCollider2D boulderCollider;

    private void Awake()
    {
        boulderRenderer = GetComponent<SpriteRenderer>();
        boulderCollider = GetComponent<BoxCollider2D>();
        originalImage = boulderRenderer.sprite.texture;

        ResetBoulder();
    }

    public void ResetBoulder()
    {
        DuplicateTexture(originalImage);
        gameObject.SetActive(true);
    }

    private void DuplicateTexture(Texture2D source)
    {
        Texture2D newTexture = new Texture2D(source.width, source.height, source.format, false)
        {
            filterMode = source.filterMode,
            anisoLevel = source.anisoLevel,
            wrapMode = source.wrapMode
        };

        newTexture.SetPixels32(source.GetPixels32());
        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, boulderRenderer.sprite.rect, new Vector2(0.5f, 0.5f), boulderRenderer.sprite.pixelsPerUnit);
        boulderRenderer.sprite = newSprite;
    }

    public bool DetectCollision(BoxCollider2D otherCollider, Vector3 collisionPoint)
    {
        Vector2 offset = otherCollider.size / 2;

        return ApplyHitEffect(collisionPoint) ||
               ApplyHitEffect(collisionPoint + (Vector3.down * offset.y)) ||
               ApplyHitEffect(collisionPoint + (Vector3.up * offset.y)) ||
               ApplyHitEffect(collisionPoint + (Vector3.left * offset.x)) ||
               ApplyHitEffect(collisionPoint + (Vector3.right * offset.x));
    }

    private bool ApplyHitEffect(Vector3 collisionPoint)
    {
        if (!MapToPixel(collisionPoint, out int px, out int py)) {
            return false;
        }

        Texture2D texture = boulderRenderer.sprite.texture;

        px -= hitEffect.width / 2;
        py -= hitEffect.height / 2;

        int startX = px;

        for (int y = 0; y < hitEffect.height; y++)
        {
            px = startX;

            for (int x = 0; x < hitEffect.width; x++)
            {
                Color pixel = texture.GetPixel(px, py);
                pixel.a *= hitEffect.GetPixel(x, y).a;
                texture.SetPixel(px, py, pixel);
                px++;
            }

            py++;
        }

        texture.Apply();

        return true;
    }

    private bool MapToPixel(Vector3 collisionPoint, out int px, out int py)
    {
        Vector3 localPoint = transform.InverseTransformPoint(collisionPoint);

        localPoint.x += boulderCollider.size.x / 2;
        localPoint.y += boulderCollider.size.y / 2;

        Texture2D texture = boulderRenderer.sprite.texture;

        px = (int)(localPoint.x / boulderCollider.size.x * texture.width);
        py = (int)(localPoint.y / boulderCollider.size.y * texture.height);

        return texture.GetPixel(px, py).a != 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            gameObject.SetActive(false);
        }
    }

}
