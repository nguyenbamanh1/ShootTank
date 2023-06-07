using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
public class Rms 
{
    

    public static string GetCurrentPath()
    {
        return Application.persistentDataPath;
    }

    public static string loadString(string fileName)
    {
        try
        {
            byte[] array = load(fileName);
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            return uTF8Encoding.GetString(array);
        }
        catch
        {
            return string.Empty;
        }
    }


    public static byte[] load(string fileName)
    {
        try
        {
            string path = GetCurrentPath() + "/" + fileName;
            FileStream stream = new FileStream(path, FileMode.Open);
            byte[] arr = new byte[stream.Length];
            stream.Read(arr, 0, arr.Length);
            stream.Close();
            arr = Encrypt(arr);
            return arr;
        }
        catch
        {
            return null;
        }
    }

    public static void saveString(string fileName, string a)
    {
        try
        {
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            save(fileName, uTF8Encoding.GetBytes(a));
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public static void save(string fileName, byte[] data)
    {
        try
        {
            string path = GetCurrentPath() + "/" + fileName;
            
            FileStream stream = new FileStream(path, FileMode.Create);

            data = Encrypt(data);

            stream.Write(data);
            stream.Flush();
            stream.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }


    public static byte[] Encrypt(byte[] data)
    {
#if !UNITY_EDITOR
        byte[] keys = new byte[] { 24, 12, 18, 19 };
        int index = 0;
        for(int i =0; i < data.Length; i++)
        {
            
            data[i] = (byte)((data[i] & 0xff) ^ (keys[index++] & 0xff));
            if(index >= keys.Length)
            {
                index %= keys.Length;
            }

        }
#endif

        return data;


    }
    
}
