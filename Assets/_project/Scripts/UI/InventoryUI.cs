using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public UIManager uIManager;
    private bool _isInventoryOpen;
    private CanvasGroup _canvasGroup;

    public List<Button> items;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (uIManager.gameDirector.gameState == GameState.GamePlay && Input.GetKeyDown(KeyCode.K))
        {
            InventoryButtonPressed();
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void InventoryButtonPressed()
    {
        if (_isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        _isInventoryOpen = true;

        for (int i = 0; i < items.Count; i++)
        {
            var rectTransform = items[i].GetComponent<RectTransform>();
            rectTransform.DOKill();
            rectTransform.DOAnchorPos3DY(200 - (i+1)*160, .2f);
            var buttonImage = items[i].GetComponent<Image>();
            buttonImage.DOKill();
            buttonImage.DOFade(1, .2f);
        }
    }

    private void CloseInventory()
    {
        _isInventoryOpen = false;
        for (int i = 0; i < items.Count; i++)
        {
            var rectTransform = items[i].GetComponent<RectTransform>();
            rectTransform.DOKill();
            rectTransform.DOAnchorPos3DY(200, .2f);
            var buttonImage = items[i].GetComponent<Image>();
            buttonImage.DOKill();
            buttonImage.DOFade(0, .2f);
        }
    }
    public void MachinegunButtonPressed()
    {
        uIManager.gameDirector.player.SwitchToMachineGun();
        InventoryButtonPressed();
    }
    public void ShotgunButtonPressed()
    {
        uIManager.gameDirector.player.SwitchToShotGun();
        InventoryButtonPressed();
    }
}
