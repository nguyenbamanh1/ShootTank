using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Tank))]
public class Player : MonoBehaviour
{
    public string name;

    public Tank tank;

    private Transform arrow;

    public int playerDiem;

    public int gold;

    public static JObject dataPlayer;

    public void Start()
    {
        tank = this.gameObject.GetComponent<Tank>();
        tank.init();
        this.gameObject.tag = "Player";
        arrow = Instantiate(Main.main.arrowPlayerObj, this.transform).transform;
        this.tank.animMove = Main.tracks[0];
        this.tank.bulletItem = Main.bullets[0];
        
        if (dataPlayer != null)
        {
            int trackid = dataPlayer.Value<int>("trackId");
            int bulletId = dataPlayer.Value<int>("bulletId");
            int diem = dataPlayer.Value<int>("diem");
            int gold = dataPlayer.Value<int>("gold");
            float hp = dataPlayer.Value<float>("hp");
            float maxHp = dataPlayer.Value<float>("maxHp");
            
            tank.hp = hp;
            tank.maxHp = maxHp;
            playerDiem = diem;
            this.gold = gold;
            this.tank.animMove = Main.tracks[trackid - 1];
            this.tank.bulletItem = Main.bullets[bulletId];
        }
        else
        {
            Debug.Log("new player");
            tank.hp = 100;
            tank.maxHp = 100;
            playerDiem = 0;
            this.gold = 0;
        }


        
    }


    public void FixedUpdate()
    {

#if UNITY_EDITOR
        tank.OnMoving(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetMouseButton(0))
        {
            tank.OnShooting();
        }
#elif UNITY_ANDROID || UNITY_IPHONE

        tank.OnMoving(new Vector2(Main.main.joystick.Horizontal, Main.main.joystick.Vertical));
#endif
        if(this.tank.target == null || Vector3.Distance(this.tank.target.transform.position, this.transform.position) > this.tank.bulletItem.range)
        {
            changeTarget();
        }

        if(this.tank.target != null)
        {
            Vector3 position = this.transform.position + new Vector3(8, 0, 0);

            Vector3 direction = this.tank.target.transform.position - position;

            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrow.rotation = Quaternion.RotateTowards(arrow.rotation, Quaternion.Euler(0, 0, angle - 90), 200);

            direction = -this.transform.position + this.tank.target.transform.position;

            Vector3 point = this.transform.position + direction.normalized * 3f;


            arrow.position = point;
        }

    }

    public void changeTarget(){
        GameObject[] _obj = GameObject.FindGameObjectsWithTag("Enemy");

        if(_obj != null && _obj.Length != 0)
        {
            GameObject __target = _obj[0];
            
            for(int i = 0; i < _obj.Length; i++)
            {
                if(Vector3.Distance(this.transform.position, __target.transform.position) > Vector3.Distance(this.transform.position, _obj[i].transform.position))
                {
                    __target = _obj[i];
                }
            }
            this.tank.target = __target.GetComponent<Tank>();
            this.timeAnimTarget = 5f;
            this.angel = 90f;
        }
    }
    float timeAnimTarget = 0;
    float angel = 90f;
    private void OnGUI()
    {

        Matrix4x4 matrix = GUI.matrix;
        if (this.tank.target != null)
        {
            matrix = GUI.matrix;

            var vector = Camera.main.WorldToScreenPoint(this.tank.target.transform.position);

            Rect rect = new Rect(vector.x - 10, Screen.height - vector.y - 10, 20, 20);

            if (timeAnimTarget > 0)
            {
                timeAnimTarget -= Time.fixedDeltaTime;
                GUIUtility.RotateAroundPivot(angel, new Vector2(vector.x, Screen.height - vector.y));
                angel += 0.5f;
            }

            Graphics.DrawTexture(rect, Main.targetImg);

            GUI.matrix = matrix;
        }
    }

    public void save()
    {
        JObject job = tank.getData();
        job.Add("diem", playerDiem);
        job.Add("gold", gold);
        job.Add("mapid", MapManager.gI().mapId);
        job.Add("mobs", MapManager.gI().getMobs());
        Rms.saveString("save.bak", job.ToString());
    }

    public static void spawnPlayer(Vector3 position)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Vector3 vector = position;
            if (dataPlayer != null)
            {
                vector = new Vector3(dataPlayer.Value<float>("x"), dataPlayer.Value<float>("y"));
            }
            player = Instantiate(Main.main.newPlayer, vector, Quaternion.Euler(0, 0, 0), null);
            Player pl = player.AddComponent<Player>();
        }
        else
        {
            player.transform.position = position;
        }
        Sound.gI().Teleport();
        MapManager.player = player.GetComponent<Player>();
        player.SetActive(false);
        //Instantiate(MapManager.teleport, player.transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity, null);
        AnimManager.gI().teleport(player.transform.position + new Vector3(0, 2.5f, 0));
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }
}
