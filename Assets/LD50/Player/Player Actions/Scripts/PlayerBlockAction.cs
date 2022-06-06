public class PlayerBlockAction : PlayerActionButton
{
    private Equipment equipment = null;


    protected override void Awake()
    {
        base.Awake();
        equipment = FindObjectOfType<Equipment>();
    }

    protected override void Action()
    {
        DisplayBaseText();
        PerformAction();
        equipment.Block();
        combatManager.ChangeTurn();
    }
}
