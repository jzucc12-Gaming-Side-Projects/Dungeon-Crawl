using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private Player player = null;

    #region //HP display
    [Header("HP Display")]
    [SerializeField] private TextMeshProUGUI hpHeader = null;
    [SerializeField] private TextMeshProUGUI hpText = null;
    [SerializeField] private Color hpTextHeaderColor = Color.red;
    #endregion

    #region //MP display
    [Header("MP Display")]
    [SerializeField] private TextMeshProUGUI mpHeader = null;
    [SerializeField] private TextMeshProUGUI mpText = null;
    [SerializeField] private Color mpTextHeaderColor = Color.blue;
    #endregion

    #region //Status effects
    [Header("Status Effect")]
    [SerializeField] private TextMeshProUGUI statusText = null;
    [SerializeField] private string okayText = "Okay";
    [SerializeField] private string poisonText = "Poisoned";
    [SerializeField] private Color okayColor = Color.white;
    [SerializeField] private Color poisonColor = Color.green;
    #endregion

    #region //Monobehaviour
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        player.OnHPChange += UpdateHPText;
        player.OnMPChange += UpdateMPText;
        player.changeStatus += UpdateStatusText;
    }

    private void OnDisable()
    {
        player.OnHPChange -= UpdateHPText;
        player.OnMPChange -= UpdateMPText;
        player.changeStatus -= UpdateStatusText;
    }

    private void Start()
    {
        hpHeader.color = hpTextHeaderColor;
        mpHeader.color = mpTextHeaderColor;
        UpdateHPText();
        UpdateMPText();
        UpdateStatusText();
    }
    #endregion

    #region //Update UI
    private void UpdateHPText()
    {
        var hpValues = player.GetHP();
        hpText.text = $"{hpValues.Item1} / {hpValues.Item2}";
    }

    private void UpdateMPText()
    {
        var mpValues = player.GetMP();
        mpText.text = $"{mpValues.Item1} / {mpValues.Item2}";
    }

    private void UpdateStatusText(bool isPoisoned = false)
    {
        if(isPoisoned)
        {
            statusText.text = poisonText;
            statusText.color = poisonColor;
        }
        else
        {
            statusText.text = okayText;
            statusText.color = okayColor;
        }
    }
    #endregion
}
