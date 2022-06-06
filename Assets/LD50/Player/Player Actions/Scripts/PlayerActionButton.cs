using JZ.AUDIO;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerActionButton : MonoBehaviour
{
    #region //Cached components
    protected CombatManager combatManager = null;
    private TimeTracker tracker = null;
    private Button button = null;
    #endregion
    [SerializeField] private string displayText = "Choose a target to Attack";
    [SerializeField] private int actionDuration = 6;
    private SoundPlayer sfxPlayer = null;


    #region //Monobehaviour
    protected virtual void Awake()
    {
        sfxPlayer = GetComponent<SoundPlayer>();
        tracker = FindObjectOfType<TimeTracker>();
        combatManager = FindObjectOfType<CombatManager>();
        button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(Action);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(Action);
    }

    protected virtual void Start() { }
    #endregion

    #region //Performing the action
    protected abstract void Action();

    protected void DisplayBaseText()
    {
        GameText.SetText(displayText);
    }

    protected void PerformAction()
    {
        if(!tracker.UpdateTime(actionDuration))
            sfxPlayer.Play("Action");
    }
    #endregion

    #region //Getters
    public int GetDuration() { return actionDuration; }
    #endregion
}
