﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Rigidbody2D rb;
	public Transform ground;
	public ScoresController meuScore;
	public AudioSource sPulo;
	public AudioSource sMoeda;
	private int stados = 0;
	private Animator anim;
	private float axis;
	public float velocidade = 10;
	public float forcaPulo = 1000;
	private bool isGrounded;
	private bool isRight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		isRight = true;
		rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 3;
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.Linecast (this.transform.position, ground.position, 1<<LayerMask.NameToLayer("chao"));
		axis = Input.GetAxisRaw ("Horizontal");

		if (axis == 0 && isGrounded) {
			stados = 0;
		} 
		if (axis > 0) {
			transform.Translate (Vector2.right * velocidade * Time.deltaTime);
			flip (1);
		} else if (axis < 0) {
			transform.Translate (Vector2.left * velocidade * Time.deltaTime);
			flip (-1);
		}

		if (Input.GetButtonDown ("Jump") && isGrounded) {
			sPulo.Play ();
			stados = 3;
			rb.AddForce (transform.up * forcaPulo);
		}

		animar ();
	}

	void animar(){
		anim.SetInteger ("state", stados);
	}

	void flip(int i){
		if (isGrounded) {
			stados = 2;
			if (i > 0 && !isRight) {
				isRight = !isRight;
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
			} else if (i < 0 && isRight) {
				isRight = !isRight;
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "moeda") {
			Destroy (col.gameObject);
			sMoeda.Play ();
			meuScore.AumentaScore (5);
		}
		if (col.gameObject.tag == "Finish") {
			if (transform.gameObject.tag == "Player") {
				PlayerPrefs.SetInt ("Score", meuScore.GetScore());
				SceneManager.LoadScene ("Score", LoadSceneMode.Single);
			}
		}
	}
}