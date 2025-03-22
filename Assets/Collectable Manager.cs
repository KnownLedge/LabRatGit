using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableManager : MonoBehaviour
{
    public List<GameObject> collectables;
    private bool[,] bools = new bool[3,10];
    private int sceneIndex;
    public static CollectableManager instance;

    private void Awake()
    {
        instance = this;
        GameObject[] Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        Array.Sort(Collectables,(a,b) => { return a.name.CompareTo(b.name); });
        collectables = Collectables.ToList();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Lab1":
                sceneIndex = 0;
                break; 
            case "Lab2":
                sceneIndex = 1;
                break; 
            case "Lab3":
                sceneIndex = 2;
                break; 
            case "RPS test":
                sceneIndex = 0;
                break;
        }
    }
   

    private void SetBools(int y)
    {
        for (int i = 0; i < collectables.Count; i++) 
        {
            bools[y,i] = collectables[i].GetComponent<Collectable>().GetState();
        }

    }
    



    public bool[] GetBools(int Sca) 
    {
        SetBools(sceneIndex);
        bool[] passthrough = new bool[10];
        for (int i = 0; i < collectables.Count; i++)
        {
            passthrough[i] = bools[Sca, i];
        }
        return passthrough;
    }
    
}
