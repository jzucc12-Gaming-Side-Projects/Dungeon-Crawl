using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    #region //Cached components
    private Canvas canvas = null;
    #endregion

    #region //Encounters
    [SerializeField] private Transform encounterContainer = null;
    [SerializeField] private EncounterBlock[] encounterBlocks = new EncounterBlock[0];
    private Queue<Encounter> encounterQueue = new Queue<Encounter>();
    public Encounter activeEncounter { get; private set; }
    public static event Action OnEncounterStart;
    public static event Action<bool> OnEncounterFinished;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        activeEncounter = null;
        canvas = GetComponent<Canvas>();
        SpawnEncounters();
    }

    private void Start()
    {
        Shutdown();
    }
    #endregion

    #region //Activation
    public void StartUp()
    {
        canvas.enabled = true;
        StartEncounter();
    }

    public void Shutdown()
    {
        canvas.enabled = false;
    }

    private void SpawnEncounters()
    {
        int count = 1;
        foreach(var encounterblock in encounterBlocks)
        {
            var encounter = encounterblock.GetEncounter();
            Encounter spawned = Instantiate(encounter, encounterContainer);
            spawned.gameObject.name = $"Encounter {count}";
            spawned.gameObject.SetActive(false);
            encounterQueue.Enqueue(spawned);
            count++;
        }
    }

    private void StartEncounter()
    {
        GameText.SetText("Enemies appeared! Choose an action!");
        activeEncounter = encounterQueue.Dequeue();
        activeEncounter.gameObject.SetActive(true);
        activeEncounter.EncounterOver += EndEncounter;
        OnEncounterStart?.Invoke();
    }

    public void EndEncounter(float delayTime, string endText = "")
    {
        StartCoroutine(EndEncounterRoutine(delayTime, endText));

    }

    private IEnumerator EndEncounterRoutine(float delayTime, string endText)
    {
        yield return new WaitForSeconds(delayTime);
        if(!string.IsNullOrEmpty(endText))
        {
            GameText.SetText(endText);
            yield return new WaitForSeconds(delayTime);
        }

        activeEncounter.EncounterOver -= EndEncounter;
        Destroy(activeEncounter.gameObject);
        activeEncounter = null;
        Shutdown();
        OnEncounterFinished?.Invoke(encounterQueue.Count == 0);
    }
    #endregion

    #region //Getters
    public IEnumerable<Enemy> GetEnemies()
    {
        if(activeEncounter == null) return null;
        return activeEncounter.GetEnemies();
    }
    #endregion
}