﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Collectable {
	protected override void OnRabitHit (HeroControll rabit)
	{
		//Level.current.addCoins (1);
		this.CollectedHide ();
	}
}

