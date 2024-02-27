using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadAddressableOnPress : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private AssetReferenceT<GameObject> _prefabReference;

    private void OnEnable()
    {
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> loadAsyncOperation = _prefabReference.LoadAssetAsync<GameObject>();

        loadAsyncOperation.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {

    }

    private void OnDisable()
    {
        _prefabReference.ReleaseAsset();
    }

    public void OnPress()
    {
        _prefabReference.InstantiateAsync();
    }
}
