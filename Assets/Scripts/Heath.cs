using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Heath : MonoBehaviour
{

    [SerializeField]
    private Image heathBar;

    [SerializeField]
    private TMP_Text heathText;
    [SerializeField]
    private TMP_Text coinText;
    [SerializeField]
    private TMP_Text goldText;
    private Player player;

    // Update is called once per frame
    void Update()
    {

        if (player == null)
        {
            GameObject _plObj = GameObject.FindGameObjectWithTag("Player");
            if (_plObj != null)
            {
                player = _plObj.GetComponent<Player>();
            }
            return;
        }

        heathBar.fillAmount = player.tank.hp / player.tank.maxHp;

        heathText.text = player.tank.hp + " / " + player.tank.maxHp;

        coinText.text = "Điểm: " + player.playerDiem;

        goldText.text = "Gold: " + player.gold;
    }
}
