public class TimeSpellAction : SpellActionButton
{
    protected override void Cast()
    {
        PerformAction();
        player.UseMP(mpCost);
        combatManager.ChangeTurn();
    }
}