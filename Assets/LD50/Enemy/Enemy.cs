using System;
using System.Collections;
using JZ.DISPLAY;
using JZ.UI;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region //Enemy Components
    [Header("Components")]
    [SerializeField] private Button targetButton = null;
    [SerializeField] private float hitFlashDuration = 1f;
    private Image image = null;
    private TimeTracker tracker = null;
    private Player player = null;
    private AudioSource source = null;
    private Blinker blinker = null;
    #endregion

    #region //Enemy info
    [Header("Enemy Info")]
    [SerializeField] private string enemyName = "";
    [SerializeField] private int maxHP = 50;
    [SerializeField, ReadOnly] private int currentHP = 50;
    #endregion

    #region //Enemy Actions
    [Header("Actions")]
    [SerializeField] private EnemyAttack[] attacks = new EnemyAttack[0];
    public event Action<Enemy> OnDeath;
    public event Action OnHealthChange;
    public delegate void ButtonBehavior();
    #endregion
    
    #region //Monobehaviour
    private void OnValidate()
    {
        currentHP = maxHP;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        tracker = FindObjectOfType<TimeTracker>();
        image = GetComponent<Image>();
        player = FindObjectOfType<Player>();
        blinker = GetComponent<Blinker>();
    }
    #endregion

    #region //Attacking
    public void Attack()
    {
        int roll = UnityEngine.Random.Range(0, GetMaxWeight()+1);
        int total = 0;
        bool died = false;

        foreach(var attack in attacks)
        {
            total += attack.usageWeight;
            if(total < roll) continue;

            if(attack.healingToSelf > 0) Heal(attack.healingToSelf);
            if(attack.damageDiceToPlayer > 0 & attack.damageToPlayerBonus > 0) died = HurtPlayer(attack);
            if(attack.poisons) 
            {
                player.Poison(true);
                GameText.AppendText(" You were poisoned!");
            }

            if(!died && !tracker.UpdateTime(attack.duration))
                source.PlayOneShot(attack.clip);

            if(attack.duration < 0)
                GameText.SetText($"{enemyName} rewound time by {attack.duration} seconds!");
            break;
        }
    }

    private bool HurtPlayer(EnemyAttack attack)
    {
        ScreenShake.CallShake(1,50);
        int damage = Helper.DamageRoller(attack.damageDiceToPlayer, attack.damageDieToPlayer) + attack.damageToPlayerBonus;
        return player.TakeDamage(enemyName, attack.attackName, damage);
    }

    private int GetMaxWeight()
    {
        int weight = 0;
        foreach(var attack in attacks)
            weight += attack.usageWeight;
        
        return weight;
    }
    #endregion

    #region //Damage and healing
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        GameText.SetText($"{enemyName} took {damage} damage!");
        OnHealthChange?.Invoke();
        StartCoroutine(HitFlash());

        if(currentHP > 0) return;
        GameText.AppendText(" It has been defeated!");
        OnDeath?.Invoke(this);
        image.enabled = false;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        GameText.SetText($"{enemyName} healed {amount} HP!");
        OnHealthChange?.Invoke();
    }

    private IEnumerator HitFlash()
    {
        blinker.enabled = true;
        yield return new WaitForSeconds(hitFlashDuration);
        blinker.enabled = false;
    }

    public float GetHealthPercentage() { return (float)currentHP / (float)maxHP; }
    #endregion

    #region //Targeting
    public void SetAsTarget(ButtonBehavior action)
    {
        targetButton.interactable = true;
        targetButton.onClick.AddListener(delegate{action();});
    }

    public void RemoveAsTarget()
    {
        targetButton.interactable = false;
        targetButton.onClick.RemoveAllListeners();
    }
    #endregion
}

[System.Serializable]
public class EnemyAttack
{
    public string attackName = "";
    public AudioClip clip = null;
    public int damageDiceToPlayer = 0;
    public int damageDieToPlayer = 0;
    public int damageToPlayerBonus = 0;
    public int healingToSelf = 1;
    public int duration = 1;
    public bool poisons = false;
    [Range(1,5)] public int usageWeight = 1;
}