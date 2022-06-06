using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    private TextMeshProUGUI timeText = null;
    [SerializeField] private string header = "Time Left: "; 


    private void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        TimeTracker.OnTimeUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        TimeTracker.OnTimeUpdated -= UpdateUI;
    }

    private void UpdateUI(int current, int goal)
    {
        timeText.text = $"{header} {goal - current} seconds";
    }
}
