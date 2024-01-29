using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Grabbable;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode
    {
        CheckObject = 1,        //000001
        CheckObjectType = 2     //000010
    }

    [Header("Setup")]
    [SerializeField] private CheckMode _checkMode;
    [SerializeField] private List<Grabbable> _validGrabbables = new();
    [SerializeField] private List<ObjectType> _validObjectTypes = new();
    [SerializeField] public bool teleportToPosition = true;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public bool IsValid(Grabbable grabbable)
    {

        if (_checkMode.HasFlag(CheckMode.CheckObject))
        {
            if (_validGrabbables.Contains(grabbable))
            {
                return true;
            }
        }

        if (_checkMode.HasFlag(CheckMode.CheckObjectType))
        {
            foreach(ObjectType objectType in grabbable.objectTypes)
            {
                if (_validObjectTypes.Contains(objectType))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void OnDrop(Grabbable grabbale)
    {
        OnObjectDropped.Invoke(grabbale.gameObject);

        if (teleportToPosition)
        {
            grabbale.transform.parent = gameObject.transform;
            grabbale.transform.localPosition = Vector3.zero;
        }
        
        

        grabbale.OnStartGrab.AddListener(OnGrab);
    }

    public void OnGrab(GameObject grabbaleObject, GameObject parent)
    {
        if (grabbaleObject.TryGetComponent(out Grabbable grabbable))
        {
            grabbable.OnStartGrab.RemoveListener(OnGrab);
        }
        OnObjectGrabbed.Invoke(grabbaleObject);
        grabbaleObject.transform.parent = null;
    }
}
