using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public InputActionReference takeDamageReference;

    float horizontalMovement;
    float verticalMovement;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, verticalMovement * moveSpeed);
    }

    void OnEnable()
    {
        if (takeDamageReference != null && takeDamageReference.action != null)
        {
            takeDamageReference.action.performed += OnTakeDamageInput;
            if (!takeDamageReference.action.enabled)
                takeDamageReference.action.Enable();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }

    private void OnTakeDamageInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}
