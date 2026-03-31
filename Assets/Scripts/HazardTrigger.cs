using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HazardTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameEvents.ObstacleHit();
    }
}
