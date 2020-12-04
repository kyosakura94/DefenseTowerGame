using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public BuildManagerData data;

    public TowerBluePrint towerToBuild;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    public void SelectTowerToBuild(TowerBluePrint tower)
    {
        //tower.prefab.GetComponent<Tower>().
        towerToBuild = tower;
    }

    public TowerBluePrint GetTowerToBuild()
    {
        return towerToBuild;
    }

    private void Update()
    {
        switch (data.ending)
        {
            case Types.Ending.Firsttoflag:
                
                break;
            case Types.Ending.lastunitstanding:

                data.timeofLevel -= Time.deltaTime;
                if (data.timeofLevel < 0)
                {
                    Debug.Log("Firsttoflag");
                }
                break;
            default:
                break;
        }
    }
}
