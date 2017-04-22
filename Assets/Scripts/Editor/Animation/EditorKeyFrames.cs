using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorKeyFrames {

    public static int SelectedKeyFrame = 1;

    public static void Update(Event e, Rect position) {
        Draw(position);
    }

    private static void Draw(Rect position) {
        GUILayout.BeginArea(new Rect(200, position.height -50, position.width - 200, 50));

        float width = position.width - 300;
        float buttonWidth = width / 120;


        GUILayout.BeginVertical();
        SelectedKeyFrame = int.Parse( GUILayout.TextField(SelectedKeyFrame.ToString(),GUILayout.Width(100), GUILayout.Height(50)));
        for (int i = 0; i < 120; i++)
        {
            if (GUI.Button(new Rect(100 +  buttonWidth * i, 0, buttonWidth, 50), "F")) {
                Debug.Log(i + 1);
                SelectedKeyFrame = i + 1;
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

}

[System.Serializable]
public class KeyFrame {
    public int Index;
    public BoneInformation[] Bones;
}
