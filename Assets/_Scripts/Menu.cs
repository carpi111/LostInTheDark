/*
 * Akash Arora & Bobby Kain
 * arora110@mail.chapman.edu & kain102@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark - Beta
 *
 * Loads tutorial level when Play button is pressed.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public Canvas MainCanvas;

	public void awake() {

	}

	public void loadOn() {
		SceneManager.LoadScene("Tutorial");
	}
}
