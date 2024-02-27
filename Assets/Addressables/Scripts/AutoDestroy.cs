using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AutoDestroy : MonoBehaviour
{
    public float Delay;

    private void Start()
    {
        StartCoroutine(destroyDelay());
    }

    IEnumerator destroyDelay()
    {
        float time = 0f;

        while (time <= Delay)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Addressables.Release(gameObject);
        Destroy(gameObject, Delay);
    }
}
