using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : BulletMovement
{
    private int frameCnt = 0;
    private bool switchColor = false;
    private bool bombStarted = false;
    public GameObject Shrapnel;
    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        Vector3 currentCheckpoint = Vector3.zero;
        currentCheckpoint.x = this.gameObject.transform.position.x;
        currentCheckpoint.y = -2f;
        currentCheckpoint.z = -2;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, (this.gameObject.transform.eulerAngles.z + 1) % 360);
        frameCnt++;
        if(frameCnt == 30)
        {
            frameCnt = 0;
            switchColor = !switchColor;
            Color color;
            if (switchColor)
            {
                ColorUtility.TryParseHtmlString("#DFFF49", out color);
            }
            else
            {
                ColorUtility.TryParseHtmlString("#FF4AC7", out color);
            }
            
            this.gameObject.GetComponent<SpriteRenderer>().material.color = color;
        }

        if (this.transform.position.y != currentCheckpoint.y)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, currentCheckpoint, 0.05f);
        }
        else
        {
            if(!bombStarted)
            {
                bombStarted = true;
                StartCoroutine(BombTimer());
            }
        }
    }

    private IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(3);
        GameObject explosion = GameObject.Instantiate(Explosion);
        explosion.transform.position = this.transform.position;
        explosion.SetActive(true);
        BombExplode();
        Destroy(this.gameObject);
    }

    private void BombExplode()
    {
        for (int i = 0; i < 360; i+=30)
        {
            GameObject shrapnel = GameObject.Instantiate(Shrapnel);
            shrapnel.GetComponent<BulletMovement>().BulletAngle = i;
            shrapnel.transform.position = this.gameObject.transform.position;
            shrapnel.SetActive(true);
        }
    }
}
