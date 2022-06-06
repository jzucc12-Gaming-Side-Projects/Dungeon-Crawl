public class AttackButtonUI : PlayerActionButtonUI
{
    private Equipment equipment = null;


    protected override void Awake()
    {
        base.Awake();
        equipment = FindObjectOfType<Equipment>();
    }

    private void OnEnable()
    {
        UpdateUI();
        equipment.SwordUpgraded += UpdateUI;
    }

    private void OnDisable()
    {
        equipment.SwordUpgraded -= UpdateUI;
    }

    private void UpdateUI()
    {
        int minDmg;
        int maxDmg;
        (minDmg, maxDmg) = equipment.GetDamageRange();

        if(equipment.GetSwordLevel() == 0)
        {
            buttonText.text = $"Attack With Sword\n{minDmg} - {maxDmg} Damage\n{DisplayDuration()}";
        }
        else
        {
            buttonText.text = $"Attack With +{equipment.GetSwordLevel()} Sword\n{minDmg} - {maxDmg} Damage\n{DisplayDuration()}";
        }
    }
}
