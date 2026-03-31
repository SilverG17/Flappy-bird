using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private Obstacle    obstaclePrefab;
    [SerializeField] private Collectible collectiblePrefab;

    [Header("World bounds")]
    [SerializeField] private float spawnX   =  10f;
    [SerializeField] private float despawnX = -10f;

    [Header("Parallax sync")]
    [Tooltip("Assign the foreground parallax layer so obstacles scroll at the same speed.")]
    [SerializeField] private ParallaxRepeatingLayer foregroundLayer;

    [SerializeField] private float currentScrollSpeed;
    [SerializeField] private float survivalTime;

    private ObjectPool<Obstacle>    _obstaclePool;
    private ObjectPool<Collectible> _collectiblePool;

    private float _spawnTimer;
    private bool  _isRunning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (obstaclePrefab == null || collectiblePrefab == null)
        {
            Debug.LogError("[ObstacleSpawner] Prefabs not assigned!", this);
            return;
        }

        _obstaclePool = new ObjectPool<Obstacle>(
            () => Instantiate(obstaclePrefab, transform),
            GameSettings.OBSTACLE_POOL_SIZE);

        _collectiblePool = new ObjectPool<Collectible>(
            () => Instantiate(collectiblePrefab, transform),
            GameSettings.COLLECTIBLE_POOL_SIZE);

        currentScrollSpeed = GameSettings.BASE_SCROLL_SPEED;
        StartSpawning();
    }

    public void StartSpawning()
    {
        _isRunning   = true;
        _spawnTimer  = GameSettings.OBSTACLE_INTERVAL;
        survivalTime = 0f;
    }

    public void StopSpawning()
    {
        _isRunning = false;
    }

    public void ResetAll()
    {
        _obstaclePool?.ReleaseAll();
        _collectiblePool?.ReleaseAll();
        survivalTime       = 0f;
        _spawnTimer        = GameSettings.OBSTACLE_INTERVAL;
        currentScrollSpeed = GameSettings.BASE_SCROLL_SPEED;
    }

    private void Update()
    {
        if (!_isRunning) return;

        survivalTime += Time.deltaTime;

        // Sync to the foreground parallax layer when assigned; otherwise self-ramp.
        if (foregroundLayer != null)
        {
            currentScrollSpeed = foregroundLayer.CurrentSpeed;
        }
        else
        {
            float speedBoost = Mathf.Floor(survivalTime / GameSettings.SCALE_INTERVAL)
                               * GameSettings.SPEED_INCREMENT;
            currentScrollSpeed = Mathf.Min(
                GameSettings.BASE_SCROLL_SPEED + speedBoost,
                GameSettings.MAX_SCROLL_SPEED);
        }

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= GameSettings.OBSTACLE_INTERVAL)
        {
            _spawnTimer = 0f;
            SpawnPair();
        }
    }

    private void SpawnPair()
    {
        float gapCenterY = Random.Range(GameSettings.GAP_MIN_Y, GameSettings.GAP_MAX_Y);
        float gapSize    = Random.Range(GameSettings.GAP_SIZE_MIN, GameSettings.GAP_SIZE_MAX);

        Obstacle obs = _obstaclePool.Get();
        obs.Setup(spawnX, gapCenterY, gapSize, currentScrollSpeed, despawnX);

        Collectible coin = _collectiblePool.Get();
        coin.Setup(spawnX, gapCenterY, currentScrollSpeed, despawnX);
    }

    public void ReturnObstacle(Obstacle obs)        => _obstaclePool?.Release(obs);
    public void ReturnCollectible(Collectible coin)  => _collectiblePool?.Release(coin);
}
