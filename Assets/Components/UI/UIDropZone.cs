using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropZone : MonoBehaviour, IDropHandler
{
    [Header("Setup")]
    [SerializeField] private RectTransform _targetParent;
    [SerializeField] public List<ObjectType> _validObjectTypes = new();

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped.TryGetComponent(out UIDraggable cell))
        {
            foreach (ObjectType objectType in cell.objectTypes)
            {
                if (_validObjectTypes.Contains(objectType))
                {
                    cell.TargetParent = _targetParent;
                }
            }
        }
    }
}
