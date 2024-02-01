using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField, Min(0)] private int _coinValue = 1;

    public void GetCoin()
    {
        SystemManager sM = SystemManager.Instance;
        sM.ModifyMoney(_coinValue);

        Destroy(this.gameObject);
    }
}
