using UnityEngine;

public class HomingProjectileController : MonoBehaviour {
	[SerializeField] Transform Target;
	[SerializeField] float MoveSpeedFloor = 1300f;
	[SerializeField] float MoveSpeedCieling = 1500f;
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
	}

	// Update is called once per frame
	void Update() {
		rb.velocity = transform.up * MoveSpeed * Time.deltaTime;

		Vector3 targetVector = Target.position - transform.position;

		float rotatingIndex = Vector3.Cross(targetVector, transform.up).z;

		rb.angularVelocity = -1 * rotatingIndex * RotateSpeed * Time.deltaTime;
	}
}