using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip birdSound;
    [SerializeField] private Bird bird; 
    [SerializeField] private GameObject player;
    [SerializeField] private AudioManager audioManager;
    private bool playerInside = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInside = true;

            if (birdSound != null)
            {
                audioManager.PlaySFX(birdSound);
            }

            if (!bird.IsAttackingOrReturning())
            {
                bird.StartAttack();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        if (bird.AttackFinished() && playerInside)
        {
            bird.RespawnPlayer(player);
        }
    }
}
