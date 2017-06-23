using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable {
	protected override void OnRabitHit (HeroControll rabit)
	{
		if(!rabit.red){
			if (rabit.super) {
				rabit.makeRed();

			} else 
				rabit.triggerDeath (); 
		}

		this.CollectedHide ();
	}
}

