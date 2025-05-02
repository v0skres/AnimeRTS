using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencySystem : MonoBehaviour
{
    //FIELDS
    public Text Text_Currency;
    public int defaultCurrency;
    public int currency;

    [Header("Passive Income Settings")]
    public bool enablePassiveIncome = true; // Включить/выключить пассивный доход
    public float passiveIncomeInterval = 1f; // Интервал в секундах
    public int passiveIncomeAmount = 1; // Сколько денег добавлять за раз

    //METHODS
    void Start()
    {
        Init();
        StartPassiveIncome();
    }

    public void Init()
    {
        currency = defaultCurrency;
        UpdateUI();
    }

    // Запуск пассивного дохода
    private void StartPassiveIncome()
    {
        if (enablePassiveIncome)
        {
            StartCoroutine(PassiveIncomeRoutine());
            // Альтернатива: InvokeRepeating("GainPassiveIncome", passiveIncomeInterval, passiveIncomeInterval);
        }
    }

    // Корутина для пассивного дохода
    private IEnumerator PassiveIncomeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(passiveIncomeInterval);
            Gain(passiveIncomeAmount);
        }
    }

    // Альтернативный метод (если не хочешь использовать корутины)
    // private void GainPassiveIncome()
    // {
    //     Gain(passiveIncomeAmount);
    // }

    public void Gain(int val)
    {
        currency += val;
        UpdateUI();
    }

    public bool Use(int val)
    {
        if (EnoughCurrency(val))
        {
            currency -= val;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnoughCurrency(int val)
    {
        return val <= currency;
    }

    void UpdateUI()
    {
        Text_Currency.text = currency.ToString();
    }

    public void USE_TEST()
    {
        Debug.Log(Use(3));
    }
}