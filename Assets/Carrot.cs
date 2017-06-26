using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Collectable {
	
	public float speed = 3f;
	public float life = 3f;

	float direction = 1f;

	Vector3 moveBy;
	SpriteRenderer sr;
	
	void Start() {
		moveBy = new Vector3();
		moveBy.x = speed;
		moveBy.y = 0.5f;
		moveBy.z = 0f;
		sr = GetComponent<SpriteRenderer>();
		StartCoroutine (destroyLater ());
	}

	public void FixedUpdate() {
        if (direction != 0) {
            if (direction < 0)
				sr.flipX = true;
			else if (direction > 0)
				sr.flipX = false;
			
            if (Mathf.Abs(direction) > 0)
				this.transform.position += moveBy * direction * Time.deltaTime;
        }
    }

	public void launch(float direction) {
		this.direction = direction;
	}

	protected override void OnRabitHit(HeroControll rabit) {
		if (rabit.red)
			return;
		if (rabit.super)
			rabit.makeSmall();
		else rabit.triggerDeath();
		this.CollectedHide();
	}

	IEnumerator destroyLater() {
		yield return new WaitForSeconds (life);
		Destroy (this.gameObject);
	}
}