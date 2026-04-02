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
    [SerializeField] private GameObject startMenu;
    [SerializeField] private CanvasGroup startMenuCanvas;
    [SerializeField] private float menuScrollMultiplier = 0.5f;

    private bool isPlaying = false;
    private int score = 0;
    private bool isGameOver = false;

    private void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        scoreText.text = "0";
        player.gameObject.SetActive(true);
        startMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        spawner.StopSpawning();
        if (startMenuCanvas != null)
        {
            startMenuCanvas.alpha = 1f;
            startMenuCanvas.blocksRaycasts = true;
        }
        player.SetIdle(true);
        spawner.SetSpeedMultiplier(menuScrollMultiplier);
    }

    public void StartGame()
    {
        isPlaying = true;
        StartCoroutine(FadeOutMenu());
        spawner.SetSpeedMultiplier(1f); 
        spawner.StartSpawning();
        player.SetIdle(false); 
        player.ResetPlayer();
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
        if (SettingsMenuUI.IsOpen) return;

        bool isClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool isSpace = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        // 👉 Nếu chưa vào game thì click để start luôn
        if (!isPlaying && (isClick || isSpace))
        {
            StartGame();
            return;
        }

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
        isPlaying = true;

        isGameOver = false;
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);

        score = 0;
        scoreText.text = "0";

        player.ResetPlayer();

        spawner.ResetAll();
        spawner.StartSpawning();
    }

    private System.Collections.IEnumerator FadeOutMenu()
    {
        float t = 0f;
        float duration = 0.5f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1f - (t / duration);

            if (startMenuCanvas != null)
                startMenuCanvas.alpha = alpha;

            yield return null;
        }

        startMenu.SetActive(false);
    }
}
