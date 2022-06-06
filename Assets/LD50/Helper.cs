using UnityEngine;

public static class Helper
{
    public static int DamageRoller(int damageDice, int damageDie)
    {
        int damage = 0;
        for (int ii = 0; ii < damageDice; ii++)
            damage += Random.Range(1, damageDie + 1);
        return damage;
    }

    public static (int, int) GetDamageRange(int damageDice, int damageDie, int bonus)
    {
        int min = damageDice + bonus;
        int max = damageDice * damageDie + bonus;
        return (min, max);
    }
}
