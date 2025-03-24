using System.Collections.Generic;
using UnityEngine;

public class CollectableOverlay : MonoBehaviour
{
    private List<Sprite> collectableImages = new List<Sprite>();
    private List<bool> collectedStatus = new List<bool>();

    void Start()
    {
        LoadCollectables();
    }

    void LoadCollectables()
    {
        collectableImages.Clear();
        collectedStatus.Clear();

        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (GameObject collectable in collectables)
        {
            Collectable collectableScript = collectable.GetComponent<Collectable>();
            if (collectableScript != null && collectableScript.itemImage != null)
            {
                collectableImages.Add(collectableScript.itemImage);
                collectedStatus.Add(false);
            }
        }
    }

    public int GetIndexOfItem(Sprite itemSprite)
    {
        return collectableImages.IndexOf(itemSprite);
    }


    public void MarkCollected(int index)
    {
        if (index >= 0 && index < collectedStatus.Count)
        {
            collectedStatus[index] = true;
        }
    }

    void OnGUI()
    {
        float iconSize = 100f;
        float spacing = 10f;
        float startX = (Screen.width - (collectableImages.Count * (iconSize + spacing))) / 2;
        float startY = Screen.height - iconSize - 20;

        for (int i = 0; i < collectableImages.Count; i++)
        {
            Rect iconRect = new Rect(startX + i * (iconSize + spacing), startY, iconSize, iconSize);

            if (!collectedStatus[i])
            {
                GUI.color = new Color(0, 0, 0, 0.8f); // Darken uncollected items
            }
            GUI.DrawTexture(iconRect, collectableImages[i].texture, ScaleMode.ScaleToFit, true);
            GUI.color = Color.white; // Reset color
        }
    }
}
