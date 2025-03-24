using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("SaveManager");
                instance = obj.AddComponent<SaveManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private PlayerData savedPlayerData;

    public void SavePlayerState(Transform playerTransform)
    {
        savedPlayerData = new PlayerData
        {
            position = playerTransform.position,
            rotation = playerTransform.rotation
        };
        Debug.Log("✅ Player state saved: " + savedPlayerData.position);
    }

    public void LoadPlayerState(Transform playerTransform)
    {
        if (savedPlayerData != null)
        {
            playerTransform.position = savedPlayerData.position;
            playerTransform.rotation = savedPlayerData.rotation;
            Debug.Log("✅ Player state loaded: " + playerTransform.position);
        }
        else
        {
            Debug.LogWarning("⚠ No saved data found.");
        }
    }
}
