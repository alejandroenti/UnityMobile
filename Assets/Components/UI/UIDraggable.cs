using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    [SerializeField] public List<ObjectType> objectTypes = new();
    [SerializeField] private RectTransform _rect;
    [SerializeField] private CanvasGroup _canvasGroup;
    public Transform TargetParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[*] INFO: Start Drag");

        _canvasGroup.blocksRaycasts = false;
        TargetParent = _rect.parent;
        _rect.SetParent(GetComponentInParent<DragContainer>().Rect);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("[*] INFO: On Drag");

        if (eventData.pointerEnter == null || eventData.pointerEnter.transform as RectTransform == null)
        {
            return;
        }

        RectTransform plane = eventData.pointerEnter.transform as RectTransform;

        if (RectTransformUtility
            .ScreenPointToWorldPointInRectangle(
                plane,
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 globalMousePos
                ))
        {
            _rect.position = globalMousePos;
            _rect.rotation = plane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("[*] INFO: End Drag");

        _rect.SetParent(TargetParent);
        _canvasGroup.blocksRaycasts = true;
    }
}
