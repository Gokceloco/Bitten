using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public TimerManager timerManager;

    private CanvasGroup _canvasGroup;
    public Image fillBar;
    public TextMeshProUGUI timerTMP;
    public TextMeshProUGUI urgentTMP;

    private float _previousRemainingTime;

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

    public void SetFillBar(float remainingTime, float totalTime)
    {
        fillBar.fillAmount = remainingTime / totalTime;
        timerTMP.text = Mathf.Round(remainingTime).ToString();
        if (Mathf.Round(remainingTime) < 6)
        {
            urgentTMP.gameObject.SetActive(true);
            timerTMP.gameObject.SetActive(false);
        }
        else
        {
            urgentTMP.gameObject.SetActive(false);
            timerTMP.gameObject.SetActive(true);
        }
        urgentTMP.text = Mathf.Round(remainingTime).ToString();

        if (Mathf.Round(remainingTime) != _previousRemainingTime)
        {
            
            if (Mathf.Round(remainingTime) < 6)
            {
                timerManager.gameDirector.audioManager.PlayTimeTickAS();
                urgentTMP.transform.localScale = Vector3.zero;
                urgentTMP.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBack);
            }
            _previousRemainingTime = Mathf.Round(remainingTime);
        }
    }
}
