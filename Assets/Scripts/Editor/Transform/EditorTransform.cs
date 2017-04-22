using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTransform  {
	protected EditorRotation _rotationTool = new EditorRotation ();
	protected EditorTranslation _movementTool = new EditorTranslation ();

	protected bool _hideRotateTool;
	protected bool _hideTranslateTool;

	public void ResetTools () {
		_hideRotateTool = false;
		_hideTranslateTool = false;
	}

	public void ShowTranslateTool() {
		_hideTranslateTool = false;
		_hideRotateTool = true;
	}

	public void ShowRotateTool() {
		_hideTranslateTool = true;
		_hideRotateTool = false;
	}

	protected float Rotation(Vector2 position, Event e) {
		_rotationTool.Position = position;
		return _rotationTool.Rotate (e);
	}

	protected Vector2 Translation(Vector2 position, Event e) {
		_movementTool.Draw (position, e);
		return _movementTool.Translate(e);
	}
}
