using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ChatPopup : MonoBehaviour
{
    public TMP_Text chat;
    public Button[] buttons;

    private Action ok, cancel;

    private void Start()
    {
        
        this.gameObject.SetActive(false);
    }

    public void createChatPopup(string text, Action ok, Action cancel)
    {
        Main.main.chatPopup.gameObject.SetActive(true);
        
        chat.text = text.ToUpperInvariant();

        if(ok != null)
        {
            buttons[0].gameObject.SetActive(true);
        }
        else
        {
            buttons[0].gameObject.SetActive(false);
        }
        if (cancel != null)
        {
            buttons[1].gameObject.SetActive(true);
        }
        else
        {
            buttons[1].gameObject.SetActive(false);
        }
        this.ok = ok;
        this.cancel = cancel;
    }

    public void btnOk()
    {
        this.ok();
        this.gameObject.SetActive(false);
    }

    public void btnCancel()
    {
        this.cancel();
        this.gameObject.SetActive(false);
    }

    public bool isActive() => this.gameObject.active;
}
