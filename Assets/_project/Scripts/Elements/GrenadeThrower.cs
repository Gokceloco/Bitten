using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public Grenade greadePrefab;
    public LayerMask grenadeClickLayerMask;

    public float grenadeTimeMultiplier;
    public float rotationSpeed;

    public Transform leftHandTransform;

    public PlayerMovement playerMovement;

    public float grenadeThrowCooldown;
    private float _lastGrenadeThrowTime;

    private void Update()
    {
        print(Time.time - _lastGrenadeThrowTime);
        if (Input.GetMouseButtonDown(1) && Time.time - _lastGrenadeThrowTime > grenadeThrowCooldown)
        {
            StartCoroutine(ThrowGreande());
        }
    }

    IEnumerator ThrowGreande()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        playerMovement.ChangeAnimationState("ThrowGrenade");
        playerMovement.SetUpperBodyLayerWeightTo1();

        _lastGrenadeThrowTime = Time.time;

        playerMovement.isThrowingGrenade = true;

        if (Physics.Raycast(ray, out var hit, 50, grenadeClickLayerMask))
        {
            var distance = (hit.point - transform.position).magnitude;

            yield return new WaitForSeconds(.15f);

            playerMovement.isThrowingGrenade = false;

            yield return new WaitForSeconds(.15f);

            var newGrenade = Instantiate(greadePrefab);
            newGrenade.transform.rotation = transform.rotation;

            newGrenade.transform.DOLocalRotate(newGrenade.transform.right * rotationSpeed,
                distance * grenadeTimeMultiplier, RotateMode.LocalAxisAdd);

            newGrenade.transform.position = leftHandTransform.position;
            newGrenade.transform.DOMoveX(hit.point.x, distance * grenadeTimeMultiplier).SetEase(Ease.Linear);
            newGrenade.transform.DOMoveZ(hit.point.z, distance * grenadeTimeMultiplier).SetEase(Ease.Linear);
            newGrenade.transform.DOMoveY(transform.position.y + 1.5f, distance * grenadeTimeMultiplier * .5f)
                .SetEase(Ease.OutQuad);
            newGrenade.transform.DOMoveY(hit.point.y, distance * grenadeTimeMultiplier * .5f)
                .SetDelay(distance * grenadeTimeMultiplier * .5f).SetEase(Ease.InQuad);
            

            yield return new WaitForSeconds(.3f);

            playerMovement.SetUpperBodyLayerWeightTo0();
            
        }        
    }
}
