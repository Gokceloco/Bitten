using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public Image bg;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float delay)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f).SetDelay(delay);

    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }
}
