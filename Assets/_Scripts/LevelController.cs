using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public int NextLevelIndex;

	public Image Black;
	public Animator Anim;

	private bool ReachedEnd;
	private int LoadedLevelIndex;

	private void Start() {
		LoadedLevelIndex  = SceneManager.GetActiveScene().buildIndex;
	}

	public void FadeEffect() {
		StartCoroutine(Fading());
	}

	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player")) return;

		ReachedEnd = true;
		StartCoroutine(Fading());
	}

	private IEnumerator Fading() {
		Anim.SetBool("Fade", true);
		yield return new WaitUntil(() => Black.color.a == 1);
		if (ReachedEnd) {
			SceneManager.LoadScene(NextLevelIndex);
		} else {
			SceneManager.LoadScene(LoadedLevelIndex);
		}
	}
}
