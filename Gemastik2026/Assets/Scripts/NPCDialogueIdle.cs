using UnityEngine;

public class NPCDialogueIdle : MonoBehaviour
{
    public Transform player;
    public GameObject InteractOption;

    public float interactDistance = 6f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance)
        {
            InteractOption.SetActive(true);
        }
        else
        {
            InteractOption.SetActive(false);
        }
    }
}
