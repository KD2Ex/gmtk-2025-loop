using System;
using TMPro;
using UnityEngine;

public class DifficultyNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        text.enabled = false;
    }

    void Update()
    {
        if (GameManager.instance.DifficultyLevel <= 10) return;

        text.enabled = true;
        text.text = $"Difficulty: {GameManager.instance.DifficultyLevel}";
    }
}
