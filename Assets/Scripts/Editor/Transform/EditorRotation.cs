using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorRotation {
    public Vector2 Position;
	private float Size = 50;
    private float Rotation;

    private int DeathZone = 8;

    private bool HasClickedRotateTool = false;

    private Vector2 LastPosition;

    public float Rotate(Event e) {
        Draw();

        CheckInput(e);

        if (e.type == EventType.MouseUp)
        {
            HasClickedRotateTool = false;
        }

        UpdateRotation(e);

        return Rotation;
    }

    public void UpdateRotation(Event e) {
		CheckInput (e);
        if (HasClickedRotateTool) {
            float diff = e.mousePosition.x - LastPosition.x;

            Handles.BeginGUI();
			Color c = Handles.zAxisColor;
            c.a = 0.05f;
            Handles.color = c;
            Handles.DrawSolidArc(Position, Vector3.forward, Vector3.down, Rotation , Size);
			Handles.EndGUI ();

            Rotation += diff;

            LastPosition = e.mousePosition;
        }
    }

    public void CheckInput(Event e) {
        Vector2 mousePos = e.mousePosition;
        float radius = Mathf.Sqrt(Mathf.Pow(mousePos.x - Position.x, 2) + Mathf.Pow(mousePos.y - Position.y, 2));

        if (radius > Size - DeathZone && radius < Size + DeathZone) {
            if (e.type == EventType.MouseDown) {
                LastPosition = e.mousePosition;
                HasClickedRotateTool = true;
            }
        }
    }

    public void Draw() {
		
        Handles.BeginGUI();
		Handles.color = Handles.zAxisColor;
        Handles.DrawWireArc(Position, Vector3.forward, Vector3.up, 360, Size);
        Handles.EndGUI();
    }
}
