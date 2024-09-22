using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    private void Awake()
    {
        initialPosition = transform.position;

        CreateInvaderGrid();
    }

    private void CreateInvaderGrid()
{
    float invaderSpacing = 3f; 
    float verticalOffset = 2f; 

    for (int i = 0; i < rows; i++)
    {
        float width = invaderSpacing * (columns - 1);
        float height = invaderSpacing * (rows - 1);

        Vector2 centerOffset = new Vector2(-width / 2f, -height / 2f + verticalOffset);
        Vector3 rowPosition = new Vector3(centerOffset.x, (invaderSpacing * i) + centerOffset.y, 0f);

        for (int j = 0; j < columns; j++)
        {
            Invader invader = Instantiate(prefabs[i], transform);

            Vector3 position = rowPosition;
            position.x += invaderSpacing * j;
            invader.transform.localPosition = position;
        }
    }
}


    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

       
            if (Random.value < (1f / amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = amountKilled / (float)totalCount;

        float speed = this.speed.Evaluate(percentKilled);
        transform.position += speed * Time.deltaTime * direction;


        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);


        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0f, 0f);

        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) {
                count++;
            }
        }

        return count;
    }

}