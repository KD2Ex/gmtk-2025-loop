using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<Relic> relics = new List<Relic>();

    public List<Relic> relicsInShop = new();


    public void FillShop()
    {
        for (int i = 0; i < 4; i++)
        {
            //var relic = GetRandomRelic();
            
            //relicsInShop.Add(relics[i]);
        }
    }

}
