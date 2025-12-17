using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoinCount;
    public CoinUI coinUI;
    
    public void SetStartingCoinCount(int v)
    {
        totalCoinCount = v;
        UpdateCoinUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CoinCollected(100);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpendCoins(50);
        }
    }   

    public void CoinCollected(int value)
    {
        totalCoinCount += value;
        UpdateCoinUI();
        PlayerPrefs.SetInt("CoinCount", totalCoinCount);
    }

    public void SpendCoins(int value)
    {
        totalCoinCount -= value;
        UpdateCoinUI();
        PlayerPrefs.SetInt("CoinCount", totalCoinCount);
    }

    private void UpdateCoinUI()
    {
        coinUI.SetCoinCount(totalCoinCount);
    }

    
}
