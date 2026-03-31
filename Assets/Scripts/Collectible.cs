using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Collectible : MonoBehaviour, IPoolable
{
    [Header("Animation")]
    [SerializeField] private Sprite[] frames;

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

    private float _scrollSpeed;
    private float _despawnX;

    private float _animTimer;
    private int   _currentFrame;

    private void Awake()
    {
        _sr  = GetComponent<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
        _col.isTrigger = true;
    }

    public void Setup(float spawnX, float gapCenterY, float scrollSpeed, float despawnX)
    {
        _scrollSpeed = scrollSpeed;
        _despawnX    = despawnX;
        transform.position = new Vector3(spawnX, gapCenterY, transform.position.z);
    }

    private void Update()
    {
        transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameEvents.CollectibleCollected();
        ObstacleSpawner.Instance?.ReturnCollectible(this);
    }
}
