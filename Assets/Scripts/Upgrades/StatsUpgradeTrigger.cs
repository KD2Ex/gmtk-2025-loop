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
        //print(other.gameObject.name);
        if (activated) return;
        UpgradeStat(Random.Range(1,5), other.GetComponent<Player>());
        activated = true;
    }

    public void ResetTrigger()
    {
        activated = false;
    }

    private void UpgradeStat(int statNum, Player player)
    {
        switch (statNum)
        {
            case 1:
                playerStats.health += health;
                player.ShowUpgrade($"Health +{health}%");
                break;
            case 2:
                playerStats.damage += damage;
                player.ShowUpgrade($"Damage +{damage}%");
                break;
            case 3:
                playerStats.moveSpeed += moveSpeed;
                player.ShowUpgrade($"Move Speed +{moveSpeed}%");
                break;
            case 4:
                playerStats.attackDelay += attackDelay;
                player.ShowUpgrade($"Attack Speed +{attackDelay}%");
                break;
        }
    }
}
