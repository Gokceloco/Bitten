using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int attackUpgradeCount;
    public int defenceUpgradeCount;
    public int coinUpgradeCount;

    public void SetStartingUpgrades(int attack, int defence, int coins)
    {
        attackUpgradeCount = attack;
        defenceUpgradeCount = defence;
        coinUpgradeCount = coins;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeAttack();
        }
    }

    public void UpgradeAttack()
    {
        attackUpgradeCount++;
        PlayerPrefs.SetInt("AttackUpgrades", attackUpgradeCount);
    }
    public void UpgradeDefence()
    {
        defenceUpgradeCount++;
        PlayerPrefs.SetInt("DefenceUpgrades", defenceUpgradeCount);
    }
    public void UpgradeCoin()
    {
        coinUpgradeCount++;
        PlayerPrefs.SetInt("CoinUpgrades", coinUpgradeCount);
    }
}
