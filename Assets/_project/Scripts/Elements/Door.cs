using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftWing;
    public Transform rightWing;

    private bool _isDoorOpen;
    private bool _isPlayerInRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isPlayerInRange)
        {
            if (GetComponentInParent<Level>().IsLevelCleared())
            {
                if (_isDoorOpen)
                {
                    CloseDoor();
                }
                else
                {
                    OpenDoor();
                }
            }
            else
            {
                GetComponentInParent<Level>().ShowClearLevelMsg();
            }            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }
    public void OpenDoor()
    {
        _isDoorOpen = true;
        leftWing.DOLocalMoveZ(1.4f, 1f);
        rightWing.DOLocalMoveZ(-2.7f, 1f);
    }
    public void CloseDoor()
    {
        _isDoorOpen = false;
        leftWing.DOLocalMoveZ(0, 1f);
        rightWing.DOLocalMoveZ(-1.43f, 1f);
    }
}
