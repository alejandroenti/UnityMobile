using System;
using UnityEngine;

public class DragContainer : MonoBehaviour
{
    [Header("Setup")]
    [NonSerialized] public RectTransform Rect;

    private void Start()
    {
        Rect = GetComponent<RectTransform>();
    }
}
