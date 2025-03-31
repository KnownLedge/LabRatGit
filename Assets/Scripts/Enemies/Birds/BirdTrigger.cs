using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    [SerializeField] private Bird bird; 
    [SerializeField] private GameObject player;

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInside = true;

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
