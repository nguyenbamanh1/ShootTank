using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private static int SHOOT = 0;
    private static int TELEPORT = 1;

    public AudioClip[] clips;
    private AudioSource[] sources;
    private static Sound instance;
    private void Start()
    {
        instance = this;
        sources = new AudioSource[clips.Length];

        for(int i =0; i < clips.Length; i++)
        {
            sources[i] = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public static Sound gI() => instance;

    public void Shoot()
    {
        play(SHOOT);
    }

    public void Teleport()
    {
        play(TELEPORT);
    }

    private void play(int clip)
    {
        sources[clip].PlayOneShot(clips[clip]);
    }
}
