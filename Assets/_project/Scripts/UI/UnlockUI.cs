using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockUI : MonoBehaviour
{
    public Image unlockImage;
    public Image unlockImageBG;
    public TextMeshProUGUI unlockTMP;

    private CanvasGroup _canvasGroup;

    public RectMask2D rectMask;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowUnlock(Sprite sprite, Sprite spriteBG, float start, string t)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1,.2f).SetDelay(3);
        unlockImage.sprite = sprite;
        unlockImageBG.sprite = spriteBG;
        unlockTMP.text = t;
        StartCoroutine(FillCoroutine(start));
    }

    IEnumerator FillCoroutine(float start)
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < 160; i++)
        {
            rectMask.padding = new Vector4(0,0,0,800 - start - i);
            yield return new WaitForSeconds(1f/320);
        }
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0,.2f).OnComplete(()=>gameObject.SetActive(false));
    }

    public void ContinueButtonPressed()
    {
        Hide();
    }
}
