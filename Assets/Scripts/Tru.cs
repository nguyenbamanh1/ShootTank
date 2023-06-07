using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tru : MonoBehaviour
{
    public GameObject truUI;

    public void onNextMap()
    {
        StartCoroutine(MapManager.gI().loadMap(MapManager.gI().mapId + 1));
    }

    public void onHeath()
    {
        int price = (int)((((MapManager.player.tank.maxHp - MapManager.player.tank.hp) / MapManager.player.tank.maxHp) * 100f) * 5);
        Action ok = () =>
        {
            MapManager.player.tank.hp = MapManager.player.tank.maxHp;
            MapManager.player.gold -= price;
            AnimManager.gI().headth(MapManager.player.transform.position);
        };
        Main.main.chatPopup.createChatPopup($"Bạn có muốn sửa chữa xe tank của bạn với giá {price} đồng không?", ok, null);
    }

    public bool isActive()
    {
        return truUI.active;
    }

    public void nextMap()
    {
        StartCoroutine(MapManager.gI().loadMap(MapManager.gI().mapId + 1));
    }


    public void onUpgrade()
    {
        Action ok = () =>
        {
            Debug.Log("OK");
        };
        Main.main.chatPopup.createChatPopup("Chức năng đang hoàn thiện?", ok, null);
    }
}
