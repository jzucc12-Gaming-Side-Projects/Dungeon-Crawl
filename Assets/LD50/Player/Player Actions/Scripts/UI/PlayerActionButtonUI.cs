using TMPro;
using UnityEngine;

public class PlayerActionButtonUI : MonoBehaviour
{
    protected PlayerActionButton actionButton = null;
    [SerializeField] protected TextMeshProUGUI buttonText = null;


    protected virtual void Awake()
    {
        actionButton = GetComponent<PlayerActionButton>();
    }

    protected string DisplayDuration()
    {
        return $"[{actionButton.GetDuration()} seconds]";
    }
}