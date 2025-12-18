using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUnit : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI levelTMP;
    public TextMeshProUGUI costTMP;

    public void SetUnitText(int level, int cost)
    {
        levelTMP.text = "LEVEL " + level;
        costTMP.text = cost.ToString();
    }

    public void SetButtonInteractable(bool v)
    {
        button.interactable = v;
    }
}
