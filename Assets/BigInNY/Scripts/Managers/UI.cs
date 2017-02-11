using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	[SerializeField]
	private Text coins = null;

	[SerializeField]
	private Text time = null;

	[SerializeField]
	private GameObject intro = null;

	[SerializeField]
	private GameObject outro = null;

	// Use this for initialization
	void Start () {
		LevelManager.Instance.OnStateChanged.AddListener (OnLevelStateChanged);
	}
	
	// Update is called once per frame
	void Update () {
		time.text = string.Format ("{0:0.0}", LevelManager.Instance.TimeTillNextEarthQuake);
		coins.text = string.Format ("{0} mill. $", LevelManager.Instance.Coins);
	}

	private void OnLevelStateChanged(LevelManager.State state) {
		intro.SetActive (false);
		outro.SetActive (false);

		switch (state) {
		case LevelManager.State.Starting:
			intro.SetActive (true);
			break;
		case LevelManager.State.GameOver:
			outro.SetActive (true);
			break;
		}
	}
}
