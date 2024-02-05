using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    [SerializeField] public List<ObjectType> objectTypes = new();
    [SerializeField] private RectTransform _rect;
    [SerializeField] private CanvasGroup _canvasGroup;
    public Transform TargetParent;

    [Header("Events")]
    // Hacer esto Draggable y DropZone
    public UnityEvent<UIDropZone, UIDraggable> OnDragStarted;
    public UnityEvent<UIDropZone, UIDraggable> OnDropSucceed;
    public UnityEvent<UIDropZone, UIDraggable> OnDropFailure;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        TargetParent = _rect.parent;
        _rect.SetParent(GetComponentInParent<DragContainer>().Rect);

        //OnDragStarted.Invoke(TargetParent, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        _rect.SetParent(TargetParent);
        _canvasGroup.blocksRaycasts = true;
    }
}
