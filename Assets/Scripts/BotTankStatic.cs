using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTankStatic : BotTank
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        this.init();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

    }
    public void FixedUpdate()
    {
        if (this.target != null && Vector3.Distance(this.target.transform.position, this.transform.position) <= this.bulletItem.range)
        {
            if (lastTimeShoot <= 0)
            {
                if (Vector3.Distance(this.target.transform.position, this.transform.position) <= this.bulletItem.range)
                {
                    this.OnShooting();
                }
                lastTimeShoot = 1.5f;
            }
            else
            {
                lastTimeShoot -= Time.fixedDeltaTime;
            }
        }
        
    }

    public void OnDrawGizmos()
    {
        if(bulletItem != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, bulletItem.range);
        }
        //Physics2D.OverlapCircle(this.transform.position, bulletItem.range);
    }
}
