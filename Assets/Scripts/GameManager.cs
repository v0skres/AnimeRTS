using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake() { instance = this; }

    public Spawner spawner;
    public HealthSystem health;
    public CurrencySystem currency;
    public ManaSystem mana;

    void Start()
    {
        GetComponent<HealthSystem>().Init();
        GetComponent<CurrencySystem>().Init();
        GetComponent<ManaSystem>().Init();

        StartCoroutine(WaveStartDelay());
    }

    IEnumerator WaveStartDelay()
    {
        //Wait for X seconds
        yield return new WaitForSeconds(2f);
        //Start the enemy spawning
        //GetComponent<EnemySpawner>().StartSpawning();
    }

}