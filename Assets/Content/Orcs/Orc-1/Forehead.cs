using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forehead : MonoBehaviour {

	public OrcGreen orc = null;

	void OnTriggerEnter2D(Collider2D collider) {
		
		HeroControll rabit = collider.GetComponent<HeroControll>();
		if (rabit != null)
			this.orc.triggerDeath();

	}
}
