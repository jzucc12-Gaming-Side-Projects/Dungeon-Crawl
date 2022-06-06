public class BlockButtonUI : PlayerActionButtonUI
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
        equipment.ShieldUpgraded += UpdateUI;
    }

    private void OnDisable()
    {
        equipment.ShieldUpgraded -= UpdateUI;
    }

    private void UpdateUI()
    {
        if(equipment.GetShieldLevel() == 0)
        {
            buttonText.text = $"Block With Shield\n-{equipment.GetBlockReductionAsInt()}% Damage\n{DisplayDuration()}";
        }
        else
        {
            buttonText.text = $"Block With +{equipment.GetShieldLevel()} Shield\n-{equipment.GetBlockReductionAsInt()}% Damage\n{DisplayDuration()}";
        }
    }
}
