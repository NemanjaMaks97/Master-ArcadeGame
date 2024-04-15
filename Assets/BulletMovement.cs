using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public enum Direction
    { 
        Up = 1,
        Down = -1,
    }

    protected GameObject Bullet;
    public Direction BulletDirection;
    public int BulletAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        Bullet = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        Bullet.transform.rotation = Quaternion.Euler(0, 0, (float)BulletAngle);
        Bullet.transform.position += Bullet.transform.up * Time.deltaTime * 5f;
        if (Bullet.transform.position.y > 5 || 
            Bullet.transform.position.y < -5 ||
            Bullet.transform.position.y > 10 ||
            Bullet.transform.position.y < -10 ||
            Bullet.transform.position.x < -10 ||
            Bullet.transform.position.x > 10 
            )
        {
            Destroy(Bullet);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(BulletDirection == Direction.Up)
        {
            if(collision.collider.gameObject.tag == "enemy")
                   Destroy(Bullet);
        }
        else
        {
            if (collision.collider.gameObject.tag == "player")
                Destroy(Bullet);
        }
    }

}
