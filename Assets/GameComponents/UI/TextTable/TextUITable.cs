using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextUITable : UITable<TextUICell>, IDropHandler
{
    [Header("Setup")]
    [SerializeField] private List<string> _allTexts = new();

    public override int TotalCellsCount => _allTexts.Count;

    public override void SetupCell(TextUICell cell)
    {
        cell.Label.text = cell.Index.ToString() + " " + _allTexts[cell.Index];
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("[*] INFO: Object Dropped Over " + name);
        GameObject dropped = eventData.pointerDrag;

        if (dropped.TryGetComponent(out TextUICell cell))
        {
            cell.TargetParent = _scrollRect.content;
        }
    }
}
