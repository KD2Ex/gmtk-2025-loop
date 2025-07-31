using Unity.Collections;
using UnityEngine;


public class StatsUpgradeTrigger : MonoBehaviour
{
    [SerializeField] private StatsSO playerStats;
    [SerializeField] private GlobalTimer timer;

    [Header("UpgradeAmount")] 
    public float health;
    public float damage;
    public float moveSpeed;
    public float attackDelay;


    public bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        UpgradeStat(Random.Range(1,5));

        activated = true;
    }

    public void ResetTrigger()
    {
        activated = false;
    }

    private void UpgradeStat(int statNum)
    {
        switch (statNum)
        {
            case 1:
                playerStats.health += health;
                Debug.Log($"health + {health}%");
                break;
            case 2:
                playerStats.damage += damage;
                Debug.Log($"damage + {damage}%");
                break;
            case 3:
                playerStats.moveSpeed += moveSpeed;
                Debug.Log($"moveSpeed + {moveSpeed}%");
                break;
            case 4:
                playerStats.attackDelay += attackDelay;
                Debug.Log($"attackDelay + {attackDelay}%");
                break;
        }
    }
}
