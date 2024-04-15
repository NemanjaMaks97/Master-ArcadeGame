using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT3 : EnemyBase
{

    public EnemyT3()
    {
        HP = 5 + Game.GameDificulty * 3;
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
        float chance = Random.Range(0f, 1f);
        if (chance <= (0.3 + Game.GameDificulty * 0.1))
        {
            SpawnDrop();
        }
    }

    public override void Shoot()
    {
        if (!waitingForShot)
        {
            waitingForShot = true;
            int time = Random.Range(2000, 4000);
            StartCoroutine(ShootBullet(time));
        }
    }

    protected override IEnumerator ShootBullet(int time)
    {
        yield return new WaitForSeconds(time / 1000);

        for (int i = 0; i < 4; i++)
        {
            GameObject bullet = GameObject.Instantiate(Bullet);
            bullet.GetComponent<BulletMovement>().BulletAngle = 180;
            float x = i % 2 == 0 ? 0.4f : -0.4f;
            bullet.transform.position = this.gameObject.transform.position + new Vector3(x, -0.2f, -2);
            bullet.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        waitingForShot = false;
    }
}
