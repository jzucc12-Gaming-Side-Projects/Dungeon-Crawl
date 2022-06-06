using TMPro;
using UnityEngine;

public class GameText : MonoBehaviour
{
    private static TextMeshProUGUI gameText = null;

    private void Awake()
    {
        gameText = GetComponent<TextMeshProUGUI>();
        gameText.text = "";
    }

    public static void SetText(string text)
    {
        gameText.text = text;
    }

    public static void AppendText(string text)
    {
        gameText.text += text;
    }
}
