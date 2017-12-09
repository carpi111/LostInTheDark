/*
 * Vince Carpino
 * 2260921
 * carpi111@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark - Beta
 *
 * Controls player movement and enemy count.
 */

using System.Xml.Schema;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public float Speed;
    public int EnemyCount;
    public int MaxEnemies;
    public float FearRate;
    public float ScareStrength;

    private Rigidbody _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
//        Mathf.Clamp(EnemyCount, 0f, MaxEnemies);
        CountEnemies();
    }

    private void FixedUpdate() {
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveVertical   = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        _rb.AddForce(movement * Speed);
    }

    public void IncreaseEnemyCount(int val) {
        if (EnemyCount + val <= MaxEnemies)
            EnemyCount += val;
    }

    public void DecreaseEnemyCount(int val) {
        if (EnemyCount - val >= 0)
            EnemyCount -= val;
    }

    private void OnTriggerEnter(Collider other) {
//        if (other.CompareTag("Enemy"))
//            EnemyCount++;
    }

    private void OnTriggerExit(Collider other) {
//        if (other.CompareTag("Enemy"))
//            EnemyCount--;
    }

    private void CountEnemies() {
        if (EnemyCount <= MaxEnemies)
            FearRate = ScareStrength * EnemyCount;

        if (EnemyCount >= 1)
            GetComponent<Heartbeat>().IncreaseHeartRate(FearRate);
        else
            GetComponent<Heartbeat>().Recover();
    }
}
