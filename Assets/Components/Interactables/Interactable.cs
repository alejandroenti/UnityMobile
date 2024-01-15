using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public bool breakWithDistance = true;

    [Header("Events")]
    public UnityEvent<GameObject> onStartInteract;
    public UnityEvent<GameObject> onEndInteract;

    public virtual void StartInteract(GameObject interactor)
    {
        onStartInteract.Invoke(interactor);
    }

    public virtual void EndInteract(GameObject interactor)
    {
        onEndInteract.Invoke(interactor);
    }
}
