using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public float speed;

    public float currentHealth;

    public bool isDead = false;
    public bool isAttack = false;
    public bool isInRange = false;

    [SerializeField]
    public EnemyData enemy;

    [Header("Path Holder")]

    public Transform pathHolder;
    public int wavepointIndex = 0;
    public Transform target;

    public float totaldistance;
    public float remainDistance;
    public float distanceTravelled = 0;
    public Vector3 lastPosition;

    BuildManager buildManager;

    // Start is called before the first frame update

    void Start()
    {
        buildManager = BuildManager.instance;
        switch (enemy.enemyMovement)
        {
            case Types.EnemyMovement.Wanderer:
                Debug.Log("Wanderer Enemy");
                break;
            case Types.EnemyMovement.Fast:
                Debug.Log("Fast Enemy");
                break;
            case Types.EnemyMovement.Slow:
                Debug.Log("Slow Enemy");
                break;
            default:
                break;
        }

        currentHealth = enemy.totalHealth;

        target = PathHolder.points[0];
        lastPosition = transform.position;

        for (int i = 0; i < PathHolder.points.Length; i++)
        {
            int k = i + 1;

            if (k > 5)
            {
                k = 5;
            }
            else
            {
                float pos = Vector3.Distance(PathHolder.points[i].position, PathHolder.points[k].position);
                totaldistance += pos;
            }
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy Die");
            Die();
        }

        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        remainDistance = totaldistance - distanceTravelled;

        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
            StartCoroutine(TurnToFace(target.position));
        }

        if (isAttack)
        {
            StartCoroutine(RunFaster(2f));
        }
        else
        {
            speed = enemy.startSpeed;
        }
        
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= PathHolder.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = PathHolder.points[wavepointIndex];
    }



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void Die()
    {
        isDead = true;
        GameObject effectIns = (GameObject)Instantiate(enemy.dieEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);
        Destroy(gameObject);
    }

    public void Slow(float pct)
    {
        speed = enemy.startSpeed * (1f - pct);
    }

    public void runStop()
    {
        speed = 0;
    }


    IEnumerator RunFaster(float pct)
    {
        while (isInRange)
        {
            speed = enemy.startSpeed * (1f + pct);
            yield return null;
        }

        isAttack = false;
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, enemy.turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void EndPath()
    {
        switch (buildManager.data.ending)
        {
            case Types.Ending.Firsttoflag:

                Debug.Log("GameOver!");
                Time.timeScale = 0;
                break;
            case Types.Ending.lastunitstanding:

                GameObject[] towerlist = GameObject.FindGameObjectsWithTag("Tower");

                Debug.Log(towerlist.Length);
                int i = Random.Range(0, towerlist.Length);

                if (towerlist.Length >= 1)
                {
                    if (towerlist[i] != null)
                    {
                        Destroy(towerlist[i]);
                    }
                }
                else
                {
                    Debug.Log("All Tower Is Destroy");
                    Debug.Log("YOU LOSE!!");
                }
                

                if (buildManager.data.timeofLevel <= 0)
                {
                    if (towerlist.Length >= 1)
                    {
                        Debug.Log("YOU WIN!!");
                    }
                    else
                    {
                        Debug.Log("YOU LOSE!!");
                    }
                }

                Destroy(gameObject);

                break;
            default:
                break;
        }



        

    }
}
