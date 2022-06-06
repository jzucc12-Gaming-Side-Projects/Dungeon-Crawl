using UnityEngine;

public class PlayerPotionAction : PlayerActionButton
{
    [SerializeField] private string noPotionText = "Out of Potions";
    [SerializeField] private string fullHPText = "Can't drink at max health";
    private PotionHolder potionHolder = null;
    private Player player = null;
    

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        potionHolder = FindObjectOfType<PotionHolder>();
    }
    
    protected override void Action()
    {
        if(potionHolder.OutOfPotions())
            GameText.SetText(noPotionText);
        else if(player.MaxHP())
            GameText.SetText(fullHPText);
        else
        {
            DisplayBaseText();
            potionHolder.UsePotion();
            PerformAction();
            combatManager.ChangeTurn();
        }
    }
}
