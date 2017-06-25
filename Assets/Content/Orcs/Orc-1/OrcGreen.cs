using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcGreen : MonoBehaviour {
	public float speed = 1;
	public Vector3 MoveBy = Vector3.one;
	Rigidbody2D myBody = null;
	bool isGrounded = false;

	public Animator animator;

	SpriteRenderer sr;
	Vector3 my_pos;
	Vector3 rabbit_pos;
	Vector3 target;
	Vector3 pointA;
    Vector3 pointB;

	public enum Mode {
		GoToA,
		GoToB,
		Attack,
		Die,
		//...
	}
	Mode mode = Mode.GoToA;

	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		this.sr = GetComponent<SpriteRenderer>();
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;
	}

	bool isArrived(Vector3 pos, Vector3 target) {
		pos.z = 0;
		target.z = 0;
		return Vector3.Distance(pos, target) < 0.02f;
	}

	void Update () {

	}

	float getDirection() {
		rabbit_pos = HeroControll.lastRabit.transform.position;
        my_pos = this.transform.position;
		if (rabbit_pos.x >= Mathf.Min (pointA.x, pointB.x) && rabbit_pos.x <= Mathf.Max (pointA.x, pointB.x))
			mode = Mode.Attack;
		else {
				if(my_pos.x >= pointB.x || Mathf.Abs(my_pos.x-pointB.x) > Mathf.Abs(my_pos.x-pointA.x))
					if (mode == Mode.Attack)
						this.mode = Mode.GoToA;
				if(my_pos.x <= pointA.x || Mathf.Abs(my_pos.x-pointB.x) <= Mathf.Abs(my_pos.x-pointA.x))
					if (mode == Mode.Attack)
						this.mode = Mode.GoToB;
		}
		if(mode == Mode.Attack) {
			if(my_pos.x < rabbit_pos.x)
				return 1;
			else
				return -1;
		}
		if(mode == Mode.GoToB) {
			if(my_pos.x >= pointB.x)
				mode = Mode.GoToA;
		} else if(mode == Mode.GoToA) {
			if(my_pos.x <= pointA.x)
				mode = Mode.GoToB;
		}
		if(mode == Mode.GoToB) {
			if(my_pos.x <= pointB.x)
				return 1;
			else
				return -1;
		} else if(mode == Mode.GoToA) {
			if(my_pos.x >= pointA.x)
				return -1;
			else
				return 1;
		}
		return 0;
	}

	void FixedUpdate () {
		if (mode == Mode.Die) return;
		float value = this.getDirection ();
		if (Mathf.Abs(value) > 0) {
			if (mode != Mode.Die) {
				animator.SetBool("walk", true);
				Vector2 vel = myBody.velocity;
				vel.x = value * speed;
				if (mode == Mode.Attack)
					vel.x *= 2;
				myBody.velocity = vel;
			}
		}
		else animator.SetBool("walk", false);

		if (value > 0) sr.flipX = true;
		else if (value < 0) sr.flipX = false;

		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer("Ground");

		//if (death)
		//	die ();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(mode != Mode.Die) {
			StartCoroutine(kill());
			HeroControll rabit = collider.GetComponent<HeroControll>();
			if (rabit != null)
				rabit.triggerDeath();
			
		}
		
	}

	public IEnumerator kill() {
		this.animator.SetTrigger("attack");
		yield return new WaitForSeconds(1.5f);
		this.animator.SetBool("walk", true);
	}
	
 
	public void triggerDeath(){
		mode = Mode.Die;
		StartCoroutine(die());
	}

	IEnumerator die() {
		animator.SetBool ("death", true);
		yield return new WaitForSeconds(0.5f);
		animator.SetBool ("death", false);
		Destroy(this.gameObject);

	}
/**/
}