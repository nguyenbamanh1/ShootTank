using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new bullet", menuName ="Tank/Bullet", order =0)]
public class BulletItem : ScriptableObject
{
    
    public Sprite icon;

    public float speed;

    public float shootReload;

    public float damage;

    public float range;

    public int price;
    
}
