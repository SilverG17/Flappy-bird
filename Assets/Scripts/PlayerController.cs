using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Physics Settings")]
    public float Flap_strength = 5f;
    private Rigidbody2D rb;

    [Header("Rotation")]
    public float upRotation = 45f;
    public float downRotation = -45f;
    public float rotationSpeed = 5f;
    [Header("Animation")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] flapFrames;
    public Sprite[] idleFrames;
    public float animFPS = 12f;
    private float timer;
    private int currentFrame;
    private bool isFlapping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool isSpacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool isMouseClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        
        if (isSpacePressed || isMouseClick)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * Flap_strength, ForceMode2D.Impulse);
            isFlapping = true;
        }
        if (rb.linearVelocity.y <=0)
        {
            isFlapping = false;
        }
        
        Sprite[] currentAnim;
        
        if (isFlapping)
        {
            currentAnim = flapFrames;
        }
        else
        {
            currentAnim = idleFrames;
        }
        if (currentAnim == null || currentAnim.Length == 0) return;
        timer += Time.deltaTime;
        float frameDuration = 1f / animFPS;
        if(timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame = (currentFrame + 1) % currentAnim.Length;
            spriteRenderer.sprite = currentAnim[currentFrame];
        }
        float targetAngle;
        float currentRotSpeed;

        if (rb.linearVelocity.y > 0) 
        {
            
            targetAngle = upRotation;
            currentRotSpeed = rotationSpeed * 4f; 
        }
        else
        {
            targetAngle = downRotation;
            currentRotSpeed = rotationSpeed; 
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotSpeed * Time.deltaTime);
    }
}
