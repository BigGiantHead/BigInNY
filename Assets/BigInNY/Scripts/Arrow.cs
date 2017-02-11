using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	private GameObject stabilizer;

	[SerializeField]
	private Renderer myRenderer;

	// Use this for initialization
	void Start () {
		transform.SetParent (null);

		LevelManager.Instance.OnStateChanged.AddListener ((state) => {
			myRenderer.enabled = state != LevelManager.State.GameOver;
		});
	}
	
	// Update is called once per frame
	void Update () {
		if (stabilizer == null) {
			stabilizer = GameObject.FindGameObjectWithTag ("Stabilizer");
		}

		Vector3 dir = stabilizer.transform.position - Player.Instance.Top.position;
		dir.Normalize ();

		transform.position = Player.Instance.Top.position + dir;
		transform.LookAt (stabilizer.transform.position);
		transform.rotation *= Quaternion.AngleAxis (90, transform.right);
	}
}
