using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverPanel;

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private ObstacleSpawner spawner;

    private int score = 0;
    private bool isGameOver = false;

    private void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        scoreText.text = "0";
        gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnObstacleHit += OnGameOver;
        GameEvents.OnCollectibleCollected += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.OnObstacleHit -= OnGameOver;
        GameEvents.OnCollectibleCollected -= AddScore;
    }

    private void Update()
    {
        bool isClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool isSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if (isGameOver && (isClick || isSpace))
        {
            RestartGame();
        }
    }

    private void AddScore()
    {
        if (isGameOver) return;

        score++;
        scoreText.text = score.ToString();
    }

    private void OnGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);

        score = 0;
        scoreText.text = "0";

        player.ResetPlayer();

        spawner.ResetAll();
        spawner.StartSpawning();
    }
}