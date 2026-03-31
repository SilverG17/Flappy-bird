using System;

public static class GameEvents
{
    public static event Action OnObstacleHit;
    public static event Action OnCollectibleCollected;

    public static void ObstacleHit()         => OnObstacleHit?.Invoke();
    public static void CollectibleCollected() => OnCollectibleCollected?.Invoke();

    public static void ClearAll()
    {
        OnObstacleHit         = null;
        OnCollectibleCollected = null;
    }
}
