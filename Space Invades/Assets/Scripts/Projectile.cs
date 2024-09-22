using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        Boulder boulder = other.gameObject.GetComponent<Boulder>();

        if (boulder == null || boulder.DetectCollision(boxCollider, transform.position)) {
            Destroy(gameObject);
        }
    }

}
