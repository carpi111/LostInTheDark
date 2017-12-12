using UnityEngine;

public class GameManager : MonoBehaviour {

	public ParticleSystem HeartBeatPs;
	public Light Spotlight;
	public GameObject LostLightGOText;
	public GameObject FearGOText;
	public GameObject[] RedTickMarks;
	public GameObject GrayTickMarks;

	public GameObject Player;
	public LevelController LC;

	void Start () {
		Player = GameObject.FindWithTag("Player");
		LC = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
	}
}
