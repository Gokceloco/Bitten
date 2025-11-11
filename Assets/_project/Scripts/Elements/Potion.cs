using DG.Tweening;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(transform.position.y + .5f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
