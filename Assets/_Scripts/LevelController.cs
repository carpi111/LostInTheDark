using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public int Index;
	public string LevelName;

	public Image Black;
	public Animator Anim;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(Fading());
		}
	}

	public void FadeEffect(int sceneIndex) {
		Index = sceneIndex;
		StartCoroutine(Fading());
	}

	private IEnumerator Fading() {
		Anim.SetBool("Fade", true);
		yield return new WaitUntil(() => Black.color.a == 1);
		SceneManager.LoadScene(LevelName);
	}
}
