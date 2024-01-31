using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField, Min(0)] private float _smoothTime = 0.25f;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    private IEnumerator _smoothPositioningCorrutine;
    private Vector3 _currentVelocity = Vector3.zero;

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

        // Revisamos si el Drop Place tiene el check de Teleport
        // En caso de tenerlo, directamente haremos el teleport del objeto al centro del objeto
        // En caso contrario, haremos una Coroutine donde llevaremos el objeto hasta la zona
        if (teleportToPosition)
        {
            grabbale.transform.parent = gameObject.transform;
            grabbale.transform.localPosition = Vector3.zero;
            grabbale.OnStartGrab.AddListener(OnGrab);

            return;
        }

        _smoothPositioningCorrutine = SmoothPositioning(grabbale);
        StartCoroutine(_smoothPositioningCorrutine);
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

    private IEnumerator SmoothPositioning(Grabbable grabbale)
    {

        while (grabbale.transform.position != gameObject.transform.position)
        {
            grabbale.transform.position =
            Vector3.SmoothDamp(
                grabbale.transform.position,
                gameObject.transform.position,
                ref _currentVelocity,
                _smoothTime
            );

            yield return null;
        }

        grabbale.transform.parent = gameObject.transform;
        grabbale.OnStartGrab.AddListener(OnGrab);
        yield break;
    }
}
