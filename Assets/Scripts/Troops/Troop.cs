using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Troop : MonoBehaviour
{
    public float InitTime;
    public Troop Target;
    public float SphereRadius;

    public float Health;
    public float MaxHealth;

    public bool CanMove;
    public float MoveSpeed;
    public float MaxMoveSpeed;

    public float AttackSpeed;
    public float AttackDistance;

    private void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        Health = MaxHealth;
        //deploy time
        yield return new WaitForSeconds(InitTime);
        //add yourself to active enemies
        GameManager.Instance.SpawnedEnemies.Add(this);
        CanMove = true;
    }

    public Troop NearestEnemy()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, SphereRadius);
        List<float> list = new List<float>();

        for (int i = 0; i < col.Length; i++)
        {
            //adds enemy distance to list
            list.Add(EnemyDistance(col[i]));
        }
        int index = list.IndexOf(list.Min());

        return col[index].GetComponent<Troop>();
    }

    public float EnemyDistance(Troop troop)
    {
        return Vector3.Distance(transform.position, troop.transform.position);
    }

    public float EnemyDistance(Collider troop)
    {
        return Vector3.Distance(transform.position, troop.transform.position);
    }

    public virtual IEnumerator Attack(Troop troop)
    {
        yield return null;
    }

    public void Walk(Troop nearestEnemy, float Movespeed)
    {
        Vector3 troopLocation = nearestEnemy.transform.position;
        //move pos to nearest enemy
        transform.position = (troopLocation * Time.deltaTime) * Movespeed;
    }

    private void FixedUpdate()
    {
        //Death();

        if (CanMove && !(EnemyDistance(NearestEnemy()) >= AttackDistance) && !Target)
        {
            Walk(NearestEnemy(), MoveSpeed);
        }
        else if (CanMove && EnemyDistance(NearestEnemy()) >= AttackDistance && !Target)
        {
            StartCoroutine(Attack(Target));
        }
    }

    public void Death()
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
