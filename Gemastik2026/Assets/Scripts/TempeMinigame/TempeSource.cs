using UnityEngine;

public class TempeSource : MonoBehaviour
{
    public GameObject squarePrefab;
    public Transform spawnPoint;

    private GameObject currentSquare;

    private void OnMouseDown()
    {
        Debug.Log("Source clicked");
        SpawnTempe();
    }

    public void SpawnTempe()
    {
        if (currentSquare != null)
            return;

        currentSquare = Instantiate(squarePrefab, spawnPoint.position, Quaternion.identity);

        DragTemp drag = currentSquare.GetComponent<DragTemp>();
        drag.source = this;
    }

    public void SquareFinished()
    {
        currentSquare = null;
    }
}