using System;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private int currenTimeGoal = 500;
    private static int staticTimeGoal = -1;
    private int currentTime = 0;
    public static event Action<int, int> OnTimeUpdated;
    public static event Action OnVictory;


    private void Start()
    {
        if(staticTimeGoal != -1) currenTimeGoal = staticTimeGoal;
        OnTimeUpdated?.Invoke(currentTime, currenTimeGoal);
    }

    public static void SetGoal(int goal) { staticTimeGoal = goal; }
    public bool UpdateTime(int amount) 
    { 
        currentTime += amount; 
        OnTimeUpdated?.Invoke(currentTime, currenTimeGoal);
        
        if(currentTime < currenTimeGoal) return false;
        OnVictory?.Invoke();
        return true;
    }
}