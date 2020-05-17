using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RocketLauncherEnemyController : MonoBehaviour {
	private CircleCollider2D detectionRadius;
	public GameObject ammo;
	public int lifeTime;
	private ParticleSystem particles;
	private GameObject activeAmmo;
	private bool isAlive;
	public string entityName;
	public int defaultHp;
	public int defaultAtk;
	public int defaultDef;
	public Entity entity;
	public Image HPBar;
	private GameObject goldIndicator;
	private bool hasGivenReward;

	// Start is called before the first frame update
	void Start() {
		detectionRadius = gameObject.GetComponent<CircleCollider2D>();
		entity = new Entity(entityName, defaultHp, defaultAtk, defaultDef);
		isAlive = true;
		goldIndicator = GameObject.Find("GoldText");
		hasGivenReward = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		emitRocket(other);
	}

	private void OnTriggerStay2D(Collider2D other) {
		emitRocket(other);
	}

	private void Update() {
		HPBar.fillAmount = (float)entity.currHP / entity.HP;
		if (entity.currHP < 0) {
			isAlive = false;
			if (!hasGivenReward) {
				goldIndicator.GetComponent<Text>().text =
                				(int.Parse(goldIndicator.GetComponent<Text>().text) + Random.Range(20, 50)).ToString();
				hasGivenReward = true;
			}
			StartCoroutine(tearDownMissile(activeAmmo, 0));
			StartCoroutine(FadeTo(0f, 1f, gameObject));
		}
	}

	void emitRocket(Collider2D other) {
		if (activeAmmo) {
			return;
		}
		if (!isAlive) {
			return;
		}
		if (other.gameObject.CompareTag("Player")) {
			print("Player detected");
			GameObject newAmmo = Instantiate(ammo,
				new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
			activeAmmo = newAmmo;
		}

		StartCoroutine(tearDownMissile(activeAmmo, lifeTime));
	}

	private IEnumerator tearDownMissile(GameObject currMissile, int lifetime) {
		yield return new WaitForSeconds(lifetime);
		currMissile.GetComponentInChildren<ParticleSystem>().Stop();
		StartCoroutine(FadeTo(0f, 0.2f, currMissile));
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