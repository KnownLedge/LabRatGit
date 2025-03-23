using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    [SerializeField] private ParticleSystem collectEffect;
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public Sprite itemBackground;
    public float interactionDistance = 3f;
    private GameObject player;
    private ArenaTestEnter arenaTestEnterScript;
    public bool Iscollected;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }

            if(collectEffect != null)
            {
                ParticleSystem effectInstance = Instantiate(collectEffect, transform.position, Quaternion.identity);
                Destroy(effectInstance.gameObject, effectInstance.main.duration);
            }

            // PopupManager.instance.ShowPopup(itemDescription, itemBackground);
            // InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
            // InventoryManager.instance.AddItem(newItem);
            Destroy(gameObject, collectSound.length);

            if (arenaTestEnterScript != null && itemName != null && itemName == arenaTestEnterScript.requiredCollectableName)
            {
                arenaTestEnterScript.OnCollectableCollected();
            }
        }
    }

    public bool GetState()
    {
        return Iscollected;
    }
}
