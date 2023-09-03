using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Troop : MonoBehaviour
{
    [SerializeField] private float InitTime;
    [SerializeField] private Troop Target;
    [SerializeField] private float SphereRadius;

    [SerializeField] private bool CanMove;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float MaxMoveSpeed;

    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackDistance;

    [SerializeField] private Collider[] col;
    [SerializeField] private List<float> list;

    [SerializeField] private float enemydist;

    public float Health;
    public float MaxHealth;

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        Health = MaxHealth;
        //deploy time
        yield return new WaitForSeconds(InitTime);
        //add yourself to active enemies
        GameManager.Instance.SpawnedEnemies.Add(this);
        Target = LocateEnemy();
        CanMove = true;
    }

    /// <summary>
    /// return the troop that is closest to the troop scanning
    /// </summary>
    /// <returns></returns>
    private Troop LocateEnemy()
    {
        print(gameObject.name + " is locating enemy");
        col = Physics.OverlapSphere(transform.position, SphereRadius);

        list = new List<float>();

        for (int i = 0; i < col.Length; i++)
        {
            //adds enemy distance to list
            if (col[i].GetInstanceID() != GetComponent<Collider>().GetInstanceID())
            {
                list.Add(EnemyDistance(col[i]));
            }
        }
        int index = list.IndexOf(list.Min());
        
        enemydist = list[index];
        
        //returns the troop that is closest to you
        return col[index].GetComponent<Troop>();
    }

    private float EnemyDistance(Collider troop)
    {
        float distance = 0;
        if (troop.GetInstanceID() != GetComponent<Collider>().GetInstanceID())
        {
            distance = Vector3.Distance(transform.position, troop.transform.position);
        }
        return distance;
    }

    /// <summary>
    /// WIP, gets called when withing a cetrain distance depending on the troop (OVERRIDE PER TROOP VARIANT)
    /// </summary>
    /// <param name="troop"></param>
    /// <returns></returns>
    public virtual IEnumerator Attack(Troop troop)
    {
        if (troop.Health <= 0)
        {
            
        }
        else 
        {
            yield return new WaitForSeconds(0.1f);
            print("attacking");
        }
    }

    private void MoveTroop()
    {
        //move pos to nearest enemy
        if (Target != null && !HasArrivedAtTarget())
        {
            transform.LookAt(Target.transform);
            transform.position = Vector3.Lerp(transform.position, Target.transform.position, Time.deltaTime * MoveSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            //put this in an if statement with an enum (IMPORTANT!!!)

            LocateEnemy();
            if (HasArrivedAtTarget() && Target.Health > 0)
            {
                StartCoroutine(Attack(Target));
            }
            MoveTroop();
        }
       
    }

    /// <summary>
    /// return true or false based on if the troop is within attacking range
    /// </summary>
    /// <returns></returns>
    private bool HasArrivedAtTarget()
    {
        //replace magic number with attackdistance when done testing
        if (enemydist <= AttackDistance)
        {
            return true;
        }
        else
            return false;
    }

    private void Death()
    {
        if (Health >= 0)
        {
            //play death Animation
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.SpawnedEnemies.Remove(this);
    }

    //This class needs to pathfind, Attack, Move and die
}
