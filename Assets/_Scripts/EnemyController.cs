/*
 * Vince Carpino
 * 2260921
 * carpi111@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark - Beta
 *
 * Place on enemy object to have them
 *     continuously face the player.
 */

using UnityEngine;

public class EnemyController : MonoBehaviour {
    public GameObject Player;

    private void Update() {
        transform.LookAt (Player.transform);
    }
}
