﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingTrigger : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		print("MONSTER SOUND");
	}
}
