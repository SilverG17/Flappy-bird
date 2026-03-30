using UnityEngine;

public class ParallaxRepeatingLayer : MonoBehaviour
{
    [SerializeField] private float startScrollSpeed = 1f;
    [SerializeField] private float maxScrollSpeed = 3f;
    [SerializeField] private float acceleration = 0.15f;
    [SerializeField] private float parallaxMultiplier = 0.5f;
    [SerializeField] private bool autoDetectTileWidth = true;
    [SerializeField] private float tileWidth = 0f;
    
    private float _currentScrollSpeed;

    private void Awake()
    {
        _currentScrollSpeed = startScrollSpeed;

        if (!autoDetectTileWidth || tileWidth > 0f)
        {
            return;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            tileWidth = spriteRenderer.bounds.size.x;
        }
    }

    private void Update()
    {
        _currentScrollSpeed = Mathf.MoveTowards(
            _currentScrollSpeed,
            maxScrollSpeed,
            acceleration * Time.deltaTime);

        float moveAmount = _currentScrollSpeed * parallaxMultiplier * Time.deltaTime;
        transform.Translate(Vector3.left * moveAmount, Space.World);

        if (tileWidth <= 0f || transform.position.x > -tileWidth)
        {
            return;
        }

        transform.position += Vector3.right * (tileWidth * 2f);
    }
}
