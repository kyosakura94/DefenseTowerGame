using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : ScriptableObject
{

    [Header("Enemy Set Up")]
    public string enemyName;
    public float startSpeed = 0.5f;
    public float totalHealth = 100;
    public float turnSpeed = 90;

    public GameObject dieEffect;
    public GameObject prefab;
    public Mesh mesh;
    public Color color;
    public Material material;

}
