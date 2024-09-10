using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Gun", order = 2)]
public class Gun: ScriptableObject
{
    public String gunname;
    public String realgunname;
    public String description;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public float damage;
    public int ammo;
    public float reloadTime;
    public int projectiles;
    public float fireRate;
    public float destroy;
    public bool front = true;
    public GameObject bulletPrefab;
    public bool locked;
    public int cost;
    public Sprite displaysprite;
}

