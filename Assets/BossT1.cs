using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossT1 : EnemyBase
{

    public BossT1()
    {
        HP = 20 + Game.GameDificulty * 20;
    }

    public override void Move()
    {
        if (onPoint)
        {
            if(this.transform.position.x != LeftEdge.x)
            {
                currentCheckpoint.x = LeftEdge.x;
                currentCheckpoint.y = this.transform.position.y;
                currentCheckpoint.z = -5;
            }
            else
            {
                currentCheckpoint.x = RightEdge.x;
                currentCheckpoint.y = this.transform.position.y;
                currentCheckpoint.z = -5;
            }

            onPoint = false;
            readyToMove = false;
        }
        if (this.transform.position.x != currentCheckpoint.x)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentCheckpoint, 0.03f);
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
        SpawnDrop();
    }

    public override void Shoot()
    {
        if (!waitingForShot)
        {
            waitingForShot = true;
            int time = Random.Range(1000, 3000);
            StartCoroutine(ShootBullet(time));
        }
    }

    protected override IEnumerator ShootBullet(int time)
    {
        yield return new WaitForSeconds(time / 1000);

        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = GameObject.Instantiate(Bullet);
            bullet.GetComponent<BulletMovement>().BulletAngle = 180;
            float x = i % 2 == 0 ? 1f : -1f;
            bullet.transform.position = this.gameObject.transform.position + new Vector3(x, 0.2f, -2);
            bullet.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        waitingForShot = false;
    }
}
