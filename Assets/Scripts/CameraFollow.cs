using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    public Tilemap tilemap; 
    private Vector3 _minBounds;
    private Vector3 _maxBounds;
    Camera _camera;
    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        this._camera = this.GetComponent<Camera>();
    }

    public void init()
    {
        GameObject grid = GameObject.Find("Grid");
        if (grid != null)
        {
            tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
            CalculateBounds();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tilemap != null)
        {
            if (MapManager.player == null)
            {
                MapManager.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
            else
            {
                this.transform.position = new Vector3(MapManager.player.transform.position.x, MapManager.player.transform.position.y, this.transform.position.z);
            }

            var pos = transform.position;
            var x = Mathf.Clamp(pos.x, _minBounds.x + _camera.orthographicSize * _camera.aspect, _maxBounds.x - _camera.orthographicSize * _camera.aspect);
            var y = Mathf.Clamp(pos.y, _minBounds.y + _camera.orthographicSize, _maxBounds.y - _camera.orthographicSize);
            transform.position = new Vector3(x, y, pos.z);
        }
        else
        {
            init();
        }
    }

    private void CalculateBounds()
    {
        var tilemapBounds = tilemap.localBounds;
        var tilemapMin = tilemapBounds.min;
        var tilemapMax = tilemapBounds.max;

        _minBounds = new Vector3(tilemapMin.x, tilemapMin.y, transform.position.z);
        _maxBounds = new Vector3(tilemapMax.x, tilemapMax.y, transform.position.z);
    }
}
