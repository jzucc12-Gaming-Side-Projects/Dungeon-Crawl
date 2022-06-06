using UnityEngine;

public class SpellActionButtonUI : PlayerActionButtonUI
{
    private SpellActionButton spellButton => (SpellActionButton)actionButton;
    private SpellAttackAction attackAction;

    private void Start()
    {
        attackAction = GetComponent<SpellAttackAction>();
        DisplayButtonText();
    }

    private void DisplayButtonText()
    {
        string top = $"Cast {spellButton.GetSpellName()} ({spellButton.GetCost()} MP)\n";
        string middle = "No damage\n";
        string bottom = DisplayDuration();

        if(attackAction != null)
        {
            int minDmg;
            int maxDmg;
            (minDmg, maxDmg) = attackAction.GetDamageRange();
            middle = $"{minDmg} - {maxDmg} Damage\n";
        }

        buttonText.text = $"{top}{middle}{bottom}";
    }
}
