using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectData : MonoBehaviour
{
    [SerializeField]
    private CollectableData Data;
    private void Start()
    {
        Collectable Collect = GetComponent<Collectable>();

        Data.CollectableDescription = Collect.itemDescription;

    }

    public void Collected()
    {
        Collectable Collect = GetComponent<Collectable>();
        Data.Collected = Collect.Iscollected;
    }

}
