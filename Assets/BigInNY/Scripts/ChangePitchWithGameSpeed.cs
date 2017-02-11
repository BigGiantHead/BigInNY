using UnityEngine;
using System.Collections;

public class ChangePitchWithGameSpeed : MonoBehaviour {
	[SerializeField]
	private AudioSource[] sources = null;

	// Use this for initialization
	void Start () {
		sources = gameObject.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < sources.Length; ++i) {
			sources[i].pitch = Mathf.Lerp (sources[i].pitch, Mathf.Clamp (LevelManager.Instance.GameSpeed, 0.7f, 1f), 0.015f);
		}
	}
}
