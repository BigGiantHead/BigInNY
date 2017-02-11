using UnityEngine;
using System.Collections;

public class SpeachBubbleText : MonoBehaviour {
	private TextMesh text = null;

	private string[] strings = new string[] {
		"#$%!",
		"&@%!",
		"#*^!",
		"%$!",
		"###!",
	};

	void OnEnable () {
		if (text == null) {
			text = gameObject.GetComponent<TextMesh>();
		}

		text.text = strings[Random.Range(0, strings.Length)];
	}

	// Use this for initialization
	void Start () {
	
	}
}
