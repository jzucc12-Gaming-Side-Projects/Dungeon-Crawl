using System;
using UnityEngine;

public class PotionHolder : MonoBehaviour
{
    private Player player = null;
    [SerializeField] private int potions = 1;
    [SerializeField] private int potionHealAmount = 10;
    public event Action<int> ChangePotionCount;


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        ChangePotionCount?.Invoke(potions);
    }

    public bool OutOfPotions() { return potions == 0; }
    public void UsePotion()
    {
        player.Heal(potionHealAmount);
        potions--;
        ChangePotionCount?.Invoke(potions);
    }

    public void RemovePotions(int amount)
    {
        potions = Mathf.Max(0, potions - amount);
        ChangePotionCount?.Invoke(potions);
    }

    public void AddPotions(int amount)
    {
        potions = Mathf.Max(0, potions + amount);
        ChangePotionCount?.Invoke(potions);
    }

    public int GetHealAmount() { return potionHealAmount; }
}
