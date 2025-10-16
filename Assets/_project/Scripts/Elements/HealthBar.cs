using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform fillBarParent;
    public Transform fillBarWhiteParent;
    public SpriteRenderer fillBarSpriteRenderer;

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void SetHealthBar(float ratio)
    {
        fillBarParent.transform.localScale = new Vector3(ratio, 1, 1);
        fillBarWhiteParent.DOKill();
        fillBarWhiteParent.DOScale(new Vector3(ratio, 1, 1), .2f).SetDelay(.1f);
        fillBarSpriteRenderer.DOKill();
        fillBarSpriteRenderer.color = Color.red;
        fillBarSpriteRenderer.DOColor(Color.yellow, .1f).SetLoops(2, LoopType.Yoyo);


        if (ratio >= 1)
        {
            gameObject.SetActive(false);
        }
        else if (ratio <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        fillBarParent.DOKill();
        fillBarSpriteRenderer.DOKill();
        fillBarWhiteParent.DOKill();
    }
}
