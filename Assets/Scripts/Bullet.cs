using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    Rigidbody2D rb2;
    Vector2 startPostion;

    private BulletItem info;

    public bool isPlayer;
    public Tank tankParent;

    private void Awake()
    {
        
    }
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

        init();
    }

    public void init()
    {
        startPostion = transform.position;
    }

    public void setInfo(BulletItem info)
    {
        this.info = info;
        GetComponent<SpriteRenderer>().sprite = info.icon;
    }


    void Update()
    {
        if(Vector2.Distance(transform.position, startPostion) >= info.range)
        {
            rb2.velocity = Vector2.zero;
            Destroy(this.gameObject);
        }
        else
        {
            rb2.velocity = transform.up * info.speed;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Tank tank = null;
        if (this.isPlayer && other.CompareTag("Enemy")){
            tank = other.GetComponent<Tank>();
        }else if(!this.isPlayer && other.CompareTag("Player")){
            tank = other.GetComponent<Tank>();
        }
        if(tank != null)
        {
            if (tank.OnDamage(this.info.damage))
            {
                
                if(this.tankParent != null)
                {
                    tankParent.isResetTower = true;
                    this.tankParent.StartCoroutine(this.tankParent.ResetTower());
                }

                if(tank.tag.Equals("Enemy") && this.tankParent.tag.Equals("Player"))
                {
                    this.tankParent.GetComponent<Player>().playerDiem += ((BotTank)tank).diem;
                    this.tankParent.GetComponent<Player>().gold += ((BotTank)tank).gold;
                }
                Destroy(tank.gameObject);
            }
            Destroy(this.gameObject);
            //Instantiate(Main.main.bulletShoot, this.transform.position, this.transform.rotation, null);
            AnimManager.gI().shoot2(this.transform.position);
        }
        
    }
}
