using UnityEngine;
using System.Collections;

public class Pedestrian : MonoBehaviour {
	private Coroutine doTurnOnSpeechBubble = null;

	private Coroutine doBehaviour = null;

	private float mustachey = 0;

	private float maxSpeed = 1f;

	private float speed = 1f;

	private float actionDuration = 10;

	private float timer = 0;

	[SerializeField]
	private GameObject speechBubble = null;

	[SerializeField]
	private GameObject mustache = null;

	[SerializeField]
	private AudioSource whistle = null;

	// Use this for initialization
	void Start () {
		if (mustache != null) {
			mustachey = mustache.transform.localPosition.y;
		}
		PickAction ();
	}

	// Update is called once per frame
	void Update () {
		this.speed = Mathf.Lerp(speed, LevelManager.Instance.GameSpeed * maxSpeed, 0.1f);
	}

	void OnTriggerEnter (Collider other) {
		Player.Instance.OnDucking.AddListener (TurnOnSpeachBubble);
		Player.Instance.OnJumping.AddListener (TurnOnSpeachBubble);
		Player.Instance.OnRunning.AddListener (TurnOnSpeachBubble);
	}

	void OnTriggerExit (Collider other) {
		Player.Instance.OnDucking.RemoveListener (TurnOnSpeachBubble);
		Player.Instance.OnJumping.RemoveListener (TurnOnSpeachBubble);
		Player.Instance.OnRunning.RemoveListener (TurnOnSpeachBubble);
	}

	private IEnumerator Move (float duration, float direction) {
		if (!whistle.isPlaying) {
			if (Random.value < 0.1f) {
				whistle.Play ();
			}
		}

		if (mustache != null) {
			mustache.transform.localPosition = new Vector3 (0, mustachey, -0.5f);
		}

		while (Time.timeSinceLevelLoad - timer < duration) {
			transform.position += direction * Vector3.left * speed * Time.fixedDeltaTime;

			if (mustache != null) {
				Vector3 mustachePos = mustache.transform.localPosition;
				mustachePos.y = mustachey + Mathf.Sin (Mathf.PingPong (Time.timeSinceLevelLoad * 4, Mathf.PI)) * 0.05f;
				mustache.transform.localPosition = mustachePos;
			}
			yield return new WaitForFixedUpdate ();
		}	

		PickAction ();

		yield break;
	}
	
	private IEnumerator Stand (float duration) {
		if (whistle.isPlaying) {
			whistle.Stop ();
		}

		if (mustache != null) {
			mustache.transform.localPosition = new Vector3 (0, mustachey, -0.5f);
		}

		yield return new WaitForSeconds (duration);

		PickAction ();

		yield break;
	}

	void StopBehaviours () {
		if (doBehaviour != null) {
			StopCoroutine (doBehaviour);
			doBehaviour = null;
		}
	}

	void PickAction () {
		StopBehaviours ();

		int randomAction = Random.Range(0, 3);
		timer = Time.timeSinceLevelLoad;

		switch (randomAction) {
		case 0:
			doBehaviour = StartCoroutine (Stand(actionDuration));
			break;
		case 1:
			doBehaviour = StartCoroutine (Move (actionDuration, 1));
			break;
		case 2:
			doBehaviour = StartCoroutine (Move (actionDuration, -1));
			break;
		}
	}

	private void TurnOnSpeachBubble () {
		if (doTurnOnSpeechBubble == null) {
			doTurnOnSpeechBubble = StartCoroutine (DoTurnOnSpeechBubble ());
		}
	}

	private IEnumerator DoTurnOnSpeechBubble() {
		if (!speechBubble.activeSelf) {
			LevelManager.Instance.LowerTimer ();

			if (SoundManager.Instance.ButtFace != null && !SoundManager.Instance.ButtFace.isPlaying) {
				SoundManager.Instance.ButtFace.Play ();
			}

			speechBubble.SetActive (true);

			yield return new WaitForSeconds (3f);

			speechBubble.SetActive (false);

			StopBehaviours ();
			doBehaviour = StartCoroutine (Stand (3));
		}

		doTurnOnSpeechBubble = null;

		yield return null;
	}
}
