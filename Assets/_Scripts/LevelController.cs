using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public int LevelIndex;

	public Image Black;
	public Animator Anim;

	public bool ReachedEnd;
	private Scene LoadedLevel;

	private void Start() {
		LoadedLevel = SceneManager.GetActiveScene();
		LevelIndex = LoadedLevel.buildIndex;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			ReachedEnd = true;
			StartCoroutine(Fading());
		}
	}

	public void FadeEffect(int sceneIndex) {
//		LevelIndex = sceneIndex;
		StartCoroutine(Fading());
	}

	private IEnumerator Fading() {
		Anim.SetBool("Fade", true);
		yield return new WaitUntil(() => Black.color.a == 1);
		if (ReachedEnd) {
			SceneManager.LoadScene(LevelIndex + 1);
		} else {
			SceneManager.LoadScene(LevelIndex);
		}
	}
}
