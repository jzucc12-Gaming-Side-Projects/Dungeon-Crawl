using System;
using UnityEngine;

public class Equipment : MonoBehaviour
{   
    private Player player = null;

    #region Attacking
    [Header("Attacking")]
    [SerializeField] private int swordLevel = 0;
    [SerializeField] private int damageDice = 1;
    [SerializeField] private int damageDie = 6;
    [SerializeField] private int damageBonus = 1;
    public event Action SwordUpgraded;
    #endregion

    #region //Blocking
    [Header("Blocking")]
    [SerializeField] private int shieldLevel = 0;
    [SerializeField] private float baseBlock = 0.20f;
    public event Action ShieldUpgraded;
    #endregion

    
    #region //Monobehaviour
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    #endregion

    #region //Attacking
    public void UpgradeSword() 
    { 
        swordLevel++; 
        SwordUpgraded?.Invoke();
    }
    public int GetSwordLevel() { return swordLevel; }

    public int CalculateDamage()
    {
        int damage = Helper.DamageRoller(swordLevel + damageDice, damageDie);
        damage += swordLevel + damageBonus;
        return damage;
    }

    public (int, int) GetDamageRange()
    {
        return Helper.GetDamageRange(swordLevel + damageDice, damageDie, swordLevel + damageBonus);
    }
    #endregion

    #region //Blocking
    public void UpgradeShield() 
    { 
        shieldLevel++; 
        ShieldUpgraded?.Invoke();
    }

    public int GetShieldLevel() { return shieldLevel; }

    public void Block()
    {
        player.SetBlockReduction(GetBlockReduction());
    }

    private float GetBlockReduction()
    {
        return baseBlock * (shieldLevel + 1);
    }

    public int GetBlockReductionAsInt()
    {
        float dec = GetBlockReduction();
        return (int)(dec * 100);
    }
    #endregion
}
