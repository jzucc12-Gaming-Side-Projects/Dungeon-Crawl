using TMPro;
using UnityEngine;

public class RouteButtonUI : MonoBehaviour
{
    private RouteButton routeButton = null;
    [SerializeField] private TextMeshProUGUI buttonText = null;
    [SerializeField] private TextMeshProUGUI pathDescription = null;
    [SerializeField] string[] treasureText = new string[0];
    [SerializeField] string[] hazardText = new string[0];
    [SerializeField] string[] revitalizeText = new string[0];
    [SerializeField] string[] magicHazardText = new string[0];



    private void Awake()
    {
        routeButton = GetComponent<RouteButton>();
    }

    private void OnEnable()
    {
        routeButton.OnRouteUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        routeButton.OnRouteUpdated -= UpdateUI;
    }

    private void UpdateUI(RouteData data)
    {
        ButtonText(data);
        PathText(data);
    }

    private void ButtonText(RouteData data)
    {
        string time = $"{data.timeCount} seconds\n";
        string upgrades = "";
        string potions = "";
        string hpmp = "";

        if (data.upgradeShield) upgrades = "Upgrade Shield\n";
        else if (data.upgradeSword) upgrades = "Upgrade Sword\n";

        if (data.potionChange > 0)
        {
            potions = $"+{data.potionChange} potion";
            if (data.potionChange > 1) potions += "s";
            potions += "\n";
        }
        else if (data.potionChange < 0)
        {
            potions = $"{data.potionChange} potion";
            if (data.potionChange < -1) potions += "s";
            potions += "\n";
        }

        if (data.hpChange > 0)
        {
            hpmp = $"+{data.hpChange} HP";
        }
        else if (data.hpChange < 0)
        {
            hpmp = $"{data.hpChange} HP";
        }

        if (data.mpChange > 0)
        {
            hpmp = $"+{data.mpChange} MP";
        }
        else if (data.mpChange < 0)
        {
            hpmp = $"{data.mpChange} MP";
        }

        buttonText.text = $"{time}{upgrades}{potions}{hpmp}";
    }

    private void PathText(RouteData data)
    {
        string pathText = "Just a boring old path";
        bool useMod = false;

        if(data.upgradeShield || data.upgradeSword || data.potionChange > 0)
        {
            pathText = ChooseText(treasureText);
            useMod = true;
        }

        if(data.mpChange > 0 || data.hpChange > 0)
        {
            if(useMod)
            {
                pathText += "\nand ";
                pathText += ChooseText(revitalizeText);
            }
            else
            {
                pathText = ChooseText(revitalizeText);
            }
        }

        if(data.potionChange < 0 || data.hpChange < 0)
        {
            if(useMod)
            {
                pathText += "\nbut ";
                pathText += ChooseText(hazardText);
            }
            else
            {
                pathText = ChooseText(hazardText);
            }
        }

        if(data.mpChange < 0)
        {
            if(useMod)
            {
                pathText += "\nbut ";
                pathText += ChooseText(magicHazardText);
            }
            else
            {
                pathText = ChooseText(magicHazardText);
            }
        }
        string finalText = char.ToUpper(pathText[0]) + pathText.Substring(1).ToLower();
        pathDescription.text = finalText;
    }

    private string ChooseText(string[] options)
    {
        int choice = Random.Range(0, options.Length);
        return options[choice];
    }
}