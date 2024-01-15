using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode
    {
        CheckObject = 1,        //000001
        CheckGrabbableType = 2  //000010
    }

    [Header("Setup")]
    [SerializeField] private CheckMode _checkMode;
    [SerializeField] private List<Grabbable> _validGrabbables = new();
    [SerializeField] private Grabbable.GrabbableType _validGrabbableTypes;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public bool IsValid(Grabbable grabbable)
    {
        bool isValid = true;

        if (_checkMode.HasFlag(CheckMode.CheckObject))
        {
            if (_validGrabbables.Contains(grabbable))
            {
                return true;
            }
        }

        if (_checkMode.HasFlag(CheckMode.CheckGrabbableType))
        {
            if (!_validGrabbableTypes.HasFlag(grabbable.grabbableType))
            {
                isValid = false;
            }
        }

        return isValid;
    }

    public void OnDrop(Grabbable grabbale)
    {
        OnObjectDropped.Invoke(grabbale.gameObject);
        grabbale.OnStartGrab.AddListener(OnGrab);
    }

    public void OnGrab(GameObject grabbaleObject, GameObject parent)
    {
        if (grabbaleObject.TryGetComponent(out Grabbable grabbable))
        {
            grabbable.OnStartGrab.RemoveListener(OnGrab);
        }
        OnObjectGrabbed.Invoke(grabbaleObject);
    }
}
