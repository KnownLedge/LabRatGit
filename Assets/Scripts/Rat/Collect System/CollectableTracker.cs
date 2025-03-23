using System.Collections.Generic;
using UnityEngine;

public class CollectableTracker : MonoBehaviour
{
    public static CollectableTracker instance;
    private List<Texture2D> collectableTextures = new List<Texture2D>();
    private List<Texture2D> collectedTextures = new List<Texture2D>();
    private List<string> collectableNames = new List<string>(); // Track collectable names
    private List<Sprite> originalSprites = new List<Sprite>(); // Track original sprites
    private float iconSize = 100f;
    private float spacing = 20f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FindAllCollectables();
    }

    private void FindAllCollectables()
    {
        Collectable[] collectables = FindObjectsOfType<Collectable>();
        foreach (Collectable collectable in collectables)
        {
            RegisterCollectable(collectable.itemName, collectable.itemImage);
        }
    }

    public void RegisterCollectable(string collectableName, Sprite originalImage)
    {
        collectableTextures.Add(CreateSilhouette(originalImage));
        collectedTextures.Add(null); // Placeholder for later updates
        originalSprites.Add(originalImage);
        collectableNames.Add(collectableName);
    }

    public void UpdateCollectable(string collectableName)
    {
        int index = collectableNames.IndexOf(collectableName);
        if (index != -1)
        {
            collectedTextures[index] = SpriteToTexture(originalSprites[index]); // Replace silhouette with original image
        }
    }

    private Texture2D CreateSilhouette(Sprite original)
    {
        Texture2D newTexture = new Texture2D(original.texture.width, original.texture.height, TextureFormat.RGBA32, false);
        Color[] pixels = original.texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.Lerp(pixels[i], Color.gray, 0.8f); // Darken image
        }
        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
    }

    private Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);
        texture.SetPixels(sprite.texture.GetPixels());
        texture.Apply();
        return texture;
    }

    void OnGUI()
    {
        float startX = (Screen.width - (collectableTextures.Count * (iconSize + spacing))) / 2;
        float yPosition = Screen.height - iconSize - spacing;

        for (int i = 0; i < collectableTextures.Count; i++)
        {
            float xPosition = startX + i * (iconSize + spacing);
            Texture2D displayTexture = collectedTextures[i] != null ? collectedTextures[i] : collectableTextures[i];
            GUI.DrawTexture(new Rect(xPosition, yPosition, iconSize, iconSize), displayTexture);
        }
    }
}
