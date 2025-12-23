using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public TextMeshProUGUI messageTMP;
    private CanvasGroup _canvasGroup;
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
        _canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    public void ShowMessage(string msg, float duration, float delay)
    {
        StartCoroutine(MessageCoroutine(msg, duration, delay));
    }

    IEnumerator MessageCoroutine(string msg, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        messageTMP.DOKill();
        messageTMP.DOFade(1, .2f);
        messageTMP.text = msg;
        messageTMP.DOFade(0,.2f).SetDelay(duration);
    }
}
