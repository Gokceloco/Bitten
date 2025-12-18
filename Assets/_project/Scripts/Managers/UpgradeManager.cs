using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int attackUpgradeCount;
    public int defenceUpgradeCount;
    public int lootUpgradeCount;

    public void SetStartingUpgrades(int attack, int defence, int coins)
    {
        attackUpgradeCount = attack;
        defenceUpgradeCount = defence;
        lootUpgradeCount = coins;
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
    public void UpgradeLoot()
    {
        lootUpgradeCount++;
        PlayerPrefs.SetInt("CoinUpgrades", lootUpgradeCount);
    }
}
