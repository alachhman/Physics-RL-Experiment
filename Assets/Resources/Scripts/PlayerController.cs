﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {
	public Camera mainCamera;
	public Rigidbody2D rigidBody;
	public string entityName;
	public int defaultHp;
	public int defaultAtk;
	public int defaultDef;
	public float maxSpeed = 10f;
	public Text playerGui;
	public Text HpOut;
	public Image HpBar;
	public GameObject damageNumber;
	private Animator animator;

	private Vector2 directionOfTravel = Vector3.zero;
	private float rotationSpeed = 1f;
	private float oldPosition = 0.0f;

	private RaycastHit hit;
	private float dist = 10f;
	private Vector3 dir = new Vector3(0, -1, 0);

	private Vector2 start;
	private Vector2 end;
	private Vector2 force;

	private Entity entity;

	void Start() {
		start = new Vector2();
		end = new Vector2();
		force = new Vector2();
		rigidBody = GetComponent<Rigidbody2D>();
		oldPosition = transform.position.x;
		entity = new Entity(entityName, defaultHp, defaultAtk, defaultDef);
		animator = gameObject.GetComponent<Animator>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) start = GetCurrentMousePosition().GetValueOrDefault();
		else if (Input.GetMouseButtonUp(0)) {
			StartCoroutine(waitForAnimationFinish());
			end = GetCurrentMousePosition().GetValueOrDefault();
			force = end - start;
			// print(force.magnitude);
			rigidBody.AddForce(-(force * (10000 * Time.deltaTime)));
			DrawLine(start, end, Color.magenta);
		}

		directionOfTravel = rigidBody.velocity.normalized;

		// if (rigidBody.velocity.magnitude > 2) {
		// 	float angle = Mathf.Atan2(directionOfTravel.y, directionOfTravel.x) * Mathf.Rad2Deg;
		// 	transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		// }

		if (oldPosition < transform.position.x) {
			transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
		}
		else if (oldPosition > transform.position.x) {
			transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
		}

		oldPosition = transform.position.x;

		if (rigidBody.velocity.magnitude > maxSpeed) {
			rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);
		}

		playerGui.text = entity.name + "\n=============\n" + entity.statsToString();
		HpOut.text = "HP: " + entity.currHP + " / " + entity.HP;
		HpBar.fillAmount = (float) entity.currHP / entity.HP;

		if (entity.currHP == 0) {
			entity.currHP = 100;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		print("Collision Detected");
		if (other.gameObject.CompareTag("Hurt")) {
			takeDamage(Random.Range(1, 100), Color.red);
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Enemy")) {
			dealDamage(other.gameObject, (Mathf.RoundToInt(rigidBody.velocity.magnitude * 2) + entity.currATK),
				Color.yellow);
		}
	}

	void DrawLine(Vector2 start, Vector2 end, Color color, float duration = 1f) {
		float PercentHead = 0.2f;
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.startWidth = 0.1f;
		lr.endWidth = 0f;
		lr.material.color = color;
		lr.startColor = Color.blue;
		lr.endColor = Color.red;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		lr.sortingOrder = 0;
		Destroy(myLine, duration);
	}

	private Vector3? GetCurrentMousePosition() {
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		var plane = new Plane(Vector3.forward, Vector3.zero);

		float rayDistance;
		if (plane.Raycast(ray, out rayDistance)) {
			return ray.GetPoint(rayDistance);
		}

		return null;
	}

	void takeDamage(int damage, Color color) {
		entity.currHP -= damage;
		GameObject dpsOut = Instantiate(damageNumber,
			new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
				transform.position.y + Random.Range(-0.5f, 0.5f), transform.position.z), transform.rotation);
		dpsOut.transform.localRotation = Quaternion.identity;
		dpsOut.GetComponent<Text>().text = damage.ToString();
		dpsOut.GetComponent<Text>().color = color;
	}

	void dealDamage(GameObject enemy, int damage, Color color) {
		enemy.GetComponent<RocketLauncherEnemyController>().entity.currHP -= damage;
		GameObject dpsOut = Instantiate(damageNumber,
			new Vector3(enemy.transform.position.x + Random.Range(-0.5f, 0.5f),
				enemy.transform.position.y + Random.Range(-0.5f, 0.5f), enemy.transform.position.z + 10),
			enemy.transform.rotation);
		dpsOut.transform.localRotation = Quaternion.identity;
		dpsOut.GetComponent<Text>().text = damage.ToString();
		dpsOut.GetComponent<Text>().color = color;
	}

	IEnumerator waitForAnimationFinish() {
		animator.SetBool("attackPerformed", true);
		yield return new WaitForSeconds(2);
		animator.SetBool("attackPerformed", false);
	}
}