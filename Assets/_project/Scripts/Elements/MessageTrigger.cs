using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public float duration;
    public string msg;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player.gameDirector.levelManager.currentLevelNo 
                < player.gameDirector.levelManager.levelPrefabs.Count)
            {
                player.gameDirector.uIManager.messageUI.ShowMessage(msg, duration, 0);
                gameObject.SetActive(false);
            }            
        }
    }
}
