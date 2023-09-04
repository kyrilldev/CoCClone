using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopBase : MonoBehaviour
{
    [SerializeField] private float InitTime = 1;
    [SerializeField] private Vector3 TargetPos;
    [SerializeField] private float SphereRadius = 10;

    [SerializeField] private bool CanMove = false;
    [SerializeField] private float MoveSpeed = 0.1f;
    [SerializeField] private float MaxMoveSpeed = 4;

    [SerializeField] private float AttackSpeed = 2;
    [SerializeField] private float AttackDistance = 1.3f;

    [SerializeField] private Collider[] col;
    [SerializeField] private List<float> list;

    //used to not call the function everytime (i think)
    [SerializeField] private float enemydist;
    [SerializeField] private int ID;

    public LayerMask enemy;

    private Coroutine CurrentCoroutine;

    public float Health;
    public float MaxHealth = 5;

    private void Start()
    {
        ID = GetComponent<Collider>().GetInstanceID();
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        Health = MaxHealth;
        //deploy time
        yield return new WaitForSeconds(InitTime);
        //add yourself to active enemies
        GameManager.Instance.SpawnedEnemies.Add(this);
        CanMove = true;
    }

    /// <summary>
    /// return the troop that is closest to the troop scanning
    /// </summary>
    /// <returns></returns>
    private Vector3 LocateEnemy()
    {
        col = Physics.OverlapSphere(transform.position, SphereRadius, enemy);
        list = new List<float>();

        for (int i = 0; i < col.Length; i++)
        {
            list.Add(EnemyDistance(col[i]));
        }

        int index = list.IndexOf(list.Min());

        enemydist = list[index];

        //returns the troop that is closest to you
        return col[index].transform.position;
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
    /// <returns></returns>
    public virtual IEnumerator Attack()
    {
            yield return new WaitForSeconds(0.1f);
            print("attacking");
        CurrentCoroutine = null;
    }

    private void MoveTroop()
    {
        //move pos to nearest enemy
        if (TargetPos != null && !HasArrivedAtTarget())
        {
            transform.LookAt(TargetPos);
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * MoveSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            //put this in an if statement with an enum (IMPORTANT!!!)
            TargetPos = LocateEnemy();
            if (HasArrivedAtTarget() && CurrentCoroutine == null)
            {
                CurrentCoroutine = StartCoroutine(Attack());
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

    private void AssignLayerMasks()
    {
        //this function needs to look at the assigned tag at the start of the game and assign layermask
    }
    //This class needs to pathfind, Attack, Move and die
}
