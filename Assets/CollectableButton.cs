using UnityEngine;
using UnityEngine.UI;

public class CollectableButton : MonoBehaviour
{
    public Text textBox;
    private string ItemDescripton = "cheese";
    public static CollectableButton Instance;
    private Sprite Spriteoff, Spriteon;
    public bool IsUnlocked;

    private void Awake()
    {
        Instance = this;   
        textBox = GameObject.Find("Panel/CollectFile/Text").GetComponent<Text>();
    }

    public void DisplayDescripton() {
        textBox.text = getState() ? ItemDescripton : "Entry not Found";
    }

    public void IStart(string Descrption, Sprite on, Sprite off)
    {
        ItemDescripton = Descrption;
        Spriteoff = off;
        Spriteon = on;
    }
    private void Update()
    {
        gameObject.GetComponent<Image>().sprite = getState() ? Spriteon : Spriteoff;

    }
    public bool getState() {
        return IsUnlocked;
    } 
    public void  SetTrue() {
        IsUnlocked = !IsUnlocked;
    }
}
