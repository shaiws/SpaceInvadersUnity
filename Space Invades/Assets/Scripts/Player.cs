using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    [SerializeField] private Transform leftBoundary; 
    [SerializeField] private Transform rightBoundary;

    public  Projectile laser { get; set; } 
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
    }

    private void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }

        position.x = Mathf.Clamp(position.x, leftBoundary.position.x, rightBoundary.position.x);

        transform.position = position;

        if (laser == null && Input.GetKeyDown(KeyCode.Space) ) {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            GameManager.Instance.OnPlayerDeath(this);
        }
    }
}
