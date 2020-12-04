using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenu(fileName = "NewTower", menuName = "Tower")]
public class TowerData : TowerCharacter
{
    public TowerType towerType;
    public EnemySelect enemySelect;
}
