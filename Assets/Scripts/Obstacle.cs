using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolable
{
    [Header("Child references")]
    [SerializeField] private Transform topPipe;
    [SerializeField] private Transform bottomPipe;

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

        transform.position = new Vector3(spawnX, 0f, transform.position.z);

        float halfGap = gapSize * 0.5f;

        float topHalfHeight = GetPipeHalfHeight(topPipe);
        float botHalfHeight = GetPipeHalfHeight(bottomPipe);

        float topY = gapCenterY + halfGap + topHalfHeight;
        float botY = gapCenterY - halfGap - botHalfHeight;

        topPipe.localPosition    = new Vector3(0f, topY, 0f);
        bottomPipe.localPosition = new Vector3(0f, botY, 0f);
    }

    private float GetPipeHalfHeight(Transform pipe)
    {
        var sr = pipe.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.y * 0.5f : 0.5f;
    }

    private void Update()
    {
        float speed = ObstacleSpawner.Instance.CurrentSpeed;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < _despawnX)
        {
            ObstacleSpawner.Instance?.ReturnObstacle(this);
        }
    }
}