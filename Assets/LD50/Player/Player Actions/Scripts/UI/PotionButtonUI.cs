public class PotionButtonUI : PlayerActionButtonUI
{
    private PotionHolder potionHolder = null;

    protected override void Awake()
    {
        base.Awake();
        potionHolder = FindObjectOfType<PotionHolder>();
    }

    private void OnEnable()
    {
        potionHolder.ChangePotionCount += UpdateUI;
    }

    private void OnDisable()
    {
        potionHolder.ChangePotionCount -= UpdateUI;
    }

    private void UpdateUI(int amount)
    {
        buttonText.text = $"Drink Potion ({amount} left)\nHeals {potionHolder.GetHealAmount()} HP\n{DisplayDuration()}";
    }
}
