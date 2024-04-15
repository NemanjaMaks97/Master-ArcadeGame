using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT2 : EnemyBase
{
    public EnemyT2()
    {
        HP = 3 + Game.GameDificulty * 2;
    }

    public override void Move()
    {
        if (onPoint)
        {
            float minX = LeftEdge.x;
            float maxX = RightEdge.x;
            float minY = RightEdge.y;
            float maxY = LeftEdge.y;

            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            currentCheckpoint.x = x;
            currentCheckpoint.y = y;
            currentCheckpoint.z = -5;

            onPoint = false;
            readyToMove = false;
        }
        if (this.transform.position.x != currentCheckpoint.x && this.transform.position.y != currentCheckpoint.y)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentCheckpoint, 0.04f);
        }
        else
        {
            if (readyToMove)
            {
                onPoint = true;
                readyToMove = false;
                inPause = false;
            }
            else
            {
                if (!inPause)
                {
                    StartCoroutine(StopMovement());
                }
            }

        }
    }

    public override void DropLoot()
    {
        float chance = Random.Range(0f, 1f);
        if (chance <= (0.2 + Game.GameDificulty * 0.1))
        {
            SpawnDrop();
        }
    }
    public override void Shoot()
    {
        if (!waitingForShot)
        {
            waitingForShot = true;
            int time = Random.Range(3000, 4000);
            StartCoroutine(ShootBullet(time));
        }
    }

    protected override IEnumerator ShootBullet(int time)
    {
        yield return new WaitForSeconds(time / 1500);
        GameObject bullet = GameObject.Instantiate(Bullet);
        bullet.GetComponent<BulletMovement>().BulletAngle = 180;
        bullet.transform.position = this.gameObject.transform.position + new Vector3(0, -0.2f, -2); ;
        bullet.SetActive(true);
        waitingForShot = false;
    }
}
