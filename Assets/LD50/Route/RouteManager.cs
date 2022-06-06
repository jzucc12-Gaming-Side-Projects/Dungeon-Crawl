using System;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    #region //Cached Components
    private Canvas canvas = null;
    private RouteButton[] buttons = new RouteButton[0];
    private TimeTracker tracker = null;
    private Player player = null;
    private PotionHolder potionHolder = null;
    private Equipment equipment = null;
    #endregion

    #region //Route Layout
    [Tooltip("Array size is the number of routes, and therefore combats. Element value is the number of branch choices before a combat.")] 
    [SerializeField] private int[] routeLayout = new int[0];
    [SerializeField] private int timeMultiplier = 3;
    private int routeIndex = 0; //Which index in the route layout array you are currently on
    private int branchCount => routeLayout[routeIndex];
    private int branchNumber = 0; //Which branch you are on in the current route
    public static event Action OnRouteFinish;
    private string chooseText = "Choose a path to explore!";
    private AudioSource source = null;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        tracker = FindObjectOfType<TimeTracker>();
        canvas = GetComponent<Canvas>();
        buttons = GetComponentsInChildren<RouteButton>();
        player = FindObjectOfType<Player>();
        potionHolder = FindObjectOfType<PotionHolder>();
        equipment = FindObjectOfType<Equipment>();
    }

    private void Start()
    {
        StartUp();
    }

    private void OnEnable()
    {
        RouteButton.OnRouteChosen += RouteChosen;
    }

    private void OnDisable()
    {
        RouteButton.OnRouteChosen -= RouteChosen;
    }
    #endregion

    #region //Activation
    public void StartUp()
    {
        canvas.enabled = true;
        UpdateRoutes();
        GameText.SetText(chooseText);
    }

    public void Shutdown()
    {
        canvas.enabled = false;
    }
    #endregion

    #region //Getters
    public IEnumerable<int> GetLayout() { return routeLayout; }
    #endregion

    #region //Updating routes
    private void RouteChosen(RouteData data)
    {
        bool died = false;
        tracker.UpdateTime(data.timeCount);
        GameText.SetText(chooseText);

        if(data.upgradeShield) equipment.UpgradeShield();
        if(data.upgradeSword) equipment.UpgradeSword();
        if(data.potionChange != 0) potionHolder.AddPotions(data.potionChange);

        if(data.mpChange > 0) player.RestoreMP(data.mpChange);
        else if(data.mpChange < 0) player.UseMP(data.mpChange);

        if(data.hpChange > 0) player.Heal(data.hpChange);
        else if(data.hpChange < 0) died = player.TakeDamage(data.hpChange);

        if(!died)
            source.Play();
    
        UpdateRoutes();
        branchNumber++;

        if(branchNumber >= branchCount)
        {
            routeIndex++;
            branchNumber = 0;
            Shutdown();
            OnRouteFinish?.Invoke();
        }
    }

    private void UpdateRoutes()
    {
        int newTimeCount = GetTimeIncrement();
        for(int ii = 0; ii < buttons.Length; ii++)
        {
            RouteData data = new RouteData();
            data.timeCount = newTimeCount;
            newTimeCount += GetTimeIncrement();

            float roll = UnityEngine.Random.Range(0f, 1f);

            if(ii == 0)
                data = FirstRouteBoons(data, roll);
            else if(ii < buttons.Length - 1)
                data = MiddleRouteBoons(data, roll);
            else
                data = LastRouteBoons(data, roll);

            buttons[ii].SetRouteData(data);
        }
    }

    private RouteData FirstRouteBoons(RouteData data, float roll)
    {

        if(data.timeCount < timeMultiplier * 2)
        {
            if(roll > 0.6f && equipment.GetShieldLevel() < 3)
            {
                data.upgradeShield = true;
            }

            if(!player.MaxMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(10, 15 + 1);
            }
            else
            {
                data.potionChange += UnityEngine.Random.Range(1, 2 + 1);
            }
        }
        else if(data.timeCount <= timeMultiplier * 4)
        {
            if(roll > 0.7f && equipment.GetShieldLevel() < 1)
            {
                data.upgradeShield = true;
            }

            if(!player.MaxMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(5, 10 + 1);
            }
            else if(!player.MaxHP())
            {
                data.hpChange = UnityEngine.Random.Range(10, 20 + 1);
            }
            else
            {
                data.potionChange++;
            }
        }
        else
        {
            if(roll > 0.7f)
            {
                data.potionChange = UnityEngine.Random.Range(1, 2 + 1);
            }

            if(!player.MaxMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(5, 10 + 1);
            }
            else if(!player.MaxHP())
            {
                data.hpChange = UnityEngine.Random.Range(5, 15 + 1);
            }
        }

        return data;
    }
    private RouteData MiddleRouteBoons(RouteData data, float roll)
    {

        if(data.timeCount <= 2 * timeMultiplier * 2)
        {
            if(roll > 0.7f && equipment.GetShieldLevel() < 1) 
            {
                data.upgradeShield = true;
            }
            else if(roll < 0.2f)
            {
                data.potionChange++; 
            }

            if(!player.MaxMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(1, 5 + 1);
            }
            else
            {
                data.potionChange++;
            }
        }
        else if(data.timeCount <= 2 * timeMultiplier * 4)
        {
            if(roll > 0.6f)
            {
                data.potionChange++;
            }

            if(!player.MaxMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(1, 5 + 1);
            }
            else if(!player.MaxHP())
            {
                data.hpChange = UnityEngine.Random.Range(1, 5 + 1);
            }
        }
        else
        {
            if(roll > 0.7f && equipment.GetSwordLevel() < 1) data.upgradeSword = true;
            if(!player.MaxHP())
            {
                data.hpChange = UnityEngine.Random.Range(-5, 5 + 1);
            }
            else
            {
                data.potionChange++;
            }
        }

        return data;
    }
    private RouteData LastRouteBoons(RouteData data, float roll)
    {

        if(data.timeCount <= 3 * timeMultiplier * 2)
        {
            if(roll < 0.3f)
            {
                data.mpChange = UnityEngine.Random.Range(-2, 0 + 1);
            }
            else if(roll < 0.6f)
            {
                data.hpChange = UnityEngine.Random.Range(-5, 0 + 1);
            }
        }
        else if(data.timeCount <= 3 * timeMultiplier * 4)
        {
            if(roll > 0.65f && equipment.GetSwordLevel() < 1) 
            {
                data.upgradeSword = true;
            }
            else if(!potionHolder.OutOfPotions() && roll > 0.4f)
            {
                data.potionChange--; 
            }

            if(!player.OutOfMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(-5, 0 + 1);
            }
            else
            {
                data.hpChange = UnityEngine.Random.Range(-10, -5 + 1);
            }
        }
        else
        {
            if(roll > 0.5f && equipment.GetSwordLevel() < 3) 
            {
                data.upgradeSword = true;
            }
            else if(!potionHolder.OutOfPotions())
            {
                data.potionChange = UnityEngine.Random.Range(-2, -1 + 1); 
            }

            if(!player.OutOfMP() && roll < 0.5f)
            {
                data.mpChange = UnityEngine.Random.Range(-10, -5 + 1);
            }
            else
            {
                data.hpChange = UnityEngine.Random.Range(-20, -10 + 1);
            }
        }

        return data;
    }

    private int GetTimeIncrement()
    {
        return timeMultiplier * ((int)UnityEngine.Random.Range(1f, 4f) + (int)UnityEngine.Random.Range(0f, 2f));
    }
    #endregion
}