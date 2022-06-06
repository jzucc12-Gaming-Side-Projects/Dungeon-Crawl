using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Transform healthBar = null;
    [SerializeField] private Enemy enemy = null;


    private void OnEnable()
    {
        UpdateUI();
        enemy.OnHealthChange += UpdateUI;
        enemy.OnDeath += TurnOff;
    }

    private void OnDisable()
    {
        enemy.OnHealthChange -= UpdateUI;
        enemy.OnDeath -= TurnOff;
    }

    private void UpdateUI()
    {
        Vector2 newScale = new Vector2(enemy.GetHealthPercentage(), 1);
        healthBar.localScale = newScale;
    }

    private void TurnOff(Enemy _)
    {
        gameObject.SetActive(false);
    }
}
