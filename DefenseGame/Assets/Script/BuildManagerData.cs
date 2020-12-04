using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenu(fileName = "Ending", menuName = "Ending")]
public class BuildManagerData : ScriptableObject
{
    public Ending ending;
    public float towerNumber;
    public float timeofLevel = 100;
    public string endingName;
}
