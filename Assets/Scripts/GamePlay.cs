using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GamePlay : MonoBehaviour
{
    public Button btnContinue;
    public Animator animBg;
    private void Start()
    {
        if(Application.loadedLevel == 0)
        {
            if (Rms.loadString("save.bak") != string.Empty)
            {
                btnContinue.gameObject.SetActive(true);
            }
            else
            {
                btnContinue.gameObject.SetActive(false);
            }
        }

        Application.targetFrameRate = 144;
    }


    public void newGame()
    {
        Application.LoadLevel(1);
    }

    public void ContinueGame()
    {
        string a = Rms.loadString("save.bak");
        Debug.Log(a);
        //JObject job = JObject.Parse(a);
        //PlayerPrefs.SetInt("trackId", job.Value<int>("trackId"));
        //PlayerPrefs.SetInt("bulletId", job.Value<int>("bulletId"));
        //PlayerPrefs.SetInt("diem", job.Value<int>("diem"));
        //PlayerPrefs.SetInt("gold", job.Value<int>("gold"));
        //PlayerPrefs.SetFloat("hp", job.Value<float>("hp"));
        //PlayerPrefs.SetFloat("maxHp", job.Value<float>("maxHp"));
        //PlayerPrefs.SetFloat("x", job.Value<float>("x"));
        //PlayerPrefs.SetFloat("y", job.Value<float>("y"));
        //PlayerPrefs.SetInt("mapid", job.Value<int>("mapid"));

        Player.dataPlayer = JObject.Parse(a);

        Application.LoadLevel(1);
    }

    public void MenuMiniOpen()
    {
        animBg.SetBool("isOpen", !animBg.GetBool("isOpen"));
    }


    public void UnPause()
    {

    }


    public void Setting()
    {

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        
#else
Application.Quit();
#endif

    }

    public void save()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.GetComponent<Player>().save();
        }
    }
}
