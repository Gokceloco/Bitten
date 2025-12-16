using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image fillBar;
    public Image blackGrenadeIcon;
    public Image grenadeIcon;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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
    public void SetFillBar(float amount)
    {
        fillBar.fillAmount = amount;
        if (amount < 1)
        {
            blackGrenadeIcon.gameObject.SetActive(true);
            grenadeIcon.gameObject.SetActive(false);
        }
        else
        {
            blackGrenadeIcon.gameObject.SetActive(false);
            grenadeIcon.gameObject.SetActive(true);
        }
    }
}
