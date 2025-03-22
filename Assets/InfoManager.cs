using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    private GameObject panel;
    public List<Button> buttons;
    private List<string> ItemDesPathWay = new List<string>
    {
        "Assets/Resources/UI/ItemDescription1.txt",
        "Assets/Resources/UI/ItemDescription2.txt",
        "Assets/Resources/UI/ItemDescription3.txt",
    };

    [SerializeField]
    private int[] CollectCount;
    private bool IsActive;
    private int pageIndex;
    private int pageCount = 2;
    [SerializeField]
    private Text LabText;

    [Header("Lab 1 Sprite")]
    [SerializeField]
    private Sprite[] Unlocked1;
    [SerializeField]
    private Sprite[] Locked1;
    [Header("Lab 2 Sprite")]
    [SerializeField]
    private Sprite[] Unlocked2;
    [SerializeField]
    private Sprite[] Locked2;
    [Header("Lab 3 Sprite")]
    [SerializeField]
    private Sprite[] Unlocked3;
    [SerializeField]
    private Sprite[] Locked3;

    private Sprite[,] Unlocked = new Sprite[3,10];
    private Sprite[,] Locked = new Sprite[3, 10];

    private bool[] bs;

    private void Awake()
    {
        panel = GameObject.Find("Canvas/Panel");
        panelSetDeActive();
        SetSprite();
    }
   
    void SetSprite()
    {
        for (int i = 0; i < CollectCount[0]; i++) 
        {
            Unlocked[0,i] = Unlocked1[i];
            Locked[0,i] = Locked1[i];
        } 
        for (int i = 0; i < CollectCount[1]; i++) 
        {
            Unlocked[1,i] = Unlocked2[i];
            Locked[1,i] = Locked2[i];
        }   
        for (int i = 0; i < CollectCount[2]; i++) 
        {
            Unlocked[2,i] = Unlocked3[i];
            Locked[2,i] = Locked3[i];
        }
    }
    private void Update()
    {
        //Activate Panel
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (IsActive == false)
            {
                panelSetActive();
            }
            else 
            {
                panelSetDeActive();
            }
        }
    }

    void panelSetActive()
    {
        panel.SetActive(true);
        IsActive = true;
        LabText.text = ($"Lab {pageIndex + 1}");
        CreateButtons(0);

    }
    void panelSetDeActive()
    {
        panel.SetActive(false);
        ClearList();
        IsActive = false;
    }
    public void ButtonIncrease()
    {
        IncreasePage();
    }

    public void ButtonDecrease()
    {
        DecreasePage();
    }

    private int IncreasePage()
    {
        pageIndex++;
        if (pageIndex > pageCount)
            pageIndex = 0;
        CreateButtons(pageIndex);
        LabText.text = ($"Lab {pageIndex + 1}");
        return pageIndex;
    }

    private int DecreasePage()
    {
        pageIndex--;
        if (pageIndex < 0)
            pageIndex = pageCount;
        CreateButtons(pageIndex);
        LabText.text = ($"Lab {pageIndex + 1}");
        return pageIndex;
    }


    #region Collectable buttons list
    private void CreateButtons(int SCA)
    {
        GameObject UIButtonsParent = GameObject.Find("Panel/CollectList/Grid");
        Button ButtonPrefab = Resources.Load<Button>(@"UI/Button");
        string[] Descriptline = File.ReadAllLines(ItemDesPathWay[pageIndex]);
        bs = CollectableManager.instance.GetBools(SCA);
        

        if (buttons != null)
        {
            ClearList();
            for (int i = 0; i < CollectCount[SCA]; i++)
            {
                buttons.Add(Instantiate(ButtonPrefab, UIButtonsParent.transform));
                buttons[i].GetComponent<CollectableButton>().IStart(Descriptline[i], Unlocked[pageIndex, i], Locked[pageIndex, i], bs[i]);
            }
        }

        else
        {
            for (int i = 0; i < CollectCount[SCA]; i++)
            {
                buttons.Add(Instantiate(ButtonPrefab, UIButtonsParent.transform));
                buttons[i].GetComponent<CollectableButton>().IStart(Descriptline[i], Unlocked[pageIndex,i], Locked[pageIndex,i], bs[i]);
            }
        }
    }

    private void ClearList()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
    }

    #endregion

}
