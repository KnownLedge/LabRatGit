using UnityEngine;

public class BirdTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip birdSound;
    [SerializeField] private Bird bird; 
    [SerializeField] private GameObject player;

    private AudioSource audioSource;
    private bool playerInside = false;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInside = true;

            if (birdSound != null)
            {
                audioSource.clip = birdSound;
                audioSource.Play();
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
            if (birdSound != null)
            {
                audioSource.clip = birdSound;
                audioSource.Stop();
            }
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
