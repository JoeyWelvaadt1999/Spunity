using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Extensions;

public class BoneJoint : EditorTransform {
	private List<BoneJoint> _children = new List<BoneJoint>();

    public List<BoneJoint> Children {
        get {
            return _children;
        }
    }

    private string _name;

    public string Name {
        get {
            return _name;
        } set {
            _name = value;
        }
    }

	private Vector2 _position;

	public Vector2 Position {
		get {
			return _position;
		} set {
            _position = value;
        }
	}

	private float _rotation;

    public float Rotation {
        get
        {
            return _rotation;
        } set {
            _rotation = value;
        }
    }

    private BoneJoint _parent;

    public BoneJoint Parent
    {
        get {
            return _parent;
        } set {
            _parent = value;
        }
    }

	private float _size = 10f;

	private EditorSpriteRenderer _spriteRenderer = new EditorSpriteRenderer();

	public EditorSpriteRenderer SpriteRenderer {
		get { 
			return _spriteRenderer;
		}
	}

	public BoneJoint(Vector2 position) {
		_position = position;

        _name = "Bone joint";
	}

	public void Update(Event e) {
		if (_hideRotateTool) {
			Move (e);
		} 

		if (_hideTranslateTool) {
			_rotation = Rotation (_position, e);
			CalculateChildren ();
		}

		Draw ();

		_spriteRenderer.RenderSprite (_position);

	}

	public void AddChild(BoneJoint child) {
		_children.Add (child);
	}

	public bool InsideJoint(Event e) {
		Vector2 pos = e.mousePosition;
		if (pos.x > _position.x - _size && pos.x < _position.x + _size && pos.y > _position.y - _size && pos.y < _position.y + _size) {
			return true;
		}

		return false;
	}

	private void Draw() {
		Handles.BeginGUI ();
		Handles.color = Color.green;

		Handles.CircleCap (0, _position, Quaternion.Euler(0,0, _rotation), _size);

		for (int i = 0; i < _children.Count; i++) {
			Handles.DrawLine (_position, _children [i]._position);
		}

		Handles.EndGUI ();
	}

	private void Move(Event e) {
		Vector2 translation = Translation (_position, e);
        _position += translation;

		for (int i = 0; i < _children.Count; i++) {
			_children [i].Position += translation;
			for (int j = 0; j < GetChildren(_children[i]).Count; j++) {
				GetChildren(_children[i]) [j].Position += translation;
			}				
		}
	}

	private List<BoneJoint> GetChildren(BoneJoint joint) {
		List<BoneJoint> children = new List<BoneJoint>();

		for (int i = 0; i < joint.Children.Count; i++) {
			children.Combine (joint.Children[i].GetChildren(joint.Children[i]));
		}
		return children;
	}

	private void CalculateChildren () {
		for (int i = 0; i < _children.Count; i++) {
			float radius = (_position - _children [i]._position).magnitude;

			float x = _position.x + radius * Mathf.Cos (_rotation * Mathf.Deg2Rad);
			float y = _position.y + radius * Mathf.Sin (_rotation * Mathf.Deg2Rad);

			_children [i]._position = new Vector2 (x, y);
		}
	}
}

[System.Serializable]
public class BoneInformation {
    public int Index;
    public Vector2 Position;
    public float Rotation;
}