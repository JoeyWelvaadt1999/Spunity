using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTranslation {
	private int _size = 100;
	private int _deathZone = 8;
	private Vector3 _position;
	private Vector2 _lastMousePosition;
	private bool _hasClickedMoveTool;

	public Vector2 Translate(Event e) {
		CheckInput (e);

		if (_hasClickedMoveTool) {
			float xDiff = e.mousePosition.x - _lastMousePosition.x;
			float yDiff = e.mousePosition.y - _lastMousePosition.y;	


			Vector2 translation = new Vector2 (xDiff * AxisClick(e).x, yDiff * AxisClick(e).y);

			_lastMousePosition = e.mousePosition;
			return translation;
		}

		return Vector2.zero;
	}

	public void Draw(Vector2 position, Event e) {
		_position = new Vector3(position.x, position.y, -10);

		Handles.BeginGUI ();
		Handles.color = Handles.xAxisColor;
		Handles.ArrowCap(1, _position, Quaternion.Euler(0, 90, 0), _size);
		Handles.color = Handles.yAxisColor;
		Handles.ArrowCap(1, _position, Quaternion.Euler(90, 0, 0), _size);
		Handles.EndGUI ();
	}

	private Vector2 AxisClick(Event e) {
		Vector2 mousePosition = e.mousePosition;
		if (_hasClickedMoveTool) {
			if (mousePosition.x > _position.x && mousePosition.x < _position.x + _size && mousePosition.y > _position.y - _deathZone && mousePosition.y < _position.y + _deathZone) {
				return Vector2.right;			
			}

			if (mousePosition.y < _position.y && mousePosition.y > _position.y - _size && mousePosition.x > _position.x - _deathZone && mousePosition.x < _position.x + _deathZone) {
				return Vector2.up;
			}
		}
		return Vector2.zero;
	}

	private void CheckInput(Event e) {
		if (e.type == EventType.mouseDown) {
			_lastMousePosition = e.mousePosition;
			_hasClickedMoveTool = true;
		}

		if (e.type == EventType.mouseUp) {
			_hasClickedMoveTool = false;
		}
	}
}

