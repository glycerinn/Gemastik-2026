using UnityEngine;

public class DoorOutline : MonoBehaviour
{
    public GameObject door;
    public GameObject doorLight;
    public Material normalMaterial;
    public Material outlineMaterial;
    private SpriteRenderer sr;
    private bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.material = normalMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            sr.material = outlineMaterial; 
            doorLight.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            sr.material = normalMaterial;
            doorLight.SetActive(false);
            door.SetActive(true);
        }
    }
}
