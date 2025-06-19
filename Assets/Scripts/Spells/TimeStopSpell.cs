using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSpell : MonoBehaviour, ISpell
{
    [Header("Settings")]
    [SerializeField] private int _manaCost = 15;
    [SerializeField] private float _cooldownTime = 30f; // Время перезарядки в секундах
    [SerializeField] private float _duration = 3f;
    public int ManaCost => _manaCost;
    public float CooldownTime => _cooldownTime;

    [Header("Effects")]
    public ParticleSystem timeStopEffect;
    public AudioClip timeStopSound;

    private float _originalTimeScale = 1f;
    private bool _wasPaused = false;

    private List<Enemy> _frozenEnemies = new List<Enemy>();
    private List<ITower> _frozenTowers = new List<ITower>();
    private List<ShootItem> _frozenProjectiles = new List<ShootItem>();
    private List<LaserBeam> _frozenLasers = new List<LaserBeam>();

    public void Activate()
    {
        StartCoroutine(TimeStopRoutine());
    }

    private IEnumerator TimeStopRoutine()
    {
        // Сохраняем оригинальное состояние паузы
        _wasPaused = PauseMenu.PauseGame;
        _originalTimeScale = Time.timeScale;

        // Если игра была на паузе, временно снимаем паузу
        if (_wasPaused)
        {
            Time.timeScale = 1f;
            PauseMenu.PauseGame = false;
        }

        // 1. Замораживаем объекты
        FreezeObjects();

        // 2. Запускаем эффекты
        if (timeStopEffect != null)
            Instantiate(timeStopEffect, transform.position, Quaternion.identity);

        if (timeStopSound != null)
            AudioSource.PlayClipAtPoint(timeStopSound, transform.position);

        // 3. Ждем указанное время (используем unscaled time, так как timeScale изменен)
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < _duration)
        {
            yield return null;
        }

        // 4. Размораживаем объекты
        UnfreezeObjects();

        // 5. Восстанавливаем оригинальное состояние паузы
        if (_wasPaused)
        {
            Time.timeScale = 0f;
            PauseMenu.PauseGame = true;
        }
        else
        {
            Time.timeScale = _originalTimeScale;
        }

        // 6. Уничтожаем спелл
        Destroy(gameObject);
    }

    private void FreezeObjects()
    {
        // Очищаем списки от предыдущих значений
        _frozenEnemies.Clear();
        _frozenTowers.Clear();
        _frozenProjectiles.Clear();
        _frozenLasers.Clear();

        // Замораживаем врагов
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if (enemy != null)
            {
                enemy.SetPaused(true);
                _frozenEnemies.Add(enemy);
            }
        }

        // Замораживаем башни
        foreach (MonoBehaviour mb in FindObjectsOfType<MonoBehaviour>())
        {
            if (mb is ITower tower)
            {
                mb.enabled = false;
                _frozenTowers.Add(tower);
            }
        }

        // Замораживаем снаряды
        foreach (ShootItem projectile in FindObjectsOfType<ShootItem>())
        {
            if (projectile != null)
            {
                projectile.SetPaused(true);
                _frozenProjectiles.Add(projectile);
            }
        }

        // Замораживаем лазеры
        foreach (LaserBeam laser in FindObjectsOfType<LaserBeam>())
        {
            if (laser != null)
            {
                laser.SetPaused(true);
                _frozenLasers.Add(laser);
            }
        }

        // Глобальные эффекты
        Time.timeScale = 0.05f;
    }

    private void UnfreezeObjects()
    {
        // Размораживаем врагов
        foreach (Enemy enemy in _frozenEnemies)
        {
            if (enemy != null)
            {
                enemy.SetPaused(false);
            }
        }

        // Размораживаем башни
        foreach (ITower tower in _frozenTowers)
        {
            if (tower is MonoBehaviour towerMono)
            {
                towerMono.enabled = true;
            }
        }

        // Размораживаем снаряды
        foreach (ShootItem projectile in _frozenProjectiles)
        {
            if (projectile != null)
            {
                projectile.SetPaused(false);
            }
        }

        // Размораживаем лазеры
        foreach (LaserBeam laser in _frozenLasers)
        {
            if (laser != null)
            {
                laser.SetPaused(false);
            }
        }

        // Возвращаем нормальное время
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        // На случай если объект уничтожился раньше времени
        if (Time.timeScale < 1f)
        {
            UnfreezeObjects();
        }
    }
}