using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ExtraShip : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float spawnInterval = 30f;
    public int shipScore = 300;

    private Vector2 leftEdgePosition;
    private Vector2 rightEdgePosition;
    private int moveDirection = -1;
    private bool isSpawned;

    private void Start()
    {
        Vector3 screenLeftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 screenRightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        leftEdgePosition = new Vector2(screenLeftEdge.x - 1f, transform.position.y);
        rightEdgePosition = new Vector2(screenRightEdge.x + 1f, transform.position.y);

        HideShip();
    }

    private void Update()
    {
        if (!isSpawned) return;

        if (moveDirection == 1) {
            MoveToRight();
        } else {
            MoveToLeft();
        }
    }

    private void MoveToRight()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.right;

        if (transform.position.x >= rightEdgePosition.x) {
            HideShip();
        }
    }

    private void MoveToLeft()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x <= leftEdgePosition.x) {
            HideShip();
        }
    }

    private void ShowShip()
    {
        moveDirection *= -1;

        if (moveDirection == 1) {
            transform.position = leftEdgePosition;
        } else {
            transform.position = rightEdgePosition;
        }

        isSpawned = true;
    }

    private void HideShip()
    {
        isSpawned = false;

        if (moveDirection == 1) {
            transform.position = rightEdgePosition;
        } else {
            transform.position = leftEdgePosition;
        }

        Invoke(nameof(ShowShip), spawnInterval);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            HideShip();
            GameManager.Instance.OnBonusShipKilled(this);
        }
    }

}
