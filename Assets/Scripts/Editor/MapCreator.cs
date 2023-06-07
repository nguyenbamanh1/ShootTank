using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : Editor
{

    [MenuItem("GameObject/Create Map", false, 10)]
    public static void createMap()
    {
        GameObject map = new GameObject("Map");
        map.layer = 6;
        map.tag = "map";
        GameObject grid = new GameObject("Grid");
        grid.AddComponent<Grid>();
        grid.transform.parent = map.transform;

        GameObject layer1 = new GameObject("layer1");
        layer1.AddComponent<Tilemap>();
        var tileRender = layer1.AddComponent<TilemapRenderer>();
        var tileCol = layer1.AddComponent<TilemapCollider2D>();
        tileCol.isTrigger = true;
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 0;
        layer1.transform.parent = grid.transform;

        GameObject layer2 = new GameObject("layer2");
        layer2.AddComponent<Tilemap>();
        tileRender = layer2.AddComponent<TilemapRenderer>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 1;
        layer2.transform.parent = grid.transform;

        GameObject water = new GameObject("water");
        water.AddComponent<Tilemap>();
        tileRender = water.AddComponent<TilemapRenderer>();
        water.AddComponent<TilemapCollider2D>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 2;
        water.transform.parent = grid.transform;

        GameObject itembg = new GameObject("ItemBg");
        itembg.AddComponent<Tilemap>();
        tileRender = itembg.AddComponent<TilemapRenderer>();
        itembg.AddComponent<TilemapCollider2D>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 3;
        itembg.transform.parent = grid.transform;

        GameObject _itembg = new GameObject("LayerNoPhysic");
        _itembg.AddComponent<Tilemap>();
        tileRender = _itembg.AddComponent<TilemapRenderer>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 3;
        _itembg.transform.parent = itembg.transform;

        _itembg = new GameObject("ItemBg_layer1");
        _itembg.AddComponent<Tilemap>();
        _itembg.AddComponent<TilemapCollider2D>();
        tileRender = _itembg.AddComponent<TilemapRenderer>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 4;
        _itembg.transform.parent = itembg.transform;

        _itembg = new GameObject("ItemBg_layer2");
        _itembg.AddComponent<Tilemap>();
        _itembg.AddComponent<TilemapCollider2D>();
        tileRender = _itembg.AddComponent<TilemapRenderer>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 5;
        _itembg.transform.parent = itembg.transform;

        _itembg = new GameObject("ItemBg_layer3");
        _itembg.AddComponent<Tilemap>();
        tileRender = _itembg.AddComponent<TilemapRenderer>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 6;
        _itembg.transform.parent = itembg.transform;

        GameObject tree = new GameObject("tree");
        tree.AddComponent<Tilemap>();
        tileRender = tree.AddComponent<TilemapRenderer>();
        tree.AddComponent<TilemapCollider2D>();
        tileRender.sortingLayerName = "map";
        tileRender.sortingOrder = 4;
        tree.transform.parent = grid.transform;

    }
}
