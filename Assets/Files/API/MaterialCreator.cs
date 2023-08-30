using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    [SerializeField] private Material materialTemplate; // The material template to copy properties from

    public static MaterialCreator Instance;

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
            Debug.Log(folderPath);
            Texture2D texture = Resources.Load<Texture2D>(_Username);
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError("Texture was not found. " + e);
            return null;
        }
    }
}
