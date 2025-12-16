using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public TextMeshProUGUI coinTMP;

    public RectTransform coinImageRT;

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

    public void SetCoinCount(int coinCount)
    {
        coinTMP.text  = coinCount.ToString();
        coinImageRT.DOKill();
        coinImageRT.anchoredPosition = new Vector3(-92, 0, 0);
        coinImageRT.DOAnchorPos3DY(20, .2f).SetLoops(2, LoopType.Yoyo);
    }
}
