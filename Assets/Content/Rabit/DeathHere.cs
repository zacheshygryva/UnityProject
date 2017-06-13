using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHere : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {

		HeroControll rabbit = collider.GetComponent<HeroControll> ();

		if(rabbit != null) {
			LevelController.current.onRabbitDeath(rabbit);
		}
	}
}
