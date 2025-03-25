using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] Collectable collectable;
    public static PopupManager instance;
    private bool showPopup = false;
    private string popupMessage = "";
    private Sprite popupSprite = null;
    private bool isPaused = false;

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

    void Update()
    {
        if (showPopup && (Input.GetKeyDown(KeyCode.E)))
        {
            ClosePopup();
        }
    }

    

    public void ShowPopup(string message, Sprite image)
    {
        popupMessage = message;
        popupSprite = image;
        showPopup = true;
        isPaused = true;
        Time.timeScale = 0; 
    }

    public void ClosePopup()
    {
        showPopup = false;
        popupMessage = "";
        popupSprite = null;
        isPaused = false;
        Time.timeScale = 1;
        Destroy(collectable.gameObject);
    }

    void OnGUI()
    {
        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 28;
        customStyle.alignment = TextAnchor.MiddleCenter;
        customStyle.normal.textColor = Color.white;
        customStyle.hover.textColor = Color.white;

        if (showPopup)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float imageWidth = screenWidth * 0.6f; 
            float imageHeight = imageWidth * (9f / 16f); 
            float imageX = (screenWidth - imageWidth) / 2;
            float imageY = (screenHeight - imageHeight) / 2;

            if (popupSprite != null)
            {
                GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), popupSprite.texture);
            }

            float textWidth = imageWidth * 0.8f;
            float textHeight = imageHeight * 0.4f;
            float textX = imageX + (imageWidth - textWidth) / 2;
            float textY = imageY + (imageHeight - textHeight) / 2;

            GUI.Label(new Rect(textX, textY, textWidth, textHeight), popupMessage, customStyle);

            textY = textY + textHeight * 1.3f;

            GUI.Label(new Rect(textX, textY, textWidth, textHeight), "Press E to close", customStyle);

        }
    }
}
