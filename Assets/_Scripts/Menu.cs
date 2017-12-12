using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;



public class Menu : MonoBehaviour {

	public Canvas MainCanvas;
    public AudioSource audio;
    public string sceneToSwitchTo;

	public void awake() {

	}

	public void loadOn() {
        StartCoroutine(MyCoroutine());
	}

    IEnumerator MyCoroutine()
    {
        audio.Play();

        //Wait until clip finish playing
         yield return new WaitForSeconds (audio.clip.length);

		SceneManager.LoadScene(sceneToSwitchTo);
    }
}
