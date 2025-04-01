using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CollectablesData", order = 1)]
public class CollectableData : ScriptableObject
{
    public bool Collected;
    public string CollectableDescription;
    public Sprite Off, On;
}
