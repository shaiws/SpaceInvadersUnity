using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    public float spriteChangeTime = 1f;
    public int alienScore = 10;

    private SpriteRenderer alienRenderer;
    

    private void Awake()
    {
        alienRenderer = GetComponent<SpriteRenderer>();
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
