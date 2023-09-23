using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldDisplay;

    public void UpdateGoldDisplay(int currentBalance)
    {
        goldDisplay.text = $"Gold: {currentBalance}";
    }
}
