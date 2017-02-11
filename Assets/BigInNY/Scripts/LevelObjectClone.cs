using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectClone : MonoBehaviour {
	private GameObject parallel = null;

	public GameObject ParallelPrefab = null;

	// Use this for initialization
	void Start () {
		if (ParallelPrefab != null) {
			parallel = Instantiate(ParallelPrefab, transform.parent) as GameObject;
			parallel.transform.position = transform.position;
			parallel.transform.rotation = transform.rotation;

			Vector3 scale = parallel.transform.localScale;
			scale.y = -scale.y;
			parallel.transform.localScale = scale;

			Vector3 pos = transform.position;
			pos.y = -pos.y;
			parallel.transform.position = pos;
		}
	}

	// Update is called once per frame
	void Update () {
		if (parallel != null) {
			Vector3 pos = transform.position;
			pos.y = -pos.y;
			parallel.transform.position = pos;
			parallel.transform.rotation = transform.rotation;
		}
	}
}
