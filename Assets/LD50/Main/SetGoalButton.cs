using JZ.MENU.BUTTON;
using TMPro;
using UnityEngine;

public class SetGoalButton : ButtonFunction
{
    [SerializeField] private string label = "Easy";
    [SerializeField] private int time = 400;

    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<TextMeshProUGUI>().text = $"{label}\n{time} seconds";
    }

    public override void OnClick()
    {
        TimeTracker.SetGoal(time);
    }
}
