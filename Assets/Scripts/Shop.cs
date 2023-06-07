using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ItemShop itemShopTemp;
    private void Awake()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }

    public void openShopBullet()
    {
        this.transform.parent.parent.gameObject.SetActive(true);
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        
        BulletItem[] bulletItems = Resources.LoadAll<BulletItem>("Bullets");
        foreach (BulletItem item in bulletItems)
        {
            var itemShop = Instantiate(itemShopTemp, this.transform);
            itemShop.icon.sprite = item.icon;
            itemShop.btn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                MapManager.player.tank.bulletItem = item;
                MapManager.player.gold -= item.price;
            }));
            itemShop.cap.text = item.price + "$";
        }
        
    }
}
