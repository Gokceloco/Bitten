using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image bg;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);
        bg.rectTransform.DOKill();
        bg.rectTransform.anchoredPosition = new Vector3(0,-300,0);
        bg.rectTransform.DOAnchorPosY(300, 60).SetLoops(-1, LoopType.Yoyo);

    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(()=>gameObject.SetActive(false));
    }
}
