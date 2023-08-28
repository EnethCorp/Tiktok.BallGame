using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialCreator : MonoBehaviour
{
    [SerializeField] private Material materialTemplate; // The material template to copy properties from
    [SerializeField] private string textureFolderPath;

    public static MaterialCreator Instance;

    private void Start()
    {
        if (Instance)
        {
            Debug.LogError("More than one MaterialCreator in scene.");
        }
        Instance = this;

        if (textureFolderPath == "")
        {
            textureFolderPath = GetScriptFolderPath() + "/Resources/";
        }
    }

    public Material CreateMaterial(string _Username)
    {
        Material createdMaterial = null;
        string[] texturePaths = System.IO.Directory.GetFiles(textureFolderPath, "*.png"); // Change extension as needed

        foreach (string texturePath in texturePaths)
        {
            if (texturePath.Replace(".png", "").Replace(".jpg", "").Split("profilepictures/")[1] != _Username)
            {
                continue;
            }

            Debug.Log(texturePath.Replace(".png", "").Split("profilepictures/")[1]);

            Debug.Log("Created material for " + texturePath.Replace(".png", "").Split("profilepictures/")[1]);

            string textureName = System.IO.Path.GetFileNameWithoutExtension(texturePath);
            Debug.Log("TEXTURE NAME: " + textureName);
            Texture2D texture = Resources.Load<Texture2D>(textureName);

            if (texture != null)
            {
                createdMaterial = new Material(materialTemplate);
                createdMaterial.mainTexture = texture;
            }
        }
        Debug.Log("MATERIAL: " + createdMaterial);
        return createdMaterial;
    }
    public Texture CreateTexture(string _Username)
    {
        try
        {
            Texture2D texture = Resources.Load<Texture2D>(_Username);
            Debug.Log("profilepictures/" + _Username);
            Debug.Log(texture);
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError("Texture was not found. " + e);
            return null;
        }
    }

    string GetScriptFolderPath()
    {
        // Get the path of the script's asset in the project
        MonoScript script = MonoScript.FromMonoBehaviour(this);
        string scriptAssetPath = UnityEditor.AssetDatabase.GetAssetPath(script);

        // Convert the asset path to the system file path
        string fullPath = System.IO.Path.Combine(Application.dataPath, scriptAssetPath.Replace("Assets/", ""));
        //fullPath += "/Files/API";
        string folderPath = fullPath.Replace("/MaterialCreator.cs", "");
        print(folderPath);

        // Remove the script's filename to get the script's folder path
        //string folderPath = System.IO.Path.GetDirectoryName(fullPath);

        return folderPath;
    }
}
