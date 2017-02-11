using UnityEngine;
using System.Collections;

public class SlowTime : PickUp {
	protected override void PickedUp ()
	{	
		LevelManager.Instance.SetSpeed (0.1f);
		
		if (SoundManager.Instance.TickTock != null) {
			SoundManager.Instance.TickTock.Play ();
		}
	}

	protected override void CalculatePosition ()
	{
		Vector3 pos = transform.position;
		pos.y = -100;

		ray.origin = pos;
		ray.direction = Vector3.up;

		if (Ground.Instance.MyCollider.Raycast (ray, out info, 1000)) {
			pos.y = info.point.y - height;
		}

		transform.position = pos;
	}
}
