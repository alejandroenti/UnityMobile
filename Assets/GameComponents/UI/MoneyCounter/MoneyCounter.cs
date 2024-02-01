using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private const string MoneyLabelTranslationKey = "money_label";

    private void Start()
    {
        SystemManager sM = SystemManager.Instance;
        UpdateTextWithCurrentMoney(sM.Money);

        // Manejo de subscripci�n a un evento
        // SIEMPRE PONER LA DESCUBSCRIPCI�N
        sM.OnMoneyChange += UpdateTextWithCurrentMoney;
    }

    private void UpdateTextWithCurrentMoney(int money)
    {
        // Manera correcta de trabajar
        //string text = TranslationManager.GetString(MoneyLabelTranslationKey, money.ToString());

        string text = "Money: " + money.ToString();
        _text.text = text;
    }

    private void OnDestroy()
    {
        SystemManager sM = SystemManager.Instance;
        sM.OnMoneyChange -= UpdateTextWithCurrentMoney;
    }
}
