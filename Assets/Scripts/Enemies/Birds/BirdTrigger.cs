using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip birdSound;
    [SerializeField] private Bird bird;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioManager audioManager;

    private bool playerInside = false;
    private bool hasRespawned = false;
    private Collider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInside = true;
            hasRespawned = false;

            if (birdSound != null)
            {
                audioManager.PlaySFX(birdSound);
            }

            if (!bird.IsAttacking())
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
        if (playerInside && !hasRespawned && bird.IsAttacking() && IsBirdInsideTrigger())
        {
            hasRespawned = true;
            StartCoroutine(bird.RespawnPlayer(player));
        }
    }

    private bool IsBirdInsideTrigger()
    {
        return triggerCollider.bounds.Contains(bird.transform.position);
    }
}
