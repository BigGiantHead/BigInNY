using UnityEngine;
using System.Collections;

public class Stabilizer : PickUp {
	protected override void PickedUp ()
	{
		LevelManager.Instance.UpperTimer ();
		LevelManager.Instance.CoinPicked (1);


		if (SoundManager.Instance.CashRegister != null) {
			SoundManager.Instance.CashRegister.Play ();
		}
	}
}
