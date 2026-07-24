using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private bool faceRight = true;
    private float moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if(moveDirection > 0 && !faceRight)
        {
            flipCharacter();
        }else if(moveDirection < 0 && faceRight)
        {
            flipCharacter();
        }

        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocityY);
    }

    void flipCharacter()
    {
        faceRight = !faceRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
