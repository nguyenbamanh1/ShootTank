using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public const byte SHOOT1 = 0;
    public const byte SHOOT2 = 1;
    public const byte HEATH = 2;
    public const byte TELE = 3;
    private static AnimManager instance;
    public Animator animTemp;

    public RuntimeAnimatorController[] animClips;

    public static AnimManager gI()
    {
        return instance;
    }


    private void Awake()
    {
        instance = this;
    }


    public void headth(Vector2 pos)
    {
        _start(HEATH, pos);
    }

    public void shoot1(Vector2 pos)
    {
        _start(SHOOT1, pos);
    }

    public void shoot2(Vector2 pos)
    {
        _start(SHOOT2, pos);
    }

    public void teleport(Vector2 pos)
    {
        _start(TELE, pos);
    }

    public void _start(int id, Vector2 pos)
    {
        var obj = Instantiate(animTemp.gameObject, pos, Quaternion.identity, null);
        obj.GetComponent<Animator>().runtimeAnimatorController = animClips[id];
    }

}
