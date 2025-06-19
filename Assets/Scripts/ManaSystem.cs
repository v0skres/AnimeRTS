using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    public static ManaSystem instance;

    [Header("UI")]
    public Text Text_Mana;
    public GameObject manaGainEffect;

    [Header("Settings")]
    public int defaultMana = 10;
    [SerializeField] private int _mana;

    public int Mana => _mana;


    //METHODS
    //Init (set the default values)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        _mana = defaultMana;
        UpdateUI();
    }
    //Gain currency (input of value)
    public void Gain(int val)
    {
        _mana += val;

        if (manaGainEffect != null)
        {
            Instantiate(manaGainEffect, transform.position, Quaternion.identity);
        }

        UpdateUI();
    }
    //Use currency (input of value)
    public bool Use(int val)
    {
        if (!EnoughCurrency(val)) return false;

        _mana -= val;
        UpdateUI();
        return true;
    }
    //Check availability of currency
    public bool EnoughCurrency(int val) => val <= _mana;

    //Update txt ui
    void UpdateUI()
    {
        if (Text_Mana != null)
            Text_Mana.text = _mana.ToString();
    }
}
