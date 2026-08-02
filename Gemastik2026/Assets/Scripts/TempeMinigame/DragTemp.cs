using UnityEngine;

public class DragTemp : MonoBehaviour
{
    public TempeSource source;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void OnMouseDown()
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    void OnMouseDrag()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        transform.position = mouse;
    }

    void OnMouseUp()
    {
        rb.gravityScale = 1f;
    }

    void OnBecameInvisible()
    {
        TempeGameManager.Instance.LosePoint();

        source.SquareFinished();

        Destroy(gameObject);
    }
}