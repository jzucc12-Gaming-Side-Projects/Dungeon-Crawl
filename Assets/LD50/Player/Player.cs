using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region //HP
    [Header("HP and Damage")]
    [SerializeField] private int maxHP = 50;
    [SerializeField, ReadOnly] int currentHP = 0;
    [SerializeField, ReadOnly] private float blockReduction = 0f;
    [SerializeField] private Animator deathAnimator = null;
    [SerializeField] private AudioClip deathSFX = null;
    public event Action OnHPChange;
    public event Action OnDeath;
    #endregion

    #region //MP
    [Header("MP")]
    [SerializeField] private int maxMP = 50;
    [SerializeField, ReadOnly] int currentMP = 0;
    public event Action OnMPChange;
    #endregion

    #region //Status effects
    [Header("Status effects")]
    [SerializeField] private bool isPoisoned = false;
    [SerializeField] private int poisonDamage = 5;
    public event Action<bool> changeStatus;
    private AudioSource poisonSFXPlayer = null;
    #endregion

    
    #region //Monobehaviour
    private void Awake()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        poisonSFXPlayer = GetComponent<AudioSource>();
    }
    #endregion

    #region //Getters
    public (int, int) GetHP() { return (currentHP, maxHP); }
    public (int, int) GetMP() { return (currentMP, maxMP); }
    #endregion

    #region //Damage and Death
    public bool MaxHP() { return currentHP == maxHP; }
    public void SetBlockReduction(float reduction)
    {
        blockReduction = reduction;
    }

    public bool TakeDamage(int damage)
    {
        damage = Mathf.Abs(damage);
        int newDamage = Mathf.RoundToInt(damage * (1 - blockReduction));
        currentHP = Mathf.Max(currentHP - newDamage, 0);
        OnHPChange?.Invoke();

        if(currentHP == 0)
        {
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.2f);
            PlayerDied("Imp'a-Tient revived you!");
            return true;
        }

        return false;
    }

    public bool TakeDamage(string attacker, string attackName, int damage)
    {
        damage = Mathf.Abs(damage);
        int newDamage = Mathf.RoundToInt(damage * (1 - blockReduction));
        currentHP = Mathf.Max(currentHP - newDamage, 0);
        OnHPChange?.Invoke();

        if(currentHP == 0)
        {
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.4f);
            PlayerDied("Imp'a-Tient revived you and destroyed all in your path!");
            return true;
        }
        else
        {
            GameText.SetText($"{attacker} used {attackName} to deal {newDamage} damage to you!");
            return false;
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHPChange?.Invoke();
    }

    private void PlayerDied(string deathText)
    {
        deathAnimator.Play("Explosion", -1 , 0f);
        GameText.SetText(deathText);
        currentHP = maxHP/2;
        currentMP = maxMP/2;
        Poison(false);
        OnHPChange?.Invoke();
        OnMPChange?.Invoke();
        OnDeath?.Invoke();
    }
    #endregion

    #region //Magic Usage
    public bool MaxMP() { return currentMP == maxMP; }
    public bool OutOfMP() { return currentMP == 0; }
    public bool EnoughMP(int cost)
    {
        return currentMP >= cost;
    }

    public void UseMP(int amount)
    {
        amount = Mathf.Abs(amount);
        currentMP = Mathf.Max(0, currentMP - amount);
        OnMPChange?.Invoke();
    }

    public void RestoreMP(int amount)
    {
        currentMP = Mathf.Min(maxMP, currentMP + amount);
        OnMPChange?.Invoke();
    }
    #endregion

    #region //Status effects
    public void Poison(bool poison)
    {
        isPoisoned = poison;
        changeStatus?.Invoke(poison);
    }

    public void TakePoisonDamage()
    {
        if(!isPoisoned) return;
        bool died = TakeDamage(poisonDamage);

        if(died)
        {
            GameText.SetText("Poison killed you, but Imp'a-Tient revived you and destroyed all in your path!");
        }
        else
        {
            poisonSFXPlayer.Play();
            GameText.SetText($"You took {poisonDamage} damage from poison!");
        }
    }
    #endregion
}