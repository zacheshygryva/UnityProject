using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControll : MonoBehaviour {
	public float speed = 1;
	Rigidbody2D myBody = null;
	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;
	public bool super = false;
	public bool red = false;
	public float redTime = 0f;
	public bool death = false;
	public float deathTime = 0f;
	public Animator animator;

	SpriteRenderer sr;
	Transform heroParent = null;

	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		LevelController.current.setStartPosition(transform.position);
		this.heroParent = this.transform.parent;
		this.sr = GetComponent<SpriteRenderer>();
	}


	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate () {
		float value = Input.GetAxis("Horizontal");
		if (!super)
			this.makeSmall ();
		if (Mathf.Abs(value) > 0) {
			animator.SetBool("run", true);
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		}
		else animator.SetBool("run", false);

		if (value < 0) sr.flipX = true;
		else if (value > 0) sr.flipX = false;

		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer("Ground");

		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if (hit) isGrounded = true;
		else isGrounded = false;
		if(hit) {
			if(hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null)
				SetNewParent(this.transform, hit.transform);
		} else SetNewParent(this.transform, this.heroParent);

		if (Input.GetButtonDown("Jump") && isGrounded) this.JumpActive = true;
		if (this.JumpActive) {
			if (Input.GetButton("Jump")) {
				this.JumpTime += Time.deltaTime;
				if (this.JumpTime < this.MaxJumpTime) {
					Vector2 vel = myBody.velocity;
					vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
					myBody.velocity = vel;
				}
			}
			else {
				this.JumpActive = false;
				this.JumpTime = 0;
			}
		}
		if (this.isGrounded)
			animator.SetBool("jump", false);
		else
			animator.SetBool("jump", true);

		if (red) {
			redTime -= Time.deltaTime;
			sr.color = Color.red;
			if (redTime <= 0)
				red = false;
				
		} else
			sr.color = Color.white;
	}

	public void makeSuper() {
		if (!super) {
			this.transform.localScale = Vector3.one * 1.5f;
			this.super = true;
		}
	}

	public void makeRed() {
		makeSmall ();
		redTime = 4f;
		red = true;
	}
	public void makeSmall() {
		this.transform.localScale = Vector3.one;
		super = false;
	}

	public void triggerDeath(){
		StartCoroutine(die());
	}

	IEnumerator die() {
		animator.SetBool ("death", true);
		yield return new WaitForSeconds(0.4f);
		animator.SetBool ("death", false);
		death = false;
		LevelController.current.onRabbitDeath(this);
		
	}

	static void SetNewParent(Transform obj, Transform new_parent) {
		if(obj.transform.parent != new_parent) {
			Vector3 pos = obj.transform.position;
			obj.transform.parent = new_parent;
			obj.transform.position = pos;
		}
	}

	public static HeroControll lastRabit = null;
	void Awake() {
		lastRabit = this;
	}
}
