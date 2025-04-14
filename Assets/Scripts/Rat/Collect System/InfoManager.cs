using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    private GameObject panel;
    public List<Button> buttons;
    private int[] CollectCount = new int[3];
    private bool IsActive;
    private int pageIndex;
    private int pageCount = 2;
    [SerializeField]
    private Text LabText; 

    [Header("Lab 1 Collectables")]
    [SerializeField]
    private CollectableData[] Collectables1;
    [Header("Lab 2 Collectables")]
    [SerializeField]
    private CollectableData[] Collectables2;
    [Header("Lab 3 Collectables")]
    [SerializeField]
    private CollectableData[] Collectables3;

    private CollectableData[,] Collectables = new CollectableData[4,10];

    private void Awake()
    {
        panel = GameObject.Find("CollectableUIHud/CollectableUIPanel");
        panelSetDeActive();
        for (int i = 0; i < Collectables1.Count(); i++) 
            Collectables[0, i] = Collectables1[i];        

        for (int i = 0; i < Collectables2.Count(); i++)
            Collectables[1, i] = Collectables2[i];        

        for (int i = 0; i < Collectables3.Count(); i++)
            Collectables[2, i] = Collectables3[i];
        
    }
    private void Start()
    {
        CollectCount[0] = Collectables1.Count();
        CollectCount[1] = Collectables2.Count();
        CollectCount[2] = Collectables3.Count();
    }


    private void Update()
    {
        //Activate Panel
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (IsActive == false)
            {
                panelSetActive();
                Time.timeScale = 0f;
            }
            else 
            {
                panelSetDeActive();
                Time.timeScale = 1.0f;

            }
        }
    }

    void panelSetActive()
    {
        panel.SetActive(true);
        IsActive = true;
        LabText.text = ($"Lab {pageIndex + 1}");
        CreateButtons(0);
        Scrollbar scrollbar = gameObject.GetComponentInChildren<Scrollbar>();
        if(scrollbar.value == 1)
            scrollbar.onValueChanged.Invoke(0.9998f);
        else
            scrollbar.onValueChanged.Invoke(1);

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
        GameObject UIButtonsParent = GameObject.Find("CollectableUIPanel/CollectList/Grid");
        Button ButtonPrefab = Resources.Load<Button>(@"UI/Button");
        
        if (buttons != null)
        {
            ClearList();
            for (int i = 0; i < CollectCount[SCA]; i++)
            {
                buttons.Add(Instantiate(ButtonPrefab, UIButtonsParent.transform));
                buttons[i].GetComponent<CollectableButton>().IStart(Collectables[SCA,i]);
            }
        }

        else
        {
            for (int i = 0; i < CollectCount[SCA]; i++)
            {
                buttons.Add(Instantiate(ButtonPrefab, UIButtonsParent.transform));
                buttons[i].GetComponent<CollectableButton>().IStart(Collectables[SCA, i]);
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
