using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Material _fantasmalMaterial;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;

    private IEnumerator _smoothPositioningCorrutine;
    private Vector3 _currentVelocity = Vector3.zero;

    private GameObject _fantasmalObject;

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
        DestroyFantasmalObject();

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
        
        if (!teleportToPosition)
        {
            StopCoroutine(_smoothPositioningCorrutine);
        }

        OnObjectGrabbed.Invoke(grabbaleObject);
        grabbaleObject.transform.parent = null;

        CreateFantasmalObject(grabbable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Grabbable grabbable) && IsValid(grabbable))
        {
            CreateFantasmalObject(grabbable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Grabbable grabbable) && IsValid(grabbable))
        {
            DestroyFantasmalObject();
        }
    }

    private IEnumerator SmoothPositioning(Grabbable grabbale)
    {
        grabbale.transform.parent = gameObject.transform;
        grabbale.OnStartGrab.AddListener(OnGrab);

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
    }

    private void CreateFantasmalObject(Grabbable grabbable)
    {
        _fantasmalObject = new GameObject("Fantasmal Object", typeof(MeshFilter), typeof(MeshRenderer));
        Mesh fantasmalMesh = grabbable.GetComponent<MeshFilter>().mesh;

        _fantasmalObject.transform.parent = gameObject.transform;
        _fantasmalObject.transform.localPosition = Vector3.zero;
        _fantasmalObject.transform.localScale = grabbable.transform.lossyScale;

        _fantasmalObject.GetComponent<MeshFilter>().mesh = fantasmalMesh;
        _fantasmalObject.GetComponent<MeshRenderer>().material = _fantasmalMaterial;
    }

    private void DestroyFantasmalObject()
    {
        Destroy(_fantasmalObject);
        _fantasmalObject = null;
    }
}
