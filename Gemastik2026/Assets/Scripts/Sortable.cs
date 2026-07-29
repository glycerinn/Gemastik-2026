using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Sortable : MonoBehaviour
{
    public enum SortableType { Fish, Trash, Coin };

    public SortableType type;

    public void SetType(SortableType newType)
    {
        type = newType;
    }

    public SortableType GetSortableType()
    {
        return type;
    }
}
