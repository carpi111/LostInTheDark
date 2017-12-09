using UnityEngine;

public class TriggersManager : MonoBehaviour {

	public uint TriggersCrossed;

	void Update () {
		CheckCurrentTrigger();
	}

	public uint GetCurrentTrigger() {
		return TriggersCrossed;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Enemy"))
            TriggersCrossed++;
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Enemy"))
            TriggersCrossed--;
	}

	private void CheckCurrentTrigger() {
		switch (TriggersCrossed) {
            case 1: // AUDIO
//                print("AUDIO TRIGGER CROSSED");
                break;
            case 2: // VISUAL
//                print("VISUAL TRIGGER CROSSED");
                break;
		}
	}
}
