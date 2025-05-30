using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour
{
    public List<GameObject> spellPrefabs;
    public List<Image> spellUI;
    public ManaSystem manaSystem;
    private int selectedSpellID = -1;

    void Update()
    {
        if (selectedSpellID != -1 && Input.GetMouseButtonDown(0))
        {
            TryCastSpell();
        }
    }

    void TryCastSpell()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        int spellCost = GetSpellCost(selectedSpellID);

        if (manaSystem.EnoughCurrency(spellCost))
        {
            if (manaSystem.Use(spellCost))
            {
                CastSpell(mousePos);
                DeselectSpells();
            }
        }
        else
        {
            Debug.Log("Not enough mana!");
            DeselectSpells();
        }
    }

    int GetSpellCost(int id)
    {
        ISpell spell = spellPrefabs[id].GetComponent<ISpell>();
        return spell != null ? spell.ManaCost : -1;
    }

    void CastSpell(Vector3 position)
    {
        GameObject spell = Instantiate(spellPrefabs[selectedSpellID]);
        spell.transform.position = position;
        spell.GetComponent<ISpell>().Activate();
    }

    public void SelectSpell(int id)
    {
        DeselectSpells();
        selectedSpellID = id;
        spellUI[selectedSpellID].color = Color.white;
    }

    public void DeselectSpells()
    {
        selectedSpellID = -1;
        foreach (var s in spellUI)
        {
            s.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}