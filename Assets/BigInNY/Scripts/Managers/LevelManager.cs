using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelManager : SingletonMonoBehaviour<LevelManager> {
	public class LevelManagerStateAction : UnityEvent<State> {
	}

	public enum State {
		None,

		Starting,

		Playing,

		GameOver
	}

	private State currentState = State.None;

	private Coroutine doTurnOnGravity = null;

	private Coroutine doResetSpeed = null;

	private int coins = 0;

	private float gameSpeed = 1;

	private float timerTillEarthQuake = 13;

	private float currentTimerTillEarthQuake = 0;

	public State CurrentState {
		get {
			return currentState;
		}
	}

	public float GameSpeed {
		get {
			return gameSpeed;
		}
	}

	public float TimeTillNextEarthQuake {
		get {
			return currentTimerTillEarthQuake;
		}
	}

	public int Coins {
		get {
			return coins;
		}
	}

	#region EVENTS
	public LevelManagerStateAction OnStateChanged = new LevelManagerStateAction();

	public UnityEvent OnEarthQuake;
	#endregion

	// Use this for initialization
	void Start () {
		currentTimerTillEarthQuake = timerTillEarthQuake;

		StartCoroutine (DoCountTillEarthQuake());

		Time.timeScale = 0f;
		StartCoroutine (DoStartGame());

		Ground.Instance.OnScaledToMinimum.AddListener (() => {
			StartCoroutine (Explode ());
			StartCoroutine(DoEndGame());
		});
	}

	// Update is called once per frame
	void Update () {
		if (currentState == State.GameOver && Input.GetKey (KeyCode.Space)) {
			SceneManager.LoadSceneAsync (0);
			StopAllCoroutines ();
		}
	}
		
	public void SetSpeed (float gameSpeed) {
		this.gameSpeed = gameSpeed;
		ResetSpeed (5);
	}
		
	public void CoinPicked(int value) {
		coins += value;
	}


	public void LowerTimer () {
		currentTimerTillEarthQuake = Mathf.Max (currentTimerTillEarthQuake - timerTillEarthQuake * 0.25f, 0);
	}

	public void UpperTimer () {
		currentTimerTillEarthQuake = timerTillEarthQuake;
	}	

	public void ResetSpeed (float delay = 0) {
		if (doResetSpeed != null) {
			StopCoroutine (doResetSpeed);
			doResetSpeed = null;
		}

		doResetSpeed = StartCoroutine(DoResetSpeed(delay));
	}

	public void TurnOnGravity (float delay = 0) {
		if (doTurnOnGravity != null) {
			StopCoroutine (doTurnOnGravity);
			doTurnOnGravity = null;
		}

		doTurnOnGravity = StartCoroutine (DoTurnOnGravity(delay));
	}

	private IEnumerator DoResetSpeed(float delay) {
		if (delay > 0) {
			yield return new WaitForSeconds (delay);
		}

		this.gameSpeed = 1;
		if (SoundManager.Instance.TickTock != null) {
			SoundManager.Instance.TickTock.Stop ();
		}
	}

	private IEnumerator DoTurnOnGravity(float delay) {
		if (delay > 0) {
			yield return new WaitForSeconds (delay);
		}

		Physics.gravity = new Vector3(0, -9.81f, 0);

		StartCoroutine (DoCountTillEarthQuake());
	}

	private void SetState(State state)
	{
		if (currentState != state) {
			currentState = state;
			if (OnStateChanged != null) {
				OnStateChanged.Invoke (currentState);
			}
		}
	}

	private void Earthquake () {
		if (currentState != State.GameOver) {
			Physics.gravity = new Vector3 (0, 0, 0);
			CameraController.Instance.Shake ();

			if (SoundManager.Instance.Tremor != null) {
				SoundManager.Instance.Tremor.Play ();
			}

			if (OnEarthQuake != null) {
				OnEarthQuake.Invoke ();
			}
		}

		TurnOnGravity (0.5f);
	}

	private IEnumerator DoStartGame () {
		SetState(State.Starting);

		Time.timeScale = 0f;

		yield return new WaitForSecondsRealtime (5);

		SetState(State.Playing);

		Time.timeScale = 1f;

		yield return null;
	}

	private IEnumerator DoEndGame () {
		SetState(State.GameOver);

		yield return null;
	}

	private IEnumerator DoCountTillEarthQuake () {
		while (currentTimerTillEarthQuake > 0) {
			currentTimerTillEarthQuake -= Time.deltaTime * gameSpeed;
				
			if (currentState == State.GameOver) {
				yield break;
			}

			yield return null;
		}

		Earthquake ();

		currentTimerTillEarthQuake = timerTillEarthQuake;

		yield break;
	}

	private IEnumerator Explode () {
		Collider[] colliders = Physics.OverlapSphere (Player.Instance.Top.position, 100);

		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders[i].attachedRigidbody != null) {
				colliders[i].attachedRigidbody.AddExplosionForce (500, Player.Instance.Top.position, 100);
			}
		}

		StartCoroutine (ReturnTimescale ());

		yield return null;
	}

	private IEnumerator ReturnTimescale () {	
		while (Time.timeScale > 0.5f) {
			Time.timeScale = Mathf.Clamp(Time.timeScale - 0.05f, 0.5f, 2f);

			yield return new WaitForEndOfFrame ();
		}

		Time.timeScale = 0.5f;

		for (int i = 0; i < 30; ++i) {
			yield return new WaitForEndOfFrame ();
		}

		while (Time.timeScale < 2) {
			Time.timeScale = Mathf.Clamp(Time.timeScale + 0.1f, 0f, 2f);
			
			yield return new WaitForEndOfFrame ();
		}

		for (int i = 0; i < 30; ++i) {
			yield return new WaitForEndOfFrame ();
		}
		
		while (Time.timeScale > 1) {
			Time.timeScale = Mathf.Clamp(Time.timeScale - 0.1f, 1f, 2f);
			
			yield return new WaitForEndOfFrame ();
		}

		yield return null;
	}
}
