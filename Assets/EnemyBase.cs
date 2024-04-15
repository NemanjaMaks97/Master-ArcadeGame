using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyTier
    {
        T1 = 1,
        T2 = 2,
        T3 = 3,
        Boss1 = 10,
        Boss2 = 20,
        Boss3 = 30,
    }

    private int hp;

    public int HP {
        get
        {
            return hp;
        } 
        set 
        {
            hp = value;
        }
    }

    public int Strength;
    public EnemyTier Tier;

    public GameObject Bullet;
    public Vector3 LeftEdge = new Vector3(-8f, 4f, 0);
    public Vector3 RightEdge = new Vector3(8f, 0, 0);
    public List<GameObject> Drops;

    protected bool waitingForShot = false;
    protected bool readyToMove = false;
    protected bool inPause = false;
    protected bool onPoint = true;
    protected Vector3 currentCheckpoint;

    public virtual void Move()
    {

    }

    public virtual void DropLoot() 
    {

    }

    public virtual void Shoot()
    {
        if (!waitingForShot)
        {
            waitingForShot = true;
            int time = Random.Range(1000, 2000);
            StartCoroutine(ShootBullet(time));
        }
    }

    protected virtual IEnumerator ShootBullet(int time)
    {
        yield return new WaitForSeconds(time / 1000);
        GameObject bullet = GameObject.Instantiate(Bullet);
        bullet.transform.position = this.gameObject.transform.position + new Vector3(0, -0.2f, -2); ;
        bullet.SetActive(true);
        waitingForShot = false;
    }


    protected void SpawnDrop()
    {
        int pick = Random.Range(0, 3);
        GameObject drop = GameObject.Instantiate(Drops[pick]);
        drop.transform.position = this.transform.position;
        drop.SetActive(true);
    }

    protected IEnumerator StopMovement()
    {
        inPause = true;
        int time = Random.Range(1, 3);
        yield return new WaitForSeconds(time);
        readyToMove = true;
    }

}
