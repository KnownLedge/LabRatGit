using UnityEngine;
using UnityEngine.UI;

public class CollectableButton : MonoBehaviour
{
    public Text textBox;
    private string ItemDescripton = "cheese";
    public static CollectableButton Instance;
    private Sprite Spriteoff, Spriteon;
    private Image ChildIm;
    public bool IsUnlocked;


    private void Awake()
    {
        Instance = this;   
        textBox = GameObject.Find("CollectableUIHud/CollectableUIPanel/CollectFile/Text").GetComponent<Text>();
    }

    public void DisplayDescripton() {
        textBox.text = IsUnlocked ? ItemDescripton : "Entry not Found";
    }

    public void IStart(string Descrption, Sprite on, Sprite off, bool state)
    {
        ChildIm = transform.GetChild(0).GetComponent<Image>();
        ItemDescripton = Descrption;
        Spriteoff = off;
        Spriteon = on;
        IsUnlocked = state;
        ChildIm.sprite = IsUnlocked ? Spriteon : Spriteoff;
    }
   
}
