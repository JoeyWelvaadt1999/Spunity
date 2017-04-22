using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpunityEditor : EditorWindow {
	private string _resourcesPath = "Assets/Scripts/Editor/Resources/";

	private List<BoneJoint> _joints = new List<BoneJoint> ();
    private List<KeyFrame> _frames = new List<KeyFrame>();

	private BoneJoint _currentJoint;

    private EditorAnimation _animation;
    private string _animName;
    private string _animPath;

    private Vector2 _inspectorScrollPosition = Vector2.zero;
    private Rect _inspectorScrollRect;

	private Vector2 _scrollPosition = Vector2.zero;
	private Texture2D _scrollTex;
	private Rect _scrollRect;

	private Rect _lastRect;

    [MenuItem("Animation/Spunity %u")]
    private static void ShowEditor() {
        SpunityEditor window = (SpunityEditor)EditorWindow.GetWindow<SpunityEditor>();
        window.Show();
		window.minSize = new Vector2 (320, 320);
    }

	private void OnEnable() {
		CreateRects ();
		CreateTextures ();
	}

	private void CreateRects() {
		_scrollRect = new Rect (200, 17, position.width - 200, position.height - 17 - 50);
        _inspectorScrollRect = new Rect(0, 17, 200, position.height / 2 - 17);
	}

	private void CreateTextures() {
		_scrollTex = new Texture2D ((int)_scrollRect.width, (int)_scrollRect.height);
		for (int x = 0; x < _scrollTex.width; x++) {
			for (int y = 0; y < _scrollTex.height; y++) {
				_scrollTex.SetPixel (x, y, new Color32(42,42,42,255));
			}
		}

		_scrollTex.Apply ();
	}

    private void OnGUI()
    {
        if (_animation != null)
        {
            if (position != _lastRect)
            {
                CreateRects();
                CreateTextures();
            }

            TransformGUI();

            CreateEditorWindow();
            CreateHierarchyWindow();
            CreateInspectorWindow(_currentJoint);
            EditorKeyFrames.Update(Event.current, position);

            UpdateJoints(Event.current);

            CheckInput(Event.current);

            //esr.RenderSprite(new Vector2(position.width / 2, position.height / 2));


            

            _lastRect = position;
        }
        else {
            GUI.Label(new Rect(position.width / 2 - 100, position.height / 2 - 10, 200, 20), "No animation is selected!");
            string temp = EditorGUI.TextField(new Rect(position.width / 2 - 125, position.height / 2 + 10, 200, 20), _animName);
            if (GUI.Button(new Rect(position.width / 2 - 125, position.height / 2 + 30, 200, 20), "Create Animation")) {
                if (_animName != string.Empty) {
                    _animation = new EditorAnimation(_animName, EditorUtility.SaveFilePanel("",Application.dataPath, "new SpunityAnimation", "json"));
                }
            }

            if (GUI.Button(new Rect(position.width / 2 - 125, position.height / 2 + 50, 200, 20), "Select Animation"))
            {
                _animPath = EditorUtility.OpenFilePanel("", Application.dataPath, "json");
                _animName = System.IO.Path.GetFileNameWithoutExtension(_animPath);
                _animation = new EditorAnimation(_animName, _animPath);
            }

        }

        if (GUI.changed) Repaint();
    }

	private void UpdateJoints(Event e) {
		for (int i = 0; i < _joints.Count; i++) {
			_joints [i].Update (e);
		}
	}

	private void CheckInput(Event e) {
		switch (e.type) {
		case EventType.mouseDown:
			if (e.button == 1) {
				for (int i = 0; i < _joints.Count; i++) {
					if (_joints [i].InsideJoint (e)) {
						ShowJointMenu (e, _joints[i]);
						return;
					} 
				}

                    if (e.mousePosition.x > 200 && e.mousePosition.y > 17) {
                        ShowMenu(e);
                    }
			}

			if (e.button == 0) {
				for (int i = 0; i < _joints.Count; i++) {
					if (_joints [i].InsideJoint (e)) {
                        _currentJoint.ResetTools();
						_currentJoint = _joints [i];
						return;
					} 
				}
			}
			break;
		}
	}

	private void ShowMenu(Event e) {
		GenericMenu menu = new GenericMenu ();
		menu.AddItem (new GUIContent("Add joint"), false, () => CreateJoint(e.mousePosition));
		menu.ShowAsContext ();
	}

	private void ShowJointMenu(Event e, BoneJoint joint) {
		GenericMenu menu = new GenericMenu ();
		menu.AddItem (new GUIContent("Connect to joint"), false, () => ConnectJoint(joint));
        menu.AddItem(new GUIContent("Remove joint"), false, () => RemoveJoint(joint));
        menu.ShowAsContext ();
	}

	private void CreateJoint(Vector2 position) {
		BoneJoint joint = new BoneJoint (position);
		_joints.Add (joint);
		if (_currentJoint != null) {
            joint.Parent = _currentJoint;
			_currentJoint.AddChild (joint);
		}
	}

	private void ConnectJoint(BoneJoint joint) {
		if (_currentJoint != null) {
			if (joint != _currentJoint) {
				_currentJoint.AddChild (joint);
			}
		}
	}

    private void RemoveJoint(BoneJoint joint) {
        if (joint.Parent != null) {
            joint.Parent.Children.Remove(joint);
        }

        _joints.Remove(joint);
    }

	private void TransformGUI() {
        GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(position.width), GUILayout.MinHeight(30));

		Texture2D freeToolTexture = AssetDatabase.LoadAssetAtPath (_resourcesPath + "FreeTool.png", typeof(Texture2D)) as Texture2D;
		Texture2D moveToolTexture = AssetDatabase.LoadAssetAtPath (_resourcesPath + "MoveTool.png", typeof(Texture2D)) as Texture2D;
		Texture2D rotateToolTexture = AssetDatabase.LoadAssetAtPath (_resourcesPath + "RotateTool.png", typeof(Texture2D)) as Texture2D;

		float width = 30;
        float height = 17;

		if (GUILayout.Button (freeToolTexture, EditorStyles.toolbarButton, GUILayout.Width (width), GUILayout.Height (height))) {
			_currentJoint.ResetTools ();
		}

		if (GUILayout.Button (moveToolTexture, EditorStyles.toolbarButton, GUILayout.Width (width), GUILayout.Height (height))) {
			_currentJoint.ShowTranslateTool ();
		}

		if (GUILayout.Button (rotateToolTexture, EditorStyles.toolbarButton, GUILayout.Width (width), GUILayout.Height (height))) {
			_currentJoint.ShowRotateTool ();
		}

        if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(width * 2), GUILayout.Height(height))) {
            KeyFrame _frame = new KeyFrame();

            BoneInformation[] bis = new BoneInformation[_joints.Count];

            for (int i = 0; i < _joints.Count; i++)
            {
                BoneInformation bi = new BoneInformation();
                bi.Index = i;
                bi.Position = _joints[i].Position;
                bi.Rotation = _joints[i].Rotation;

                bis[i] = bi;
            }

            _frame.Index = EditorKeyFrames.SelectedKeyFrame;
            _frame.Bones = bis;

            _frames.Add(_frame);
            _animation.SetJsonObject(CreateJson());
        }

		GUILayout.EndHorizontal ();
	}

	private void CreateEditorWindow() {
		_scrollPosition = GUI.BeginScrollView (_scrollRect, _scrollPosition, new Rect(0, 0, position.width, position.height), GUIStyle.none, GUIStyle.none);

		GUI.DrawTexture (new Rect(0,0,position.width,position.height), _scrollTex);

		EditorGUIGrid.DrawGrid(10, 0.2f, position);
		EditorGUIGrid.DrawGrid(100, 0.4f, position);

		GUI.EndScrollView ();
	}

	private void CreateHierarchyWindow() {
        GUILayout.BeginVertical(GUILayout.Width(180));

        _inspectorScrollPosition = GUI.BeginScrollView(_inspectorScrollRect, _inspectorScrollPosition, new Rect(0, 0, 190, (22 * _joints.Count)), false, false);

        for (int i = 0; i < _joints.Count; i++)
        {

            if (GUILayout.Button(_joints[i].Name)) {
                if (_currentJoint != null)
                {
                    _currentJoint.ResetTools();
                }
                _currentJoint = _joints[i];
            }
        }

        GUILayout.EndVertical();
        GUI.EndScrollView();
	}

    private void CreateInspectorWindow(BoneJoint joint) {
        if (joint != null)
        {
            GUILayout.BeginArea(new Rect(0, position.height / 2, 200, position.height / 2 - 17));

            GUILayout.Label("Name");
            joint.Name = EditorGUILayout.TextField(joint.Name);
            joint.Position = EditorGUILayout.Vector2Field("Position", joint.Position);
            GUILayout.Label("Rotation");
            joint.Rotation = EditorGUILayout.FloatField(joint.Rotation);

			joint.SpriteRenderer.Sprite = EditorGUILayout.ObjectField ("Sprite", joint.SpriteRenderer.Sprite, typeof(Sprite)) as Sprite;

            if (joint.Parent != null)
            {
                GUILayout.Label("Parent");
                GUILayout.Label(joint.Parent.Name);
            }

            GUILayout.EndArea();
        }
    }

    private JsonObject CreateJson() {
        JsonObject json = new JsonObject();
        json.Frames = _frames.ToArray();

        return json;
    }
}