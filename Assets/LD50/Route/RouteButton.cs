using System;
using UnityEngine;
using UnityEngine.UI;

public class RouteButton : MonoBehaviour
{
    #region //Cached Components
    private Button button = null;
    #endregion

    #region //Route variables
    [SerializeField] private RouteData routeData = new RouteData();
    public static event Action<RouteData> OnRouteChosen;
    public event Action<RouteData> OnRouteUpdated;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ChooseRoute);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ChooseRoute);
    }
    #endregion

    #region //Route Choosing
    private void ChooseRoute()
    {
        //Add Items if needed
        //Heal/Damage if needed
        //Change MP if needed
        OnRouteChosen?.Invoke(routeData);
    }
    #endregion

    #region //Getters
    public void SetRouteData(RouteData newData) 
    { 
        routeData = newData; 
        OnRouteUpdated?.Invoke(newData);
        
    }
    public RouteData GetRouteData() { return routeData; }
    #endregion
}