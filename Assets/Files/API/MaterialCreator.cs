using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    [SerializeField] private Material materialTemplate; // The material template to copy properties from

    public static MaterialCreator Instance;

    public Texture2D imageAsset;


    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("More than one MaterialCreator in scene.");
        }
        Instance = this;
    }
    public Texture2D CreateTexture(string _Username)
    {
        try
        {
            string folderPath = Application.streamingAssetsPath + "/" + _Username + ".png";
            //Debug.Log(folderPath);
            Texture2D texture = Resources.Load<Texture2D>(_Username);
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError("Texture was not found. " + e);
            return null;
        }
    }


    public Texture2D ImageLoader(string _Username)
    {
        //Create an array of file paths from which to choose
        string folderPath = Application.streamingAssetsPath + "/" + _Username + ".png";  //Get path of folder
        Debug.Log(folderPath);


        //Converts desired path into byte array
        byte[] bytes = System.IO.File.ReadAllBytes(folderPath);

        //Creates texture and loads byte array data to create image
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);

        return tex;
    }
}
