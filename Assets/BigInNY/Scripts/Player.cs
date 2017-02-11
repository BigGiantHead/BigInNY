using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Player : SingletonMonoBehaviour<Player> {
	private float timeSinceJump = float.MinValue;

	private Ray ray;
	
	private RaycastHit info;

	private float maxy = 0;

	private float speed = 5f;

	[SerializeField]
	private float walkingSpeed = 5f;

	[SerializeField]
	private float runningSpeed = 8f;

	[SerializeField]
	private Transform bottom;

	[SerializeField]
	private Transform top;

	[SerializeField]
	private Rigidbody topRigidbody = null;

	public Transform Bottom {
		get {
			return bottom;
		}
	}

	public Transform Top {
		get {
			return top;
		}
	}

	#region EVENTS
	public UnityEvent OnDucking;

	public UnityEvent OnJumping;

	public UnityEvent OnRunning;
	#endregion

	// Use this for initialization
	void Start () {
		LevelManager.Instance.OnStateChanged.AddListener ((state) => {
			if (state == LevelManager.State.GameOver) {
				gameObject.SetActive(false);
			}
		});
	}

	void Update () {
		ray = new Ray (top.position, Vector3.down);
		
		if (Ground.Instance.MyCollider.Raycast (ray, out info, 1000)) {
			maxy = info.point.y + 4f;
		}

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			speed = runningSpeed;

			if (OnRunning != null) {
				OnRunning.Invoke ();
			}
		} else {
			speed = walkingSpeed;
		}

		//duck
		if (Input.GetKey (KeyCode.S)) {
			if (top.localScale.y != 0.4f) {
				top.position -= new Vector3(0, 0.2f, 0);

				if (OnDucking != null) {
					OnDucking.Invoke ();
				}
			}
			top.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
		} else {
			if (top.localScale.y != 0.8f) {
				top.position += new Vector3(0, 0.2f, 0);
			}
			top.localScale = new Vector3 (0.4f, 0.8f, 0.4f);
		}

		//move left
		if (Input.GetKey (KeyCode.A)) {
			top.position += Vector3.left * speed * Time.deltaTime;
		}

		//move right
		if (Input.GetKey (KeyCode.D)) {
			top.position -= Vector3.left * speed * Time.deltaTime;
		}

		//jump
		if (Input.GetKeyDown (KeyCode.W) && Time.timeSinceLevelLoad - timeSinceJump > 0.85f) {
			topRigidbody.AddForce(new Vector3(0, 9.81f * 0.75f, 0), ForceMode.VelocityChange);
			if (OnJumping != null) {
				OnJumping.Invoke ();
			}

			timeSinceJump = Time.timeSinceLevelLoad;
		}

		//clamp position
		Vector3 position = top.position;
		
		if (Time.timeScale > 0) {
			position.y = Mathf.Clamp (position.y, 0, maxy);
			top.position = position;
		}

		position.x = Mathf.Clamp (position.x, -100, 100);
		
		top.position = position;
		
		Vector3 pos = top.position;
		pos.y = -pos.y;

		//update bottom position and scale
		bottom.position = pos;
		bottom.localScale = top.localScale;
	}
}
