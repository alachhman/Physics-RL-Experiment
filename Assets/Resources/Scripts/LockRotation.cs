﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {
	void Update() {
		gameObject.transform.rotation = Quaternion.identity;
	}
}