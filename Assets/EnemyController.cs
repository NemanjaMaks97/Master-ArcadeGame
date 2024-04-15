using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyBase EnemyObject;
    private bool dead = false;

    private void FixedUpdate()
    {
        EnemyObject.Shoot();
        EnemyObject.Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "bullet_t1")
        {
            EnemyObject.HP -= 1;
        }
        if (collision.collider.gameObject.tag == "bullet_t2")
        {
            EnemyObject.HP -= 2;
        }
        if (collision.collider.gameObject.tag == "bullet_t3")
        {
            EnemyObject.HP -= 3;
        }
        if (EnemyObject.HP <= 0 && !dead)
        {
            dead = true;
            EnemyObject.DropLoot();
            Game.InvokeEnemyDestroyed(this, (int)EnemyObject.Tier);
            Destroy(this.gameObject);
        }
    }
}
