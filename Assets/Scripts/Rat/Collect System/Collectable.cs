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
    public bool Iscollected = false;
    private GameObject player;
    private ArenaTestEnter arenaTestEnterScript;


    void Start()
    {
        player = GameObject.FindWithTag("Player");

        GameObject arenaObject = GameObject.Find("ArenaTestEnter");
        if (arenaObject != null)
        {
            arenaTestEnterScript = arenaObject.GetComponent<ArenaTestEnter>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CollectItem();
            }
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
        //PLay effect when collect
        if(collectEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effectInstance.gameObject, effectInstance.main.duration);
        }

        PopupManager.instance.ShowPopup(itemDescription, itemBackground);
        InventoryItem newItem = new InventoryItem(itemName, itemDescription, itemImage);
        InventoryManager.instance.AddItem(newItem);

        CollectableOverlay overlay = FindObjectOfType<CollectableOverlay>();
        if (overlay != null)
        {
            int index = overlay.GetIndexOfItem(itemImage); // Find correct index
            overlay.MarkCollected(index); // Mark this as collected
        }

        Destroy(gameObject, collectSound.length);

        if (arenaTestEnterScript != null && itemName != null && itemName == arenaTestEnterScript.requiredCollectableName)
        {
            arenaTestEnterScript.OnCollectableCollected();
        }
    }

    public bool GetState()
    {
        return Iscollected;
    }

}

