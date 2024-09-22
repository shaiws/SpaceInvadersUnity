using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    public Sprite[] alienSprites = new Sprite[0];
    public float spriteChangeTime = 1f;
    public int alienScore = 10;

    private SpriteRenderer alienRenderer;
    private int currentFrame;

    private void Awake()
    {
        alienRenderer = GetComponent<SpriteRenderer>();
        alienRenderer.sprite = alienSprites[0];
    }

    private void Start()
    {
        InvokeRepeating(nameof(ChangeSprite), spriteChangeTime, spriteChangeTime);
    }

    private void ChangeSprite()
    {
        currentFrame++;

        if (currentFrame >= alienSprites.Length) {
            currentFrame = 0;
        }

        alienRenderer.sprite = alienSprites[currentFrame];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            GameManager.Instance.OnEnemyKilled(this);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Bound")) {
            GameManager.Instance.OnEdgeReached();
        }
    }

}
