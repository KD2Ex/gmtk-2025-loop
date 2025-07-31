using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubScript : MonoBehaviour
{
    [SerializeField] private List<PortalScript> loopPortals;
    [SerializeField] private List<StatsUpgradeTrigger> statsTriggers;
    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var loopPortal in loopPortals)
        {
            loopPortal.activated = false;
        }

        foreach (var statsTrigger in statsTriggers)
        {
            statsTrigger.activated = false;
        }
    }
}
