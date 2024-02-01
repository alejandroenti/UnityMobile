using UnityEngine;

public class SystemManager
{
    private static SystemManager instance;
    public static SystemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SystemManager();
            }

            return instance;
        }
    }

    public delegate void MoneyChange(int money);
    public event MoneyChange OnMoneyChange; 
    
    private const string MoneyKey = "Money";
    private int _money = 0;

    public int Money
    {
        get => _money;
    }
    
    public bool ModifyMoney(int value)
    {
        if (Money + value < 0)
        {
            return false;
        }

        _money += value;
        OnMoneyChange?.Invoke(_money);

        return true;
    }

    private SystemManager()
    {
        LoadData();
    }

    private void LoadData()
    {
        LoadMoney();
        // Other Loads
    }

    private void LoadMoney()
    {
        // Load from File or other DB
        _money = PlayerPrefs.GetInt(MoneyKey, 0);
    }
}
