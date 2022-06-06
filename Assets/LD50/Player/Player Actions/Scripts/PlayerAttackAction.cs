public class PlayerAttackAction : PlayerActionButton
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
        combatManager.StartTargetSelect(Attack);
    }

    private void Attack(Enemy target)
    {
        PerformAction();
        target.TakeDamage(equipment.CalculateDamage());
        combatManager.ChangeTurn();
    }
}