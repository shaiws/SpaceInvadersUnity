using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event System.Action<int> OnScoreChanged;
    public event System.Action<int> OnLivesChanged;
    public event System.Action<int> OnLevelChanged;
    public event System.Action OnGameOver;


    private Player mainPlayer;
    private Invaders enemyGroup;
    private Boulder[] boulders;

    public int Points { get; private set; } = 0;
    public int Health { get; private set; } = 3;

    public int CurrentLevel { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        InitializeGameObjects();
        StartGame();
    }

    private void InitializeGameObjects()
    {
        mainPlayer = FindObjectOfType<Player>();
        enemyGroup = FindObjectOfType<Invaders>();
        boulders = FindObjectsOfType<Boulder>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGameObjects();
        StartRound();
    }

    private void Update()
    {
        if (Health <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
        }
    }

   
    public void StartGame()
    {
        CurrentLevel = 1; 
        Points = 0;
        Health = 3;
        OnScoreChanged?.Invoke(Points);
        OnLivesChanged?.Invoke(Health);
        SceneManager.LoadScene(GetSceneIndexForLevel(CurrentLevel));
    }

  
    private void RestartGame()
    {
        StartGame();
    }

       private int GetSceneIndexForLevel(int level)
    {
        return level;
    }

    public void StartRound()
    {
        enemyGroup.ResetInvaders();
        enemyGroup.gameObject.SetActive(true);

        foreach (var boulder in boulders)
        {
            boulder.ResetBoulder();
        }

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Vector3 playerPos = mainPlayer.transform.position;
        playerPos.x = 0f;
        mainPlayer.transform.position = playerPos;
        mainPlayer.laser = null;
        mainPlayer.gameObject.SetActive(true);
    }

    private void EndGame()
    {
        OnGameOver?.Invoke();
        enemyGroup.gameObject.SetActive(false);
    }

    public void OnPlayerDeath(Player player)
    {
        Health = Mathf.Max(Health - 1, 0);
        OnLivesChanged?.Invoke(Health);

        player.gameObject.SetActive(false);

        if (Health > 0)
        {
            Invoke(nameof(StartRound), 1f);
        }
        else
        {
            EndGame();
        }
    }

    public void OnEnemyKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        Points += invader.alienScore;
        OnScoreChanged?.Invoke(Points);

        if (enemyGroup.GetAliveCount() == 0)
        {
            OnLevelChanged?.Invoke(CurrentLevel);
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        CurrentLevel++;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2) {
            SceneManager.LoadScene(1); 
            CurrentLevel = 1; 
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
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
