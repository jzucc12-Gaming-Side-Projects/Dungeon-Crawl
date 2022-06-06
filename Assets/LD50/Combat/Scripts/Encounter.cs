using System;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();
    public event Action<float, string> EncounterOver;

    
    #region //Monobehaviour
    public void Awake()
    {
        foreach(var enemy in GetComponentsInChildren<Enemy>())
            enemies.Add(enemy);
    }

    private void OnEnable()
    {
        foreach(var enemy in enemies)
            enemy.OnDeath += RemoveEnemy;
    }

    private void OnDisable()
    {
        foreach(var enemy in enemies)
            enemy.OnDeath -= RemoveEnemy;
    }
    #endregion

    #region //Enemy modification
    private void RemoveEnemy(Enemy enemy)
    {
        enemy.OnDeath -= RemoveEnemy;
        enemies.Remove(enemy);
        
        if(enemies.Count > 0) return;
        EncounterOver?.Invoke(2f, "All enemies have been defeated!");
    }
    #endregion

    #region //Getters
    public IEnumerable<Enemy> GetEnemies() { return enemies; }
    #endregion
}