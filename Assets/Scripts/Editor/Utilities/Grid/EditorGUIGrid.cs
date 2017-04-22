using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorGUIGrid {
    public static void DrawGrid(float spacing, float opacity, Rect position) {
        Color c = Color.grey;
        c.a = opacity;

        Handles.color = c;

        int width = (int)(position.width / spacing);
        int height = (int)(position.height / spacing);

        Handles.BeginGUI();

        for (int x = 1; x <= width; x++)
        {
            Vector3 p1 = new Vector3(spacing * x, 0, 0);
            Vector3 p2 = new Vector3(spacing * x, position.height, 0);

            Handles.DrawLine(p1, p2);
        }

        for (int y = 1; y <= height; y++)
        {
            Vector3 p1 = new Vector3(0, spacing * y, 0);
            Vector3 p2 = new Vector3(position.width, spacing * y, 0);

            Handles.DrawLine(p1, p2);
        }

        Handles.EndGUI();

		GUI.changed = true;
    }
}
