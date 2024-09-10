using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public String itemname;
    public String type;
    public String flavortext;
    public String description;
    public Sprite sprite;
    public int stacks;

    public bool found;
}
