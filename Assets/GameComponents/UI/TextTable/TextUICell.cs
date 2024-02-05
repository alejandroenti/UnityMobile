using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextUICell : UICell, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    public TextMeshProUGUI Label;

    public Transform TargetParent;
    private RectTransform _rect;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[*] INFO: Start Drag");

        Label.raycastTarget = false;
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
        Label.raycastTarget = true;
    }
}
