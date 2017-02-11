using UnityEngine;
using System.Collections;

public class CameraController : SingletonMonoBehaviour<CameraController> {
	private float groundHeight = 0;

	private float z = 0;

	private float shake = 0;

	public Transform TopPlayer = null;

	public GameObject Ground = null;

	// Use this for initialization
	void Start () {
		z = transform.position.z;
		groundHeight = Ground.GetComponent<Renderer>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = TopPlayer.position;
		pos.y = transform.position.y;
		pos.z = z;

		if (Physics.gravity.y != 0 && LevelManager.Instance.CurrentState != LevelManager.State.GameOver) {
			Ray ray = new Ray (TopPlayer.position, Vector3.down);
			RaycastHit info;

			if (Ground.GetComponent<Renderer>().bounds.extents.y >= groundHeight / 2 && Ground.GetComponent<Collider>().Raycast (ray, out info, 1000)) {
				pos.y = info.point.y + Ground.GetComponent<Renderer>().bounds.extents.y * 0.25f;
			} else {
				pos.y = 0;
			}
		}
		
		transform.position = Vector3.Lerp(transform.position, pos, 0.075f);

		transform.localPosition += Random.onUnitSphere * shake;
		shake = Mathf.Lerp (shake, 0, 0.05f);
	}

	public void Shake () {
		this.shake = 0.7f;
	}
}
