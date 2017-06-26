using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {
	protected override void OnRabitHit (HeroControll rabit)
	{
		rabit.makeSuper();
		this.CollectedHide ();
	}
}
