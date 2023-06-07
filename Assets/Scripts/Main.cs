using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Joystick joystick;

    public Button btnShoot;

    public Button btnChangeTarget;

    public ChatPopup chatPopup;

    public Bullet bulletPrefab;

    public static int gameTick;

    public static Main main;

    public static TrackItem[] tracks;

    public static BulletItem[] bullets;

    public static Texture2D targetImg;

    public static Texture2D arrowPlayer;

    public GameObject bulletShoot, tankShoot, destroyEff;

    public GameObject spawnStarted, arrowPlayerObj, newPlayer;

    public GameObject loadingScr;

    public PhysicMaterial physic;

    public GameObject flyText;

    private int FPS;

    private float timeleft;

    private float accum;

    private int frames;

    private float updateInterval = 0.5f;

    public static bool loadOk;

    private void Awake()
    {
        main = this;
        tracks = Resources.LoadAll<TrackItem>("Tracks");
        Debug.Log(tracks.Length);

        targetImg = Resources.Load<Texture2D>("WhiteBox");

        bullets = Resources.LoadAll<BulletItem>("Bullets");
        
        arrowPlayer = Resources.Load<Texture2D>("WhiteArrow");

        Resources.UnloadUnusedAssets();

        GC.Collect();

        Application.targetFrameRate = 144;
    }

    public void init()
    {
        spawnStarted = GameObject.Find("SpawnStarted");
    }

    public void init2()
    {
        btnShoot.onClick.AddListener(() =>
        {
            MapManager.player.tank.OnShooting();
        });

        btnChangeTarget.onClick.AddListener(() => {
            MapManager.player.changeTarget();
        });
    }

    void Start()
    {
        loadOk = true;

        Application.targetFrameRate = 144;


        if (Player.dataPlayer != null && Player.dataPlayer.ContainsKey("mapid"))
        {
            StartCoroutine(MapManager.gI().loadMap(Player.dataPlayer.Value<int>("mapid")));
        }
        else
        {
            StartCoroutine(MapManager.gI().loadMap(1));
        }
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        // Nếu hết thời gian cập nhật kết quả thì tính toán FPS và hiển thị lên màn hình
        if (timeleft <= 0.0f)
        {
            FPS = (int)(accum / frames);

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
    GUIStyle style;
    private void OnGUI()
    {
        
        if(style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.red;
        }
        
        GUI.Label(new Rect(0, 0, 100, 30), "FPS: " + FPS, style);
    }

    private void FixedUpdate()
    {
        gameTick++;
        if (gameTick > 100000) gameTick = 0;
    }

    public void loading(bool a)
    {
        loadingScr.SetActive(a);
    }
}
