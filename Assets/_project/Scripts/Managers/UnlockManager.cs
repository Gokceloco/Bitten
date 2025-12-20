using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public List<int> unlockLevels;
    public List<string> unlockNames;
    public List<Sprite> unlockSprites;
    public List<Sprite> unlockSpritesBG;

    public UnlockUI unlockUI;

    public void ShowUnlockUI(int levelNo)
    {
        var nextUnlockIndex = 0;

        for (int i = 0; i < unlockLevels.Count; i++)
        {
            if (levelNo <= unlockLevels[i])
            {
                nextUnlockIndex = i; 
                break;
            }
        }

        var start = 0f;

        var previousUnlockLevel = 0;

        if (nextUnlockIndex > 0)
        {
            previousUnlockLevel = unlockLevels[nextUnlockIndex - 1];
        }

        var fillPercent = (levelNo - previousUnlockLevel) / 5f;

        start = (fillPercent-.2f) * 800;

        var t = "NEXT UNLOCK " + fillPercent * 100 + "%";
        if (fillPercent == 1)
        {
            t = unlockNames[nextUnlockIndex] + " UNLOCKED!";
        }

        if (levelNo > unlockLevels[^1])
        {
            return;
        }

        unlockUI.ShowUnlock
            (unlockSprites[nextUnlockIndex],
            unlockSpritesBG[nextUnlockIndex], start,
            t);
    }
}
