using UnityEngine;

public abstract class SpellActionButton : PlayerActionButton
{
    [SerializeField] protected int mpCost = 5;
    [SerializeField] protected string spellName = "Magic Poke";
    protected Player player = null;
    private string notEnoughMPText = "Not Enough MP";

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }

    protected override void Action()
    {
        if(player.EnoughMP(mpCost))
        {
            DisplayBaseText();
            Cast();
        }
        else
        {
            GameText.SetText(notEnoughMPText);
        }
    }

    protected abstract void Cast();

    public string GetSpellName() { return spellName; }
    public int GetCost() { return mpCost; }
}
