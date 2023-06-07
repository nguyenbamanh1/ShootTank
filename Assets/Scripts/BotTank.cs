using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTank : Tank
{

    public float lastTimeMoving = 1f;

    public float lastTimeShoot = 1f;

    private Vector2 _movement;

    private float lastTimeResetTarget = 5f;

    private bool waitResetTarget = false;

    public int diem;
    public int gold;
    public void Start()
    {
        base.Start();
        this.init();
        randomDirection = Random.insideUnitCircle;
        this.tag = "Enemy";
    }
    
    public void Update()
    {
        base.Update();
        
        //kiểm tra target là null thì tìm kiếm player
        if(this.target == null)
        {
            //tìm player
            GameObject _player = GameObject.FindGameObjectWithTag("Player");
            //nếu player khác null
            if (_player != null)
            {
                //kiểm tra khoảng cách mà nhỏ hơn khoảng cách đạn bắn
                if (Vector3.Distance(_player.transform.position, this.transform.position) <= this.bulletItem.range)
                {
                    //gán cho target = player
                    this.target = _player.GetComponent<Tank>();
                    waitResetTarget = false;
                }
            }

            //nếu target khác null thì kiểm tra khoảng cách lớn hơn khoảng cách đạn thì
        }else if(Vector3.Distance(target.transform.position, this.transform.position) > this.bulletItem.range)
        {
            //kiểm tra nếu như không trong thời gian chờ reset target thì
            if (!waitResetTarget)
            {
                //gán thời gian chờ là 5f
                lastTimeResetTarget = 5f;
                waitResetTarget = true;
                //đặt thành đang chờ reset
            }
            else if(lastTimeResetTarget == 0)//nếu như thời gian chờ hết target là 0 thì
            {
                //đặt lại chế độ chờ reset thành false
                waitResetTarget = false;
                //đặt target thành null
                this.target = null;
            }
            else
            {
                lastTimeResetTarget -= Time.deltaTime;
            }
        }
        
    }
    Vector2 randomDirection;
    public void FixedUpdate() {
        if(lastTimeMoving <= 0){
            lastTimeMoving = 2f;

            _movement = new Vector2(Random.RandomRange(-1f, 1f), Random.RandomRange(-1f, 1f));

            if(target != null && Vector3.Distance(this.target.transform.position, this.transform.position) > this.bulletItem.range){
                _movement = target.transform.position - this.transform.position;
                _movement.Normalize();
            }
           
        }else{
            lastTimeMoving -= Time.fixedDeltaTime;
        }
        
        this.OnMoving(_movement);

        if (this.target != null && Vector3.Distance(this.target.transform.position, this.transform.position) <= this.bulletItem.range)
        {
            //this.OnMoveTower(this.target.transform.position);

            if(lastTimeShoot <= 0){
                this.OnShooting();
                lastTimeShoot = 1.5f;
            }else{
                lastTimeShoot -= Time.fixedDeltaTime;
            }
        }
    }

    public override bool OnDamage(float dame)
    {
        
        bool isDie = base.OnDamage(dame);
        if (isDie)
        {
            MapManager.vMob.Remove(this);
        }
        return isDie;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("map"))
        {
            lastTimeMoving = 0f;
        }
    }
}
