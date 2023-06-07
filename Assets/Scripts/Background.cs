using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [Range(-1, 1)]
    public float speed = 0.5f;
    private float offets;
    Material mat;
    public Camera cam;
    private Transform _transform;

    Vector2 screenResolution;

    private void Awake()
    {
        _transform = this.transform;
        cam = (cam != null) ? cam : Camera.main;
        screenResolution = new Vector2(Screen.width, Screen.height);
    }


    void Start()
    {
        mat = GetComponent<Image>().material;
    }

    private void FixedUpdate()
    {
    }


    // Update is called once per frame
    void Update()
    {
        offets += (Time.deltaTime * speed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offets, 0));
    }
}
