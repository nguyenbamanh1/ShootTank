using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletItem))]
public class BulletEditor : Editor
{
    public BulletItem bullet
    {
        get { return target as BulletItem; }
    }

    private void OnEnable()
    {
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if(bullet.icon != null)
        {
            return bullet.icon.texture;
        }
        else
        {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }
        
    }
}
