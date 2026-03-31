using UnityEngine;
public class Obstacle : MonoBehaviour, IPoolable
{
    [Header("Child references")]
    [SerializeField] private Transform topPipe;
    [SerializeField] private Transform bottomPipe;
    [SerializeField] private float pipeHeight = 10f;
    public bool IsActive => gameObject.activeSelf;
    public void OnSpawn()   => gameObject.SetActive(true);
    public void OnDespawn() => gameObject.SetActive(false);
    private float _scrollSpeed;
    private float _despawnX;
    public float ScrollSpeed
    {
        get => _scrollSpeed;
        set => _scrollSpeed = value;
    }

    private void Awake()
    {
        if (topPipe    == null) topPipe    = transform.Find("TopPipe");
        if (bottomPipe == null) bottomPipe = transform.Find("BottomPipe");
    }

    public void Setup(float spawnX, float gapCenterY, float gapSize, float scrollSpeed, float despawnX)
    {
        _scrollSpeed = scrollSpeed;
        _despawnX    = despawnX;

        transform.position = new Vector3(spawnX, 0f, transform.position.z);

        float halfGap   = gapSize * 0.5f;
        float topPivotY = gapCenterY + halfGap + pipeHeight * 0.5f;
        float botPivotY = gapCenterY - halfGap - pipeHeight * 0.5f;

        topPipe.localPosition    = new Vector3(0f, topPivotY, 0f);
        bottomPipe.localPosition = new Vector3(0f, botPivotY, 0f);

        ResizePipeCollider(topPipe,    pipeHeight);
        ResizePipeCollider(bottomPipe, pipeHeight);
    }

    private void Update()
    {
        transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

        if (transform.position.x < _despawnX)
            ObstacleSpawner.Instance?.ReturnObstacle(this);
    }

    private static void ResizePipeCollider(Transform pipe, float height)
    {
        var col = pipe.GetComponent<BoxCollider2D>();
        if (col != null)
            col.size = new Vector2(GameSettings.PIPE_WIDTH, height);
    }
}
