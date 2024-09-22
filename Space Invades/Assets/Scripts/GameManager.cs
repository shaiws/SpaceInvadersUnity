using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // [SerializeField] private Text scoreLabel;
    // [SerializeField] private Text livesLabel;

    private Player mainPlayer;
    private Invaders enemyGroup;
    private ExtraShip bonusShip;
    private Boulder[] rocks;

    public int points { get; private set; } = 0;
    public int health { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        mainPlayer = FindObjectOfType<Player>();
        enemyGroup = FindObjectOfType<Invaders>();
        bonusShip = FindObjectOfType<ExtraShip>();
        rocks = FindObjectsOfType<Boulder>();

        StartGame();
    }

    private void Update()
    {
        if (health <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        }
    }

    private void StartGame()
    {
        UpdateScore(0);
        UpdateLives(3);
        StartRound();
    }

    private void StartRound()
    {
        enemyGroup.ResetInvaders();
        enemyGroup.gameObject.SetActive(true);

        for (int i = 0; i < rocks.Length; i++) {
            rocks[i].ResetBoulder();
        }

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Vector3 playerPos = mainPlayer.transform.position;
        playerPos.x = 0f;
        mainPlayer.transform.position = playerPos;
        mainPlayer.gameObject.SetActive(true);
    }

    private void UpdateScore(int newScore)
    {
        points = newScore;
        // scoreLabel.text = points.ToString().PadLeft(4, '0');
    }

    private void UpdateLives(int newLives)
    {
        health = Mathf.Max(newLives, 0);
        // livesLabel.text = health.ToString();
    }

    public void OnPlayerDeath(Player player)
    {
        UpdateLives(health - 1);

        player.gameObject.SetActive(false);

        if (health > 0) {
            Invoke(nameof(StartRound), 1f);
        } else {
            // EndGame();
        }
    }

    public void OnEnemyKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        UpdateScore(points + invader.alienScore);

        if (enemyGroup.GetAliveCount() == 0) {
            StartRound();
        }
    }

    public void OnBonusShipKilled(ExtraShip extraShip)
    {
        UpdateScore(points + extraShip.shipScore);
    }

    public void OnEdgeReached()
    {
        if (enemyGroup.gameObject.activeSelf)
        {
            enemyGroup.gameObject.SetActive(false);
            OnPlayerDeath(mainPlayer);
        }
    }
}
