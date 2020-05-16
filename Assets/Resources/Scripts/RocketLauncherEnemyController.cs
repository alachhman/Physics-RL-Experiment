using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherEnemyController : MonoBehaviour {
	private CircleCollider2D detectionRadius;
	public GameObject ammo;
	public int lifeTime;
	private ParticleSystem particles;
	private GameObject activeAmmo;

	// Start is called before the first frame update
	void Start() {
		detectionRadius = gameObject.GetComponent<CircleCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		emitRocket(other);
	}

	private void OnTriggerStay2D(Collider2D other) {
		emitRocket(other);
	}

	private void Update() { }

	void emitRocket(Collider2D other) {
		if (activeAmmo) {
			return;
		}

		if (other.gameObject.CompareTag("Player")) {
			print("Player detected");
			GameObject newAmmo = Instantiate(ammo,
				new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
			activeAmmo = newAmmo;
		}

		StartCoroutine(tearDownMissile(activeAmmo));
	}

	private IEnumerator tearDownMissile(GameObject currMissile) {
		yield return new WaitForSeconds(lifeTime);
		currMissile.GetComponentInChildren<ParticleSystem>().Stop();
		StartCoroutine(FadeTo(0f, 0.5f, currMissile));
	}
	
	IEnumerator FadeTo(float aValue, float aTime, GameObject gameObject)
	{
		float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = gameObject.GetComponent<SpriteRenderer>().color;
			newColor.a = Mathf.Lerp(alpha,aValue,t);
			gameObject.GetComponent<SpriteRenderer>().color = newColor;
			yield return null;
		}
		Destroy(gameObject);
	}
}