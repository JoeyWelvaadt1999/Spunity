using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SpunityEditor))]
public class BoneInspector : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
