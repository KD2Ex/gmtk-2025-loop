using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image bar;

    private void Update()
    {
        switch (GameManager.instance.DifficultyLevel)
        {
            case 1:
                bar.fillAmount = 0.135f;
                break;
            case 2:
                bar.fillAmount = 0.228f;
                break;
            case 3:
                bar.fillAmount = 0.319f;
                break;
            case 4:
                bar.fillAmount = 0.4f;
                break;
            case 5:
                bar.fillAmount = 0.5f;
                break;
            case 6:
                bar.fillAmount = 0.61f;
                break;
            case 7:
                bar.fillAmount = 0.7f;
                break;
            case 8:
                bar.fillAmount = 0.781f;
                break;
            case 9:
                bar.fillAmount = 0.873f;
                break;
            case 10:
                bar.fillAmount = 1f;
                break;
        }
    }
}
