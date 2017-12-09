/*
 * Vince Carpino
 * 2260921
 * carpi111@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark - Beta
 *
 * Attach to object (camera, spotlight) to maintain offset
 *     between the player and that object.
 */

using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public GameObject Player;

    private Vector3 _offset;

    private void Start ()
    {
        _offset = transform.position - Player.transform.position;
    }

    // Called once per frame after all calcs have been done
    private void LateUpdate ()
    {
        transform.position = Player.transform.position + _offset;
    }
}
