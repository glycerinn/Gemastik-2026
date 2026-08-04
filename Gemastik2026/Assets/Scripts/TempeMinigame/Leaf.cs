using System.Collections;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    private bool occupied;

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (occupied)
            return;

        if (!other.CompareTag("Tempe"))
            return;

        occupied = true;

        DragTemp temp = other.GetComponent<DragTemp>();

        temp.wasPlacedSuccessfully = true;
        TempeGameManager.Instance.AddPoint();
        temp.source.SquareFinished();

        Destroy(other.gameObject);

        StartCoroutine(RespawnSlot());
    }

    IEnumerator RespawnSlot()
    {
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(1f);

        sr.enabled = true;
        col.enabled = true;

        occupied = false;
    }
}