using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Linq;

public class SpriteSheetConverter : EditorWindow {
    private Sprite _sprite;

    [MenuItem("Tools/Spritesheetconverter")]
    private static void ShowConverter() {
        SpriteSheetConverter window = (SpriteSheetConverter)EditorWindow.GetWindow<SpriteSheetConverter>();
        window.Show();
    }

    private void OnGUI()
    {
        _sprite = (Sprite)EditorGUILayout.ObjectField(_sprite, typeof(Sprite));

        if (_sprite != null)
        {
            Debug.Log(_sprite.vertices.Length);
        }
    }
}
