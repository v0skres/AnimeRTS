using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SpellManager : MonoBehaviour
{
    public List<GameObject> spellPrefabs;
    public List<Image> spellUI;
    public List<Image> cooldownOverlays; // Затемнение для перезарядки
    public ManaSystem manaSystem;

    private int selectedSpellID = -1;
    private List<bool> spellsOnCooldown;
    private List<float> cooldownTimes;
    private List<float> currentCooldowns;
    public List<Text> cooldownTexts; // Текстовые элементы для отображения времени

    private void Start()
    {
        // Инициализация системы перезарядки
        spellsOnCooldown = new List<bool>(new bool[spellPrefabs.Count]);
        cooldownTimes = new List<float>();
        currentCooldowns = new List<float>();

        // Установка времени перезарядки для каждого спелла
        foreach (var spellPrefab in spellPrefabs)
        {
            ISpell spell = spellPrefab.GetComponent<ISpell>();
            cooldownTimes.Add(spell.CooldownTime);
            currentCooldowns.Add(0f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentCooldowns.Count; i++)
        {
            if (currentCooldowns[i] > 0)
            {
                currentCooldowns[i] -= Time.deltaTime;
                UpdateCooldownUI(i);

                if (currentCooldowns[i] <= 0)
                {
                    spellsOnCooldown[i] = false;
                    cooldownOverlays[i].fillAmount = 0;
                    cooldownTexts[i].gameObject.SetActive(false);
                }
                else
                {
                    // Обновляем текст с временем перезарядки
                    cooldownTexts[i].text = Mathf.Ceil(currentCooldowns[i]).ToString();
                }
            }
        }

        if (selectedSpellID != -1 && Input.GetMouseButtonDown(0))
        {
            TryCastSpell();
        }
    }

    void TryCastSpell()
    {
        if (spellsOnCooldown[selectedSpellID])
        {
            Debug.Log("Spell is on cooldown!");
            DeselectSpells();
            return;
        }

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

    void StartCooldown(int spellID)
    {
        spellsOnCooldown[spellID] = true;
        currentCooldowns[spellID] = cooldownTimes[spellID];
        cooldownTexts[spellID].gameObject.SetActive(true);
        cooldownTexts[spellID].text = Mathf.Ceil(cooldownTimes[spellID]).ToString();
    }

    void UpdateCooldownUI(int spellID)
    {
        if (cooldownOverlays.Count > spellID)
        {
            cooldownOverlays[spellID].fillAmount =
                currentCooldowns[spellID] / cooldownTimes[spellID];
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
        if (spellsOnCooldown[id])
        {
            Debug.Log("This spell is on cooldown!");
            return;
        }

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