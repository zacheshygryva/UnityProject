using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	protected virtual void OnRabitHit(HeroControll rabit) {}

	void OnTriggerEnter2D(Collider2D collider) {
		
		HeroControll rabit = collider.GetComponent<HeroControll>();
		if (rabit != null)
			this.OnRabitHit (rabit);

	}

	public void CollectedHide() {
		Destroy(this.gameObject);
	}
}