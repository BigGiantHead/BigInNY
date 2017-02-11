using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour {
	protected Ray ray;

	protected RaycastHit info;

	protected float height = 1.5f;

	// Use this for initialization
	void Start () {
		height = Random.Range (1.5f, 2.5f);

		ray = new Ray (transform.position, Vector3.down);

		CalculatePosition ();

		LevelManager.Instance.OnEarthQuake.AddListener (CalculatePosition);
	}

	void OnTriggerEnter (Collider collider) {
		PickedUp ();

		//move to random location on level, within prefined constraints
		//can be refactored better
		float x = -200;
		float dir = Random.Range(-1, 1);
		if (dir == 0) {
			dir = 1;
		}
		while (x < -90 || x > 90) {
			x = transform.position.x + dir * Random.Range(40f, 70f);
			if (dir == 1) {
				dir = -1;
			} else {
				dir = 1;
			}
		}

		GameObject pickup = Instantiate (gameObject, transform.parent) as GameObject;
		pickup.transform.position = new Vector3 (x, Random.Range (info.point.y + 1, info.point.y + 3.5f), 0);

		Destroy (gameObject);
	}

	protected abstract void PickedUp();

	protected virtual void CalculatePosition() {
		Vector3 pos = transform.position;

		ray.origin = transform.position;

		if (Ground.Instance.MyCollider.Raycast (ray, out info, 1000)) {
			pos.y = info.point.y + height;
		}

		transform.position = pos;
	}
}
