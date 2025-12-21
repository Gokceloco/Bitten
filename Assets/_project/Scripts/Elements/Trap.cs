using DG.Tweening;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Saw saw;

    private void Start()
    {
        saw.transform.DOLocalMoveX(2, Random.Range(1,3)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        saw.transform.DOLocalRotate(transform.right * 90, .2f)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        saw.transform.DOKill();
    }
}
