using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT1 : EnemyBase
{
    public EnemyT1()
    {
        HP = 1 + Game.GameDificulty * 1;
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
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentCheckpoint, 0.06f);
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
        if (chance <= (0.1 + Game.GameDificulty * 0.1))
        {
            SpawnDrop();
        }
    }

    protected override IEnumerator ShootBullet(int time)
    {
        yield return new WaitForSeconds(time / 1000);
        GameObject bullet = GameObject.Instantiate(Bullet);
        bullet.GetComponent<BulletMovement>().BulletAngle = 180;
        bullet.transform.position = this.gameObject.transform.position + new Vector3(0, -0.2f, -2); ;
        bullet.SetActive(true);
        waitingForShot = false;
    }

}
