using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DichChuyen : MonoBehaviour
{

    private GameObject truUI;
    public GameObject child;

    private void Awake()
    {
        child.SetActive(false);
        truUI = GameObject.FindGameObjectWithTag("tru");
        
    }
    private void Start()
    {
        truUI.SetActive(false);
    }

    public void show()
    {
        child.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(child.active == true && collision.CompareTag("Player") && truUI.active == false)
        {
            truUI.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (child.active == true && collision.CompareTag("Player") && truUI.active == false)
        {
            truUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (child.active == true && collision.CompareTag("Player") && truUI.active == false)
        {
            truUI.SetActive(false);
        }
    }
}
