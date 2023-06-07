using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Tank : MonoBehaviour, IObjectMap
{

    protected Rigidbody2D rb2;

    protected float speedMove = 55f;

    protected float rotateSpeed = 250f;

    public Tank target;

    [Header("Animation")]
    public TrackItem animMove;

    public SpriteRenderer[] tracks;

    [Header("Control")]
    [SerializeField]
    private List<GameObject> nongSung;

    public List<GameObject> towers;


    public BulletItem bulletItem;

    public Vector2 movement;

    [Header("Heath")]
    public float hp = 100f;
    public float maxHp = 100f;


    public float def = 0f;

    
    public void Start()
    {

    }

    public void init()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.gravityScale = 0;
        towers = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name.Equals("Body"))
            {
                tracks = child.GetComponentsInChildren<SpriteRenderer>();
            }
            else if (child.name.Equals("Tower"))
            {
                towers.Add(child);
            }
        }
        nongSung = new List<GameObject>();
        foreach (GameObject tower in towers)
        {
            for (int i = 0; i < tower.transform.childCount; i++)
            {
                GameObject shootGun = tower.transform.GetChild(i).gameObject;
                if (shootGun.name.Equals("GunShoot"))
                {
                    nongSung.Add(shootGun);
                }
            }
        }


        if (bulletItem == null)
        {
            bulletItem = Main.bullets[0];
        }
    }

    
    public void OnMoving(Vector2 position)
    {

        if (!MapManager.isloading)
        {
            this.movement = position;

            this.AnimationTrack();

            Quaternion rotation = Quaternion.identity;

            if (position.x != 0 || position.y != 0)
            {
                rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -Mathf.Atan2(position.x, position.y) * Mathf.Rad2Deg), rotateSpeed * Time.deltaTime);
                rb2.MoveRotation(rotation);
            }

            var _speed = speedMove;
            Vector2 newPostion = rb2.position + position * 0.1f * _speed * Time.fixedDeltaTime;
            if (CameraFollow.instance.tilemap != null && CameraFollow.instance.tilemap.GetComponent<TilemapCollider2D>().OverlapPoint(newPostion))
            {
                rb2.MovePosition(newPostion);
            }

            if (target == null && !isResetTower)
            {
                //StartCoroutine(ResetTower());
            }
        }
    }

    int indexTrack = 0;
    int animCount = 5;
    private void AnimationTrack()
    {
        if (Main.gameTick % 2 == 0 && animCount > 0)
        {
            animCount--;
        }

        if (animCount == 0)
        {
            animCount = 5;
            indexTrack++;
            if (indexTrack >= tracks.Length)
            {
                indexTrack %= tracks.Length;
            }
            if (movement.y != 0 || movement.x != 0)
            {
                tracks[0].sprite = animMove.animSprite[indexTrack];
                tracks[1].sprite = animMove.animSprite[indexTrack];
            }
        }
    }

    public void Update()
    {
        if(countReload > 0)
        {
            countReload -= Time.deltaTime;
        }

        if (this.target != null && Vector3.Distance(this.target.transform.position, this.transform.position) <= this.bulletItem.range)
        {
            OnMoveTower(this.target.transform.position);
            this.isResetTower = false;
        }
    }

    float countReload = 0;
    public void OnShooting()
    {
        if (countReload <= 0)
        {
            countReload = bulletItem.shootReload;
            Sound.gI().Shoot();
            foreach (var s in nongSung)
            {
                var _bullet = Instantiate(Main.main.bulletPrefab, s.transform.position, s.transform.rotation, null);
                _bullet.tankParent = this;
                _bullet.setInfo(bulletItem);
                _bullet.isPlayer = this.tag.Equals("Player");

                Instantiate(Main.main.tankShoot, s.transform.position, s.transform.rotation, null);
            }
        }
    }

    public void OnMoveTower(Vector3 vector)
    {
        foreach(GameObject tower in towers)
        {
            if (Vector3.Distance(this.transform.position, vector) <= bulletItem.range)
            {
                var _distance = vector - tower.transform.position;

                var angel = Mathf.Atan2(_distance.y, _distance.x) * Mathf.Rad2Deg;

                tower.transform.rotation = Quaternion.RotateTowards(tower.transform.rotation, Quaternion.Euler(0, 0, angel - 90), rotateSpeed * Time.deltaTime);
            }
        }
    }
    public bool isResetTower;
    public IEnumerator ResetTower()
    {

        isResetTower = true;
        //tower.transform.rotation = Quaternion.Slerp(tower.transform.rotation, saveTowerRotation, rotateSpeed * Time.fixedDeltaTime);
        foreach(GameObject tower in towers)
        {
            while (Quaternion.Angle(tower.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0))) != 0 && isResetTower)
            {
                tower.transform.localRotation = Quaternion.RotateTowards(tower.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, -0)), 200f * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        isResetTower = false;
    }

    public void OnDrawGizmos() {

#if UNITY_EDITOR
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, bulletItem.range);

        if (this.tag.Equals("Player") && target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.target.transform.position, this.transform.position);
        }
#endif

    }

    public virtual bool OnDamage(float dame)
    {
        dame -= def;

        this.hp -= dame;
        TextMesh textMesh = Main.main.flyText.GetComponentInChildren<TextMesh>();
        textMesh.text = "-" + dame;
        textMesh.color = Color.red;
        Instantiate(Main.main.flyText, this.transform.position + new Vector3(0, 2), Quaternion.identity, null);
        if (this.hp <= 0)
        {
            this.hp = 0;
            AnimManager.gI().shoot2(this.transform.position);
            if (this.tag.Equals("Player"))
            {
                Main.main.chatPopup.createChatPopup("Bạn đã chết!", () =>
                {
                    Application.LoadLevel(0);
                }, () =>
                {
                    Application.LoadLevel(0);
                });
            }

            return true;
        }

        return false;
    }

    public JObject getData()
    {
        JObject job = new JObject();
        job.Add("name", this.gameObject.name.Replace("(Clone)", ""));
        job.Add("maxHp", maxHp);
        job.Add("hp", hp);
        job.Add("bulletId", int.Parse(bulletItem.name));
        if(animMove != null)
        {
            job.Add("trackId", int.Parse(animMove.name));
        }
        job.Add("x", transform.position.x);
        job.Add("y", transform.position.y);
        
        return job;
    }
    
    public void applyData(JObject job)
    {
        this.transform.position = new Vector3(job.Value<float>("x"), job.Value<float>("y"));
        this.maxHp = job.Value<float>("maxHp");
        this.hp = job.Value<float>("hp");
        this.bulletItem = Main.bullets[job.Value<int>("bulletId")];
        if (job.ContainsKey("trackId"))
        {
            this.animMove = Main.tracks[job.Value<int>("trackId") - 1];
        }
    }

}
