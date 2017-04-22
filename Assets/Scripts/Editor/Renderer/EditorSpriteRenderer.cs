using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;

using System.Threading;

public class EditorSpriteRenderer {
	private Sprite _sprite;

	public Sprite Sprite {
		get { 
			return _sprite;
		} set { 
			_sprite = value;
		}
	}

	public void RenderSprite(Vector2 position) {
        if (_sprite != null)
        {
			Vector2 size = new Vector2(_sprite.rect.width / 5, _sprite.rect.height / 5);
			Vector2 pivot = new Vector2(position.x, position.y);

//			(_sprite.rect.width / 100f * (_sprite.pivot.x * 100f))
//			(_sprite.rect.height / 100f * (_sprite.pivot.y * 100f))

			Rect rect = new Rect(pivot, size);

			Color[] pixels = _sprite.texture.GetPixels(
				(int)_sprite.rect.x,
				(int)_sprite.rect.y,
				(int)_sprite.rect.width,
				(int)_sprite.rect.height);
			Texture2D tcropped = new Texture2D(
				(int)_sprite.rect.width,
				(int)_sprite.rect.height);
			tcropped.SetPixels(pixels);

			tcropped.Apply ();


			GUI.DrawTexture (rect, tcropped);

        }
    }
}
