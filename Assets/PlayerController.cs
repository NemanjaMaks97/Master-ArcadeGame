using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int HP = 8 - 2 * Game.GameDificulty;
    private readonly int MaxHP = 8 - 2 * Game.GameDificulty;
    private bool shieldActive = false;

    public GameObject Player;
    public GameObject Bulet_t1;
    public GameObject Bulet_t2;
    public GameObject Bulet_t3;
    public GameObject Shield;

    public int PlayerStrength = 1;
    public int GunSize = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void UpgradeWeapon()
    {
        PlayerStrength++;

        if(PlayerStrength > 3)
        {
            if(GunSize < 3)
            {
                PlayerStrength = 1;
                GunSize++;
            }
            else
            {
                PlayerStrength = 3;
            }
        }
    }

    private void DowngradeWeapon()
    {
        PlayerStrength--;

        if (PlayerStrength < 1)
        {
            if (GunSize > 1)
            {
                GunSize--;
                PlayerStrength = 3;
            }
            else
            {
                PlayerStrength = 1;
            }
        }
    }

    private void EnableShield(bool enable)
    {
        Shield.SetActive(enable);
        shieldActive = enable;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            this.GetComponent<AudioSource>().volume = Game.EffectVolume;
            this.GetComponent<AudioSource>().Play();
            GameObject currentBullet;
            switch (PlayerStrength)
            {
                case 1:
                    currentBullet = Bulet_t1;
                    break;
                case 2:
                    currentBullet = Bulet_t2;
                    break;
                case 3:
                    currentBullet = Bulet_t3;
                    break;
                default:
                    currentBullet = Bulet_t1;
                    break;
            }

            GameObject bulet1;
            GameObject bulet2;
            GameObject bulet3;

            switch (GunSize)
            {
                case 1:
                    bulet1 = GameObject.Instantiate(currentBullet);
                    bulet1.transform.position = Player.transform.position + new Vector3(0, 0.3f, -1);
                    bulet1.SetActive(true);
                    break;
                case 2:
                    bulet2 = GameObject.Instantiate(currentBullet);
                    bulet3 = GameObject.Instantiate(currentBullet);
                    bulet2.transform.position = Player.transform.position + new Vector3(0.3f, 0.3f, -1);
                    bulet2.SetActive(true);
                    bulet3.transform.position = Player.transform.position + new Vector3(-0.3f, 0.3f, -1);
                    bulet3.SetActive(true);
                    break;
                case 3:
                    bulet1 = GameObject.Instantiate(currentBullet);
                    bulet2 = GameObject.Instantiate(currentBullet);
                    bulet3 = GameObject.Instantiate(currentBullet);
                    bulet1.transform.position = Player.transform.position + new Vector3(0, 0.3f, -1);
                    bulet1.SetActive(true);
                    bulet2.transform.position = Player.transform.position + new Vector3(0.3f, 0.3f, -1);
                    bulet2.SetActive(true);
                    bulet3.transform.position = Player.transform.position + new Vector3(-0.3f, 0.3f, -1);
                    bulet3.SetActive(true);
                    break;
                default:
                    bulet1 = GameObject.Instantiate(currentBullet);
                    bulet1.transform.position = Player.transform.position + new Vector3(0, 0.3f, -1);
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        //if (Input.GetKey("w"))
        //{
        //    Player.transform.position = Player.transform.position + new Vector3(0, 0.01f, 0);
        //}
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            if(Player.transform.position.x < -9)
            {
                return;
            }
            Player.transform.position = Player.transform.position + new Vector3(-0.1f, 0, 0);
        }
        //if (Input.GetKey("s"))
        //{
        //    Player.transform.position = Player.transform.position + new Vector3(0, -0.01f, 0);
        //}
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            if (Player.transform.position.x > 9)
            {
                return;
            }
            Player.transform.position = Player.transform.position + new Vector3(0.1f, 0, 0);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag.Contains("enemy") && !shieldActive)
        {
            int dmg = 0;
            if (collision.collider.gameObject.tag == "enemy_bullet_t1")
            {
                dmg += 1;
            }
            if (collision.collider.gameObject.tag == "enemy_bullet_t2")
            {
                dmg += 2;
            }
            if (collision.collider.gameObject.tag == "enemy_bullet_t3")
            {
                dmg += 3;
            }
            DowngradeWeapon();
            Game.InvokePlayerHit(this, dmg);
            HP -= dmg;

            if (HP <= 0)
            {
                Game.InvokePlayerDestroyed(this);
                Destroy(this.gameObject);
            }
            else
            {
                EnableShield(true);
                StartCoroutine(IFrameTimer());
            }
        }

        if (collision.collider.gameObject.tag.Contains("drop"))
        {
            if (collision.collider.gameObject.tag == "drop_hp")
            {
                if(HP < MaxHP)
                {
                    HP += 1;

                    Game.InvokePlayerHeal(this);
                }
            }
            if (collision.collider.gameObject.tag == "drop_shield" && !shieldActive)
            {
                EnableShield(true);
                StartCoroutine(ShieldTimer());
            }
            if (collision.collider.gameObject.tag == "drop_weapon")
            {
                UpgradeWeapon();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("enemy") && !shieldActive)
        {
            int dmg = 0;
            if (collision.gameObject.tag == "enemy_bullet_t1")
            {
                dmg += 1;
            }
            if (collision.gameObject.tag == "enemy_bullet_t2")
            {
                dmg += 2;
            }
            if (collision.gameObject.tag == "enemy_bullet_t3")
            {
                dmg += 3;
            }
            DowngradeWeapon();
            Game.InvokePlayerHit(this, dmg);
            HP -= dmg;

            if (HP <= 0)
            {
                Game.InvokePlayerDestroyed(this);
                Destroy(this.gameObject);
            }
            else
            {
                EnableShield(true);
                StartCoroutine(IFrameTimer());
            }
        }
    }

    private IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(10);
        EnableShield(false);
    }

    private IEnumerator IFrameTimer()
    {
        yield return new WaitForSeconds(1);
        EnableShield(false);
    }

}
