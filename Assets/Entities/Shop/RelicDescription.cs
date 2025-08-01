using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicDescription : MonoBehaviour
{
    public string description;
    private TextMeshProUGUI textBox;

    private void Start()
    {
        textBox =  GameObject.FindWithTag("ShopTextBox").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        textBox.text = description;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        textBox.text = null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        textBox.text = description;
    }
}
