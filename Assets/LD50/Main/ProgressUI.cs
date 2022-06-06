using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Transform display = null;
    private RouteManager routeManager = null;
    private int obstacleCount = 0;
    private float percentChange => 1f / obstacleCount;
    private float percentage = 0f;


    #region //Monobehaviour
    private void Awake()
    {
        routeManager = FindObjectOfType<RouteManager>();
    }

    private void OnEnable()
    {
        RouteButton.OnRouteChosen += DecrementFromRoute;
        EncounterManager.OnEncounterFinished += DecrementFromCombat;
    }

    private void OnDisable()
    {
        RouteButton.OnRouteChosen -= DecrementFromRoute;
        EncounterManager.OnEncounterFinished -= DecrementFromCombat;
    }

    private void Start()
    {
        foreach(var section in routeManager.GetLayout())
            obstacleCount += section + 1;

        UpdateUI();
    }
    #endregion

    #region //Update UI
    private void DecrementFromRoute(RouteData _)
    {
        ChangePercentage();
    }

    private void DecrementFromCombat(bool _)
    {
        ChangePercentage();
    }

    private void ChangePercentage()
    {
        percentage += percentChange;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Vector2 newScale = new Vector2(percentage, 1);
        display.transform.localScale = newScale;
    }
    #endregion
}