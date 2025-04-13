using UnityEngine;
using UnityEngine.UI;

public class CollectableButton : MonoBehaviour
{
    public Text textBox;
    private string ItemDescripton = "cheese";
    public static CollectableButton Instance;
    private Sprite Spriteoff, Spriteon;
    public Image ChildIm;
    public bool IsUnlocked;

    private void Awake()
    {
        Instance = this;   
        textBox = GameObject.Find("CollectableUIHud/CollectableUIPanel/CollectFile/Text").GetComponent<Text>();
    }

    public void DisplayDescripton() {
        textBox.text = IsUnlocked ? ItemDescripton : "Entry not Found";
    }

    public void IStart(CollectableData Data)
    {
        IsUnlocked = Data.Collected;
        ItemDescripton = Data.CollectableDescription;
        Spriteoff = Data.Locked;
        Spriteon = Data.Unlocked;
        ChildIm.sprite = IsUnlocked ? Spriteon : Spriteoff;
    }
   
}
