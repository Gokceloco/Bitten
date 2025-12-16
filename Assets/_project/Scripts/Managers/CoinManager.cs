using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoinCount;
    public CoinUI coinUI;

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

    public void ResetCoinCount()
    {
        totalCoinCount = 0;
        UpdateCoinUI();
    }

    public void CoinCollected(int value)
    {
        totalCoinCount += value;
        UpdateCoinUI();
    }

    public void SpendCoins(int value)
    {
        totalCoinCount -= value;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinUI.SetCoinCount(totalCoinCount);
    }
}
