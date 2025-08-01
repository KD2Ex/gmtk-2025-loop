using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int coins;

    public TextMeshProUGUI text;

    private void Update()
    {
        text.text = $"Coins: {coins}";
    }
}
