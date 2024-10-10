using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Boulder : MonoBehaviour
{
    private BoxCollider2D boulderCollider;

    private void Awake()
    {
        boulderCollider = GetComponent<BoxCollider2D>();

        ResetBoulder();
    }

    public void ResetBoulder()
    {
        gameObject.SetActive(true);
    }

    public bool DetectCollision(BoxCollider2D otherCollider, Vector3 collisionPoint)
    {
        Vector2 offset = otherCollider.size / 2;

        return CheckCollisionPoint(collisionPoint) ||
               CheckCollisionPoint(collisionPoint + (Vector3.down * offset.y)) ||
               CheckCollisionPoint(collisionPoint + (Vector3.up * offset.y)) ||
               CheckCollisionPoint(collisionPoint + (Vector3.left * offset.x)) ||
               CheckCollisionPoint(collisionPoint + (Vector3.right * offset.x));
    }

    private bool CheckCollisionPoint(Vector3 collisionPoint)
    {
        Vector3 localPoint = transform.InverseTransformPoint(collisionPoint);
        localPoint.x += boulderCollider.size.x / 2;
        localPoint.y += boulderCollider.size.y / 2;

        return true;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }
    }
}
