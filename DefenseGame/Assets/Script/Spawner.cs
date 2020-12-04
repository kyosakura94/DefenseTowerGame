using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject tower;
    BuildManager buildManager;
    public int towerCount; 

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (buildManager.towerToBuild == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.tag == ("MovePoint"))
                {
                    if (towerCount <= buildManager.data.towerNumber)
                    {
                        BuildTurret(buildManager.GetTowerToBuild(), hit.collider.gameObject.transform.position);
                    }
                    else
                    {
                        Debug.Log("You use all the tower!!");
                    }

                    //Instantiate<GameObject>(tower, (hit.collider.gameObject.transform.position), Quaternion.identity);
                    
                }
            }
        }
    }

    void BuildTurret(TowerBluePrint blueprint, Vector3 position)
    {

        GameObject _tower = (GameObject)Instantiate(blueprint.prefab, position, Quaternion.identity);
        tower = _tower;
        Debug.Log("Tower build!");
        towerCount++;
    }



}
