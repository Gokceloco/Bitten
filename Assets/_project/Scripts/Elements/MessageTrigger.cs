using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public float duration;
    public string msg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().gameDirector.uIManager.messageUI.ShowMessage(msg, duration, 0);
            gameObject.SetActive(false);
        }
    }
}
