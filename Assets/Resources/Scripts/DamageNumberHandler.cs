using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberHandler : MonoBehaviour {
	public float scroll = 0.05f;
	public float duration = 1.5f;
	public float alpha;
	public Text display;

	void Start() {
		display = gameObject.GetComponent<Text>();
		
		alpha = 1f;
	}

	void Update() {
		if (alpha > 0) {
			display.transform.Translate(Vector3.up * scroll * Time.deltaTime);
			alpha -= Time.deltaTime / duration;
			Color temp = display.color;
			temp.a = alpha;
			display.color = temp;
		}
		else {
			Destroy(gameObject); // text vanished - destroy itself
		}
	}
}