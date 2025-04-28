using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    //FIELDS
    //currency txt UI
    public Text Text_Mana;
    //default currency value
    public int defaultMana;
    //current currency value
    public int mana;


    //METHODS
    //Init (set the default values)
    public void Init()
    {
        mana = defaultMana;
        UpdateUI();
    }
    //Gain currency (input of value)
    public void Gain(int val)
    {
        mana += val;
        UpdateUI();
    }
    //Use currency (input of value)
    public bool Use(int val)
    {
        if (EnoughCurrency(val))
        {
            mana -= val;
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }
    //Check availability of currency
    public bool EnoughCurrency(int val)
    {
        //Check if the val is equal or more than currency
        if (val <= mana)
            return true;
        else
            return false;
    }
    //Update txt ui
    void UpdateUI()
    {
        Text_Mana.text = mana.ToString();
    }

    public void USE_TEST()
    {
        Debug.Log(Use(3));
    }
}
