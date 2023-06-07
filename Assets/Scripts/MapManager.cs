using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;

    public int mapId = -1;

    public static List<BotTank> vMob = new List<BotTank>();

    public static GameObject teleport;

    public static Player player;

    public GameObject currMap;
    public static bool isloading;
    private void Awake()
    {
        instance = this;
        teleport = Resources.Load<GameObject>("Prefabs/Teleport");
        currMap = GameObject.FindGameObjectWithTag("map");
    }
    public static MapManager gI()
    {
        return instance;
    }

    public IEnumerator loadMap(int id)
    {
        isloading = true;
        Main.main.loading(true);
        vMob.Clear();
        mapId = id;
        yield return null;
        if (currMap != null)
        {

            Destroy(currMap.gameObject);
        }
        Debug.Log("load map prefab");

        GameObject game = Resources.Load<GameObject>("Prefabs/Maps/Map " + id);

        currMap = game = Instantiate(game, game.transform.position, Quaternion.identity, null);
        GameObject @object = GameObject.Find("Mobs");
        if (Player.dataPlayer != null)
        {
            foreach(Transform child in @object.transform)
            {
                Destroy(child.gameObject);
            }

            JArray jar = Player.dataPlayer.Value<JArray>("mobs");
            for(int i =0; i < jar.Count; i++)
            {
                JObject job = (JObject)jar[i];
                string name = job.Value<string>("name");
                GameObject game1 = Resources.Load<GameObject>("Prefabs/" + name);
                game1 = Instantiate(game1, @object.transform);
                BotTank bot = game1.GetComponent<BotTank>();
                if (bot == null)
                {
                    if(game1.name.ToLower().Contains("platform") == false)
                    {
                        bot = game1.AddComponent<BotTank>();
                    }
                    else
                    {
                        bot = game1.AddComponent<BotTankStatic>();
                    }
                    
                }
                bot.applyData(job);
                vMob.Add(bot);
            }
        }
        else
        {
            BotTank[] bots = @object.GetComponentsInChildren<BotTank>();
            vMob.AddRange(bots);
        }
        Debug.Log("mob size => " + vMob.Count);

        Camera.main.GetComponent<CameraFollow>().init();

        GameObject.Find("CameraMiniMap").GetComponent<CameraFollow>().init();

        Main.main.init();

        Player.spawnPlayer(Main.main.spawnStarted.transform.position);

        Main.main.init2();

        Resources.UnloadUnusedAssets();

        Main.main.loading(false);

        isloading = false;
    }

    public JArray getMobs()
    {
        JArray jar = new JArray();
        foreach(BotTank bot in vMob)
        {
            jar.Add(bot.getData());
        }
        return jar;
    }

    private void FixedUpdate()
    {
        if(vMob.Count == 0 && !isloading)
        {
            GameObject gameObject = GameObject.Find("DichChuyen");
            if(gameObject != null)
            {
                DichChuyen tl = gameObject.GetComponent<DichChuyen>();
                if (tl != null)
                {
                    tl.show();
                }
            }
        }
    }
}
