using IGL_Tech.RPGM.Auto_Tile_Importer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class B_Tile : EditorWindow
{
    /// <summary>
    /// Scroll position
    /// </summary>
    static Vector2 scrollPosition = Vector2.zero;

    /// <summary>
    /// Loaded Image from file
    /// </summary>
    static protected Texture2D img = null;

    /// <summary>
    /// List of the sub pieces from the tile set
    /// </summary>
    static protected List<Texture2D> sub_blocks;

    /// <summary>
    /// List of boolean to select the block to import
    /// </summary>
    static protected List<bool> sub_blocks_to_import;

    static protected List<string> sub_block_names;

    /// <summary>
    /// The path of the loaded image
    /// </summary>
    static protected string path;

    static protected int wBlock = 48, hBlock = 48;
    static protected int mini_tile_w = 16, mini_tile_h = 16;
    static int w, h;
    //[MenuItem("Tools/RPGM Importer/A2/A2 FULL Layout")]
    //public static void ShowWindow()
    //{
    //    EditorWindow.GetWindow<A2_Tiles_Importer_Window>(false, "A2 Full Layout Impoter");
    //}

    public static void Cut_Layout(string path_to_slice)
    {
        path = path_to_slice;
        img = new Texture2D(1, 1);
        byte[] bytedata = File.ReadAllBytes(path);
        img.LoadImage(bytedata);

        //sub block slicing
        sub_blocks = B_Tile_Slice_File(img, out wBlock, out hBlock);
        sub_blocks_to_import = new List<bool>();
        sub_block_names = new List<string>();
        for (int i = 0; i < sub_blocks.Count; i++)
        {
            sub_blocks_to_import.Add(false);
            sub_block_names.Add(string.Format("_T_{0}_{1}", Path.GetFileNameWithoutExtension(path), i));
        }
    }

    private static List<Texture2D> B_Tile_Slice_File(Texture2D img, out int wBlock, out int hBlock)
    {
        wBlock = 48;
        hBlock = 48;
        w = img.width / wBlock;
        h = img.height / hBlock;
        List<Texture2D> ds = new List<Texture2D>();
        for(int i = 0; i < h; i++)
        {
            for(int j = 0; j < w; j++)
            {
                Texture2D texture = new Texture2D(wBlock, hBlock);
                texture.SetPixels(img.GetPixels(j * 48, i * 48, wBlock, hBlock));
                texture.Apply();
                ds.Add(texture);
            }
        }

        return ds;
    }


    public static void OnGUI()
    {
        //generate_sprite_sheet_image = GUILayout.Toggle(generate_sprite_sheet_image, "Generate Sprite Sheet Image");
        //Select_Image();

        if (img == null) return;

        GUILayout.Label("Select the tile you want to import, then click the 'Generate Tiles' Button");
        //can select or deselect all
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select All"))
        {
            for (int i = 0; i < sub_blocks_to_import.Count; i++)
                sub_blocks_to_import[i] = true;
        }
        if (GUILayout.Button("Select None"))
        {
            for (int i = 0; i < sub_blocks_to_import.Count; i++)
                sub_blocks_to_import[i] = false;
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Generate Tiles", "Create the selected auto tiles saving the asset in the default folders")))
        {
            //generate the final tiles for the tile palette
            Generate_Tiles(path, sub_blocks, sub_blocks_to_import, sub_block_names, mini_tile_w, mini_tile_h, wBlock, hBlock);
        }
        GUILayout.Space(10);
        if (GUILayout.Button(new GUIContent("Generate Tiles as", "Create the selected auto tiles saving the asset in the specified folders")))
        {
            string sprite_sheet_path = EditorUtility.OpenFolderPanel("Select the directory for the sprite sheets", "Assets", "");
            if (sprite_sheet_path != null && sprite_sheet_path != "")
            {
                string auto_tile_path = EditorUtility.OpenFolderPanel("Select the directory for the auto tiles", "Assets", "");
                if (auto_tile_path != null && auto_tile_path != "")
                {
                    sprite_sheet_path = sprite_sheet_path.Substring(sprite_sheet_path.IndexOf("Assets"));
                    auto_tile_path = auto_tile_path.Substring(auto_tile_path.IndexOf("Assets"));
                    //generate the final tiles for the tile palette
                    Generate_Tiles(path, sub_blocks, sub_blocks_to_import, sub_block_names, mini_tile_w, mini_tile_h, wBlock, hBlock,
                        sprite_sheet_path, auto_tile_path);
                }
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar,
            GUILayout.Height(Screen.height / 3 * 2));

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        //foreach (var sub in sub_blocks)
        for (int i = 0; i < sub_blocks.Count; i++)
        {
            if (i != 0 && i % 8 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            Texture2D sub = sub_blocks[i];
            //toggle to select the sub block of the image
            GUILayout.BeginVertical();
            sub_blocks_to_import[i] = GUILayout.Toggle(sub_blocks_to_import[i],
                new GUIContent(sub, "Click on the image to (un)toggle it"));
            sub_block_names[i] = GUILayout.TextArea(sub_block_names[i], GUILayout.Width(84));
            GUILayout.EndVertical();

        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// generate the tile of tipe A2 based on the parameter passed to the method
    /// </summary>
    /// <param name="path">The path of the input file</param>
    /// <param name="sub_blocks">collection of sliced block</param>
    /// <param name="sub_blocks_to_import">list of boolean to know which block we need to elaborate</param>
    /// <param name="mini_tile_w">size of the mini tile</param>
    /// <param name="mini_tile_h">size of the mini tile</param>
    /// <param name="wBlock">size of the final tile</param>
    /// <param name="hBlock">size of the final tile</param>
    /// <param name="generate_sprite_sheet_image"></param>
    public static void Generate_Tiles(string path, List<Texture2D> sub_blocks, List<bool> sub_blocks_to_import, List<string> names,
        int mini_tile_w, int mini_tile_h, int wBlock, int hBlock,
        string dest_sprite_sheet = "", string dest_auto_tile = "")
    {
        if (sub_blocks == null) return;
        string path_for_sprite_sheet = dest_sprite_sheet != "" ? dest_sprite_sheet : Tiles_Utility.Final_image_folder_path;
        string path_for_auto_tile = dest_auto_tile != "" ? dest_auto_tile : Tiles_Utility.Auto_Tile_Folder_Path;
        //create the final directory for the auto tile
        if (!Directory.Exists(path_for_auto_tile))
            Directory.CreateDirectory(path_for_auto_tile);

        //create the final directory for the generated Images
        if (!Directory.Exists(path_for_sprite_sheet))
            Directory.CreateDirectory(path_for_sprite_sheet);

        //create the folder for that specific file image
        string fileName = Path.GetFileNameWithoutExtension(path);
        
        string sprite_sheet_path = Path.Combine(path_for_sprite_sheet,
                    string.Format(@"{0}.png", Path.GetFileName(path).Replace(".png", "")));
        File.WriteAllBytes(sprite_sheet_path, img.EncodeToPNG());

        AssetDatabase.Refresh();
        TextureImporter importer = AssetImporter.GetAtPath(sprite_sheet_path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;
            importer.spritePixelsPerUnit = hBlock;
            importer.compressionQuality = 0;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = img.width;
            SpriteMetaData[] tmps = new SpriteMetaData[w * h];


            for(int y = 0; y < h; y++)
            {
                for(int x = 0; x < w; x++)
                {
                    int count = y * w + x;
                    SpriteMetaData smd = new SpriteMetaData();
                    smd = new SpriteMetaData
                    {
                        alignment = 0,
                        border = new Vector4(0, 0, 0, 0),
                        name = string.Format("{0}_{1:00}", Path.GetFileName(path).Replace(".png", ""), count),
                        pivot = new Vector2(.5f, .5f),
                        rect = new Rect(x * 48, img.height - (y + 1) * 48, wBlock, hBlock)
                    };
                    tmps[y * w + x] = smd;
                }
            }

            importer.spritesheet = tmps;
            importer.SaveAndReimport();
            
        }
        AssetDatabase.Refresh();
        Generate_A2_Tile(sprite_sheet_path, path_for_auto_tile, wBlock);
        AssetDatabase.Refresh();

    }

    public static void Generate_A2_Tile(string source_File_Path, string tile_path, int wBlock)
    {
        Debug.Log(tile_path);
        //Sprite[] sprites = Resources.LoadAll<Sprite>(source_File_Path.Replace(".png", ""));
        //Debug.Log(sprites.Length);
        var vars = Tiles_Utility.LoadSpriteSheet(source_File_Path);
        foreach(UnityEngine.Object a in vars)
        {
            Sprite sprite = a as Sprite;
            if(sprite != null)
            {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.name = sprite.name + "_pattet";
                tile.sprite = sprite;
                AssetDatabase.CreateAsset(tile, tile_path + "/" + tile.name + ".asset");
            }
        }
       
    }

}
