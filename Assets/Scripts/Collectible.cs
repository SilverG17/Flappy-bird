using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Collectible : MonoBehaviour, IPoolable
{
    [Header("Animation")]
    [SerializeField] private Sprite[] frames;
    private Obstacle linkedObstacle;

    public bool IsActive => gameObject.activeSelf;

    public void OnSpawn() => gameObject.SetActive(true);

    public void OnDespawn()
    {
        gameObject.SetActive(false);
        _animTimer    = 0f;
        _currentFrame = 0;
    }

    private SpriteRenderer   _sr;
    private CircleCollider2D _col;

    private float _despawnX;

    private float _animTimer;
    private int   _currentFrame;

    private void Awake()
    {
        _sr  = GetComponent<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
        _col.isTrigger = true;
    }

    public void Setup(float spawnX, float gapCenterY, float despawnX, Obstacle obstacle)
    {
        _despawnX = despawnX;
        transform.position = new Vector3(spawnX, gapCenterY, 0f);
        linkedObstacle = obstacle;
    }

    private void Update()
    {
        float speed = ObstacleSpawner.Instance.CurrentSpeed;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < _despawnX)
        {
            ObstacleSpawner.Instance?.ReturnCollectible(this);
            return;
        }

        if (frames == null || frames.Length == 0) return;

        _animTimer += Time.deltaTime;
        float frameDuration = 1f / GameSettings.COIN_ANIM_FPS;

        if (_animTimer >= frameDuration)
        {
            _animTimer -= frameDuration;
            _currentFrame = (_currentFrame + 1) % frames.Length;
            _sr.sprite = frames[_currentFrame];
        }
        if (linkedObstacle != null)
        {
            Vector3 pos = transform.position;
            // theo obstacle
            pos.y = linkedObstacle.transform.position.y;
            pos.y += Mathf.Sin(Time.time * 3f) * 0.2f;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameEvents.CollectibleCollected();
        ObstacleSpawner.Instance?.ReturnCollectible(this);
    }
}