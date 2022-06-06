using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region //UI Canvases
    [SerializeField] private RouteManager routeManager = null;
    [SerializeField] private EncounterManager encounterManager = null;
    [SerializeField] private Canvas mainCanvas = null;
    [SerializeField] private GameObject victoryScreen = null;
    [SerializeField] private GameObject defeatScreen = null;
    private bool gameOver = false;
    #endregion


    #region //Monobehaviour
    private void Start()
    {
        mainCanvas.enabled = true;
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
    }

    private void OnEnable()
    {
        RouteManager.OnRouteFinish += RouteFinished;
        EncounterManager.OnEncounterFinished += EncounterFinished;
        TimeTracker.OnVictory += Victory;
    }

    private void OnDisable()
    {
        RouteManager.OnRouteFinish -= RouteFinished;
        EncounterManager.OnEncounterFinished -= EncounterFinished;
        TimeTracker.OnVictory -= Victory;
    }
    #endregion

    #region //Progression
    private void RouteFinished()
    {
        if(gameOver) return;
        encounterManager.StartUp();
    }

    private void EncounterFinished(bool lastCombat)
    {
        if(gameOver) return;

        if(lastCombat)
        {
            mainCanvas.enabled = false;
            victoryScreen.SetActive(false);
            defeatScreen.SetActive(true);
            gameOver = true;
        }
        else
        {
            routeManager.StartUp();
        }
    }

    private void Victory()
    {
        if(gameOver == true) return;
        gameOver = true;
        routeManager.Shutdown();
        encounterManager.Shutdown();
        mainCanvas.enabled = false;
        victoryScreen.SetActive(true);
    }
    #endregion
}
