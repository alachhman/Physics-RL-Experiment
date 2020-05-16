using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomingProjectileController : MonoBehaviour {
	private Transform Target;
	public ParticleSystem deathAnim;
	[SerializeField] float MoveSpeedFloor = 1400f;
	[SerializeField] float MoveSpeedCieling = 1900f;
	[SerializeField] float RotateSpeedfloor = 8000f;
	[SerializeField] float RotateSpeedCieling = 17000f;
	private float MoveSpeed;
	private float RotateSpeed;
	Rigidbody2D rb;

	// Use this for initialization
	void Start() {
		MoveSpeed = Random.Range(MoveSpeedFloor, MoveSpeedCieling);
		RotateSpeed = Random.Range(RotateSpeedfloor, RotateSpeedCieling);
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		Target = GameObject.Find("Player").transform;
	}

	// Update is called once per frame
	void Update() {
		rb.velocity = transform.up * (MoveSpeed * Time.deltaTime);

		Vector3 targetVector = Target.position - transform.position;

		float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;

		rb.angularVelocity = -1 * rotatingIndex * RotateSpeed * Time.deltaTime;
	}

	private void OnDisable() {
		Instantiate(deathAnim, gameObject.transform);
	}
	
	
}