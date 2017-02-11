using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ground : SingletonMonoBehaviour<Ground> {
	private float height = 1;

	private float scaley = 0;

	private Vector3 scale = Vector3.zero;

	[SerializeField]
	private Collider myCollider = null;

	[SerializeField]
	private Renderer myRenderer = null;

	public Collider MyCollider {
		get {
			return myCollider;
		}
	}

	public Renderer MyRenderer {
		get {
			return myRenderer;
		}
	}

	#region EVENTS
	public UnityEvent OnScaledToMinimum;
	#endregion

	// Use this for initialization
	void Start () {
		scale = transform.localScale;
		scaley = scale.y;

		LevelManager.Instance.OnEarthQuake.AddListener (() => {
			height = height / 2f;
		});
	}

	void Update() {
		scale.y = scaley * height;
		transform.localScale = scale;

		if (scale.y < 0.025f && MyCollider.enabled) {
			if (OnScaledToMinimum != null) {
				OnScaledToMinimum.Invoke ();
			}
			MyCollider.enabled = false;
			MyRenderer.enabled = false;
		}
	}
}
