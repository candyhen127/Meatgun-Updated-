using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStats", order = 3)]
public class PlayerStats: ScriptableObject
{
    public String troopername;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public float maxHealth;
    public float moveSpeed;
    public String ability;
    public GameObject abilityprefab;
    public float cooldown;
    public String abilitydescription;

    public bool locked;
    public int cost;
    public Sprite displaysprite;
}

