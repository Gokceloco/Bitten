using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public CoinManager coinManager;
    public UpgradeUnit attackUU;
    public UpgradeUnit defenceUU;
    public UpgradeUnit lootUU;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float delay)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f).SetDelay(delay);
        UpdateButtons();
    }

    void UpdateButtons()
    {
        attackUU.SetUnitText(upgradeManager.attackUpgradeCount, 100);
        defenceUU.SetUnitText(upgradeManager.defenceUpgradeCount, 100);
        lootUU.SetUnitText(upgradeManager.lootUpgradeCount, 100);
        if(coinManager.totalCoinCount < 100)
        {
            attackUU.SetButtonInteractable(false);
            defenceUU.SetButtonInteractable(false);
            lootUU.SetButtonInteractable(false);
        }
        else
        {
            attackUU.SetButtonInteractable(true);
            defenceUU.SetButtonInteractable(true);
            lootUU.SetButtonInteractable(true);
        }
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void ExitButtonPressed()
    {
        Hide();
    }

    public void AttackUpgradeButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeAttack();        
        UpdateButtons();
    }
    public void DefenceUpgradeButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeDefence();
        UpdateButtons();
    }
    public void LootUpgradeButtonPressed()
    {
        coinManager.SpendCoins(100);
        upgradeManager.UpgradeLoot();
        UpdateButtons();
    }

}
