using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.IO;

public class EditorAnimation {
    private FileStream _jsonAnimation;

    public FileStream JsonAnimation {
        get {
            return _jsonAnimation;
        }
    }

    private string _name;
    private string _path;

    public EditorAnimation(string name, string path) {
        _path = path;
        if (path == string.Empty) {
            _path = GetPathFromName(name);
        }
       
        CreateJson(_path);
    }

    public void SetJsonObject(JsonObject json) {
        Debug.Log(json.Frames[0].Bones.Length);
        string text = EditorJsonUtility.ToJson(json);
        string path = _jsonAnimation.Name;

        Debug.Log(text);
        File.WriteAllText(path, text);
    }

    private string GetPathFromName(string name) {
        string path = Application.dataPath + "/Json/" + name + ".json";
        return path;
    }

    private void CreateJson(string path)
    {
        if (File.Exists(path))
        {
            _jsonAnimation = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }
        else {
            _jsonAnimation = File.Create(path);
        }

        _jsonAnimation.Close();
    }
}

[System.Serializable]
public class JsonObject {
    public KeyFrame[] Frames;
}
