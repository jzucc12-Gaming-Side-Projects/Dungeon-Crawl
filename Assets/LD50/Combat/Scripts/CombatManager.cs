using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject playerButtonContainer = null;
    private Player player = null;
    private EncounterManager manager;
    private bool playerTurn = true;
    public delegate void CombatFunction(Enemy target);
    public CombatFunction playerCombatFunction;
    private bool combatActive = false;


    #region //Monobehaviour
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<EncounterManager>();
    }

    private void OnEnable()
    {
        SetPlayerTurn(true);
        EncounterManager.OnEncounterStart += StartCombat;
        EncounterManager.OnEncounterFinished += EndCombat;
    }

    private void OnDisable()
    {
        EncounterManager.OnEncounterStart -= StartCombat;
        EncounterManager.OnEncounterFinished -= EndCombat;
    }
    #endregion

    #region //Activation
    private void StartCombat()
    {
        player.OnDeath += EndEarly;
        combatActive = true;
        SetPlayerTurn(true);
    }

    private void EndCombat(bool _)
    {
        player.OnDeath -= EndEarly;
        combatActive = false;
        player.Poison(false);
    }

    private void EndEarly()
    {
        StopAllCoroutines();
        manager.EndEncounter(3f);
    }
    #endregion

    #region //Turn changing
    public void ChangeTurn()
    {
        if(!combatActive) return;
        SetPlayerTurn(!playerTurn);
    }

    private void SetPlayerTurn(bool isPlayerTurn)
    {
        playerTurn = isPlayerTurn;
        if(isPlayerTurn)
            MakeTurnPlayer();
        else
            StartCoroutine(MakeTurnEnemy());
    }

    private void MakeTurnPlayer()
    {
        player.SetBlockReduction(0);
        player.TakePoisonDamage();
        foreach(var button in playerButtonContainer.GetComponentsInChildren<Button>())
            button.interactable = true;
    }

    private IEnumerator MakeTurnEnemy()
    {
        float delayTime = 2f;
        foreach(var button in playerButtonContainer.GetComponentsInChildren<Button>())
            button.interactable = false;

        int count = 0;
        foreach(var enemy in manager.GetEnemies())
        {
            count ++;
            yield return new WaitForSeconds(delayTime);
            enemy.Attack();
            if(!combatActive) yield break;
        }

        if(count == 0) yield break;
        yield return new WaitForSeconds(delayTime);
        ChangeTurn();
    }
    #endregion

    #region //Target selection
    public void StartTargetSelect(CombatFunction function)
    {
        EndTargetSelect();
        playerCombatFunction = function;

        foreach(var enemy in GetCurrentEnemies())
        {
            enemy.SetAsTarget(delegate{PlayerCombatAction(enemy);});
        }
    }

    public void EndTargetSelect()
    {
        foreach(var enemy in GetCurrentEnemies())
        {
            enemy.RemoveAsTarget();
        }
    }

    public void PlayerCombatAction(Enemy target)
    {
        EndTargetSelect();
        playerCombatFunction.Invoke(target);
        playerCombatFunction = null;
    }
    #endregion

    #region //Getters
    private IEnumerable<Enemy> GetCurrentEnemies()
    {
        Encounter encounter = manager.activeEncounter;

        if(encounter == null) return null;
        else return encounter.GetEnemies();
    }
    #endregion
}
