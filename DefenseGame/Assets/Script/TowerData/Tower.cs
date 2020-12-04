using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData tower;


    [SerializeField]
    private Enemy targetEnemy;
    private Transform target;


    [Header("Upgrade")]
    public int kill;
    public bool isUpgraded = false;

    private GameObject mesh;
    private Transform firePoint;


    private float fireCountdown = 0f;

    //[Header("Map")]
    //[SerializeField]
    //private GameObject endPoint;

    
    private bool useLaser = false;
    private LineRenderer lineRenderer;

    private GameObject test;
    private ParticleSystem impactEffect;
    private Light impactLight;


    public List<GameObject> activeEnemies;


    private void Awake()
    {
        switch (tower.towerType)
        {
            case Types.TowerType.Shoot:

                break;
            case Types.TowerType.Lazer:

                lineRenderer = transform.GetComponent<LineRenderer>();
                test = Instantiate(tower.lazerEffect, transform.position, transform.rotation);
                test.transform.parent = transform;

                impactEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
                impactLight = transform.GetChild(1).GetChild(1).GetComponent<Light>();
                useLaser = true;

                break;
            case Types.TowerType.Barrier:
                break;
            default:
                break;
        }

        mesh = Instantiate(tower.mesh, transform.position, transform.rotation);
        mesh.transform.parent = transform;

        firePoint = transform.GetChild(0);

    }


    void Start()
    {
        kill = 0;
        InvokeRepeating("UpdateEnemy", 0f, 0.5f);
    }

    void UpdateEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject shootEnemy = null;

        float shortestDistance = Mathf.Infinity;
        switch (tower.enemySelect)
        {
            case Types.EnemySelect.NearTower:

                foreach (GameObject enemy in enemies)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= tower.rangeRadius)
                    {
                        enemy.GetComponent<Enemy>().isInRange = true;
                    }
                    else
                    {
                        enemy.GetComponent<Enemy>().isInRange = false;
                    }
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        shootEnemy = enemy;
                    }
                }

                break;
            case Types.EnemySelect.NearEndPoint:

                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= tower.rangeRadius)
                    {
                        enemy.GetComponent<Enemy>().isInRange = true;
                    }
                    else
                    {
                        enemy.GetComponent<Enemy>().isInRange = false;
                    }

                    if (enemy.GetComponent<Enemy>().isInRange == true)
                    {
                        activeEnemies.Add(enemy);
                    }

                    float closesttoEndpoint = enemy.GetComponent<Enemy>().remainDistance;

                    float towertoEnemynear = Vector3.Distance(transform.position, enemy.transform.position);

                    if (towertoEnemynear <= tower.rangeRadius)
                    {
                        if (closesttoEndpoint < shortestDistance)
                        {
                            shortestDistance = closesttoEndpoint;
                            shootEnemy = enemy;
                        }
                    }
                }

                break;
            default:
                break;
        }
        

        if (shootEnemy != null )
        {
            target = shootEnemy.transform;
            targetEnemy = shootEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }


    }

    void Update()
    {

        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }


        LockOnTarget();

        switch (tower.towerType)
        {
            case Types.TowerType.Shoot:

                if (fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / tower.fireRate;
                }

                fireCountdown -= Time.deltaTime;

                break;
            case Types.TowerType.Lazer:

                Laser();

                break;
            case Types.TowerType.Barrier:

                Barrier();

                break;

            default:
                break;
        }
       
        if (target.GetComponent<Enemy>().currentHealth <= 0)
        {
            kill++;

            Debug.Log(kill);
        }

        if (kill >= 6 && isUpgraded == false)
        {
            towerUpgrate();
            isUpgraded = true;
        }

        fireCountdown -= Time.deltaTime;
    }

    void towerUpgrate()
    {
        GameObject effectIns = (GameObject)Instantiate(tower.upgradeEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        gameObject.GetComponentInChildren<Renderer>().material = tower.upGradeMat;

        tower.bulletPrefab.GetComponent<Bullet>().damage = 50;

        //Destroy(gameObject);
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);


        Vector3 rotation = Quaternion.Lerp(mesh.transform.rotation, lookRotation, Time.deltaTime * tower.turnSpeed).eulerAngles;
        mesh.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(tower.bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
    }
    void Barrier()
    {
        //targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].GetComponent<Enemy>().runStop();
        }

        StartCoroutine(TimeBased());
    }

    IEnumerator TimeBased()
    {
        while (tower.totaltime >= 0)
        {
            tower.totaltime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        //Destroy(child.gameObject);


        GameObject effectIns = (GameObject)Instantiate(tower.upgradeEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);
    }

    void Laser()
    {
        targetEnemy.TakeDamage(tower.damageOverTime * Time.deltaTime);
        targetEnemy.Slow(tower.slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tower.rangeRadius);
    }

}
