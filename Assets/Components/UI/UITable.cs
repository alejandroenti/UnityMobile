using UnityEngine;
using UnityEngine.UI;

public abstract class UITable<T> : MonoBehaviour where T : UICell
{
    [Header("Setup UITable")]
    [SerializeField] protected ScrollRect _scrollRect;
    [SerializeField] private T _baseCell;

    protected virtual void Start()
    {
        if (_scrollRect == null)
        {
            Debug.LogWarning("[!] WARNING: Have not scroll rect in UITable named: " + name);
            return;
        }

        ReloadTable();
    }

    public void ReloadTable()
    {
        RectTransform parent = _scrollRect.content;

        // Limpiamos del parent todo el contenido existente
        int childCount = parent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject gO = parent.GetChild(i).gameObject;

            if (gO != _baseCell.gameObject)
            {
                Destroy(gO);
            }
        }

        // Rellenamos el parent con el nuevo contenido
        int cellsCount = TotalCellsCount;
        _baseCell.gameObject.SetActive(true);

        for (int i = 0; i < cellsCount; i++)
        {
            T cell = Instantiate(_baseCell, parent);
            cell.Index = i;
            SetupCell(cell);
        }
        _baseCell.gameObject.SetActive(false);
    }

    public abstract int TotalCellsCount {  get; }
    public abstract void SetupCell(T cell);
}