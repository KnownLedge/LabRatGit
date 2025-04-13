using UnityEngine;
using System.Collections;

public class Collect : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    public bool Iscollected = false;


    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        //Play Sound when collect
        if (collectSound != null)
        {
            audioSource.clip = collectSound;
            audioSource.Play();
        }

        Iscollected = true;
        Destroy(gameObject);
    }
}

