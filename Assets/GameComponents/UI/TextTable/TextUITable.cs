using System.Collections.Generic;
using UnityEngine;

public class TextUITable : UITable<TextUICell>
{
    [Header("Setup")]
    [SerializeField] private List<string> _allTexts = new();

    public override int TotalCellsCount => _allTexts.Count;

    public override void SetupCell(TextUICell cell)
    {
        cell.Label.text = cell.Index.ToString() + " " + _allTexts[cell.Index];
    }
}
