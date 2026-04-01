using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolable
{
    [Header("Child references")]
    [SerializeField] private Transform topPipe;
    [SerializeField] private Transform bottomPipe;
    [SerializeField] private float moveAmplitude = 1f;
    [SerializeField] private float moveSpeed = 2f;

    private float baseY;
    private float timeOffset;
    public float CurrentOffsetY { get; private set; }
    public bool IsActive => gameObject.activeSelf;

    public void OnSpawn()   => gameObject.SetActive(true);
    public void OnDespawn() => gameObject.SetActive(false);

    private float _despawnX;

    private void Awake()
    {
        if (topPipe == null)    topPipe    = transform.Find("TopPipe");
        if (bottomPipe == null) bottomPipe = transform.Find("BottomPipe");
    }

    public void Setup(float spawnX, float gapCenterY, float gapSize, float despawnX)
    {
        _despawnX = despawnX;

        transform.position = new Vector3(spawnX, gapCenterY, transform.position.z);

        float halfGap = gapSize * 0.5f;

        float topHalfHeight = GetPipeHalfHeight(topPipe);
        float botHalfHeight = GetPipeHalfHeight(bottomPipe);

        float topY = halfGap + topHalfHeight;
        float botY = -halfGap - botHalfHeight;

        topPipe.localPosition    = new Vector3(0f, topY, 0f);
        bottomPipe.localPosition = new Vector3(0f, botY, 0f);

        baseY = gapCenterY;
        timeOffset = Random.Range(0f, 10f);
    }

    private float GetPipeHalfHeight(Transform pipe)
    {
        var sr = pipe.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.y * 0.5f : 0.5f;
    }

    private void Update()
    {
        float speed = ObstacleSpawner.Instance.CurrentSpeed;

        // di chuyển ngang
        Vector3 pos = transform.position;
        pos.x += -speed * Time.deltaTime;

        // dao động Y
        CurrentOffsetY = Mathf.Sin((Time.time + timeOffset) * moveSpeed) * moveAmplitude;
        pos.y = baseY + CurrentOffsetY;

        transform.position = pos;

        if (pos.x < _despawnX)
        {
            ObstacleSpawner.Instance?.ReturnObstacle(this);
        }
    }
}