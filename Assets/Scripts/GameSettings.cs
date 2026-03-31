using UnityEngine;

public static class GameSettings
{
    public const float BASE_SCROLL_SPEED    = 5f;
    public const float MAX_SCROLL_SPEED     = 12f;

    public const float OBSTACLE_INTERVAL    = 2.0f;
    public const float GAP_SIZE_MIN         = 2.8f;
    public const float GAP_SIZE_MAX         = 4.5f;
    public const float GAP_MIN_Y            = -1.5f;
    public const float GAP_MAX_Y            =  1.5f;
    public const float SPAWN_X_OFFSET       = 12f;
    public const float PIPE_WIDTH           = 1.2f;

    public const float PLAYER_ANIM_FPS      = 12f;
    public const float COIN_ANIM_FPS        =  8f;

    public const float SPEED_INCREMENT      = 0.4f;
    public const float SCALE_INTERVAL       = 10.0f;

    public const int OBSTACLE_POOL_SIZE     = 8;
    public const int COLLECTIBLE_POOL_SIZE  = 8;
}
