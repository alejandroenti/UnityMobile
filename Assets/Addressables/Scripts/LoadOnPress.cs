using UnityEngine;

public class LoadOnPress : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject _prefab;

    public void OnPress()
    {
        Instantiate(_prefab);
    }
}
