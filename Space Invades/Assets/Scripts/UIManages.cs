using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public Text scoreLabel;
    public Text livesLabel;

    public Text levelLabel;

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        GameManager.Instance.OnGameOver += ShowGameOverUI;
        GameManager.Instance.OnLevelChanged += UpdateLevel;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnLivesChanged -= UpdateLives;
            GameManager.Instance.OnGameOver -= ShowGameOverUI;
            GameManager.Instance.OnLevelChanged -= UpdateLevel;
        }
    }

    private void UpdateUI()
    {
        scoreLabel.text = GameManager.Instance.Points.ToString("D4");
        livesLabel.text = GameManager.Instance.Health.ToString();
        levelLabel.text = GameManager.Instance.CurrentLevel.ToString();
        gameOverUI.SetActive(false);
    }

    private void UpdateScore(int newScore)
    {
        scoreLabel.text = newScore.ToString("D4");
    }

    private void UpdateLives(int newLives)
    {
        livesLabel.text = newLives.ToString();
    }

    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    private void UpdateLevel(int newLevel)
    {
        levelLabel.text = newLevel.ToString();
    }
}
