using UnityEngine;

[RequireComponent(typeof(DoorOutline))]
public class SchoolDoorLock : MonoBehaviour
{
    private DoorOutline doorOutline;

    private void Awake()
    {
        doorOutline = GetComponent<DoorOutline>();
    }

    private void Update()
    {
        // If the school is unlocked, keep DoorOutline enabled
        if (TownGameManager.Instance.CanEnterSchool())
        {
            doorOutline.enabled = true;
        }
        else
        {
            doorOutline.enabled = false;
        }
    }
}