using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshPro damageTMP;
    public void StartFloatingText(int damage)
    {
        damageTMP.text = damage.ToString();
        transform.DOMoveY(transform.position.y + 2, .4f);
        transform.DOScale(0, .2f).SetDelay(.4f).SetEase(Ease.InBack);
        transform.DOMoveX(transform.position.x + Random.Range(-1f,1f), .4f);
        transform.DOMoveZ(transform.position.z + Random.Range(-1f,1f), .4f);
    }
}
