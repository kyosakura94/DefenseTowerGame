using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCharacter : ScriptableObject
{
    [Header("General")]
    public GameObject mesh;
    public GameObject prefab;
    public string towername;
    public float rangeRadius = 15f;
    public float turnSpeed = 10f;
    public GameObject upgradeEffect;
    public Material upGradeMat;

    [Header("Use for Shoot")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    [Header("Use Laser")]

    public GameObject lazerEffect;
    public float damageOverTime = 30;
    public float slowAmount = .5f;

    [Header("Use Barrier")]
    public float totaltime = 300f;

}
