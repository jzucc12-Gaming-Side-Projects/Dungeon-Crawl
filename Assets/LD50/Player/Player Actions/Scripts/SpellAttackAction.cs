using UnityEngine;

public class SpellAttackAction : SpellActionButton
{
    [SerializeField] private int damageDice = 1;
    [SerializeField] private int damageDie = 4;

    protected override void Cast()
    {
        combatManager.StartTargetSelect(Attack);
    }

    private void Attack(Enemy target)
    {
        int damage = Helper.DamageRoller(damageDice, damageDie);
        target.TakeDamage(damage);
        PerformAction();
        player.UseMP(mpCost);
        combatManager.ChangeTurn();
    }

    public (int, int) GetDamageRange()
    {
        return Helper.GetDamageRange(damageDice, damageDie, 0);
    }
}
