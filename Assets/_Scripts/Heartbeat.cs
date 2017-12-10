/*
 * Vince Carpino
 * 2260921
 * carpi111@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark
 *
 * Controls the particle system relating to
 *     the player's heart rate throughout the
 *     game as well as the spotlight above the
 *     player corresponding to their "light".
 */

using System;
using System.Collections;
using UnityEngine;

public class Heartbeat : MonoBehaviour {
    public float HeartRate;
    public float HoldBreathScale;
    public float RecoveryRate;
    public float RingSizeDivisor;
    public float SpotlightDivisor;
    public ParticleSystem HeartBeatPs;
    public Light Spotlight;
    public GameObject LostLightGameOverText;
    public GameObject FearGameOverText;
    public int GameOverTextDelay;

    public GameObject[] RedTickMarks;
    public GameObject GrayTickMarks;

    private readonly Color _red    = new Color (1.000f, 0.266f, 0.266f);
    private readonly Color _yellow = new Color (1.000f, 0.984f, 0.433f);
    private readonly Color _green  = new Color (0.333f, 0.569f, 0.373f);

    private bool IsHoldingBreath;
    private GameObject Player;
    private float MoveSpeed;
    private float SprintSpeed;
    private bool AtMaxHeartRate;
    private bool Dead;
    private bool IsSprinting;

    public uint MinHeartRate    = 12;
    public uint NormalHeartRate = 25;
    public uint MaxHeartRate    = 85;

    private int DeathTimer;

    private AudioSource HeartBeat;

    private LevelController LC;

    private void Start() {
        HeartRate = NormalHeartRate;
        LostLightGameOverText.SetActive(false);
        FearGameOverText.SetActive(false);
        Player = GameObject.FindWithTag("Player");
        MoveSpeed = gameObject.GetComponent<Movement>().moveSpeed;
        SprintSpeed = GetComponent<Movement>().sprintSpeed;

        HeartBeat = GameObject.FindGameObjectWithTag("SM").GetComponent<AudioSource>();

        foreach (var tickMark in RedTickMarks) {
            tickMark.SetActive(false);
        }

        GrayTickMarks.SetActive(false);

        LC = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();

        InvokeRepeating("CountUpDeathTimer", 0.0f, 1.0f);
    }

    private void Update() {
        SetHeartRate();
        SetHeartBeatColor();
        SetHeartRateSpeed();
        SetHeartRateSize();
        SetSpotlightRadius();

        CheckHeartRate();
        CheckSprint();

        HeartBeat.pitch = (float) (0.6 + (HeartRate - 12) * (2.5 - 0.6) / (85 - 12));

        Mathf.Clamp(Player.GetComponent<PlayerController>().EnemyCount, IsSprinting ? 1f : 0f,
            Player.GetComponent<PlayerController>().MaxEnemies);

//        if (IsHoldingBreath) {
//            HoldBreath();
//        } else {
//            Recover();
//        }

//
//        #if !UNITY_ANDROID
//            if (Input.GetKeyDown(KeyCode.Space) && HeartRate > MinHeartRate)
//                IsHoldingBreath = true;
////                HoldBreath();
////            else if (Input.GetKeyUp("Space") && HeartRate < NormalHeartRate)
//            else if (!Input.GetKey(KeyCode.Space) && HeartRate < NormalHeartRate)
//                StopHoldingBreath();
//                Recover();
#if UNITY_ANDROID
            if (IsHoldingBreath)
                HoldBreath();
            else
                Recover();
//                StopHoldingBreath();
        #endif

    }

    public void IncreaseHeartRate(float val) {
        if (HeartRate >= MaxHeartRate) return;

        // WHILE HEART RATE < MAX, INCREASE IT
        HeartRate += val;

        // IF WITHIN 1 OF MAX, AT MAX HEART RATE
        if (HeartRate >= MaxHeartRate - 1) {
            AtMaxHeartRate = true;
        }
    }

    public void DecreaseHeartRate() {
        if (HeartRate >= NormalHeartRate)
            HeartRate -= RecoveryRate;
    }

    public void HoldBreath() {
//        IsHoldingBreath = true;
        gameObject.GetComponent<Movement>().moveSpeed = MoveSpeed / 1.25f;

        if (HeartRate >= MinHeartRate) {
            HeartRate -= HoldBreathScale;

            if (HeartRate < MinHeartRate) {
                LostLightGameOverText.SetActive(true);
//                gameObject.SetActive(false);
//                Time.timeScale = 0.0f;
                StartCoroutine(ShowDeathText(GameOverTextDelay));
//                GetComponent<PlayerController>().enabled = false;
//                GetComponent<Heartbeat>().enabled = false;
//                Dead = true;
            }
        }
    }

//    public void SetIsHoldingBreath(bool val) {
//        IsHoldingBreath = val;
//    }

//    public void StopHoldingBreath() {
//        IsHoldingBreath = false;
//        gameObject.GetComponent<Movement>().moveSpeed = MoveSpeed;
//    }

    public void Recover() {
        IsHoldingBreath = false;
        gameObject.GetComponent<Movement>().moveSpeed = MoveSpeed;

        if (HeartRate >= NormalHeartRate)
            HeartRate -= RecoveryRate;
        else if (HeartRate <= NormalHeartRate)
            HeartRate += RecoveryRate;
    }

    private void SetHeartRate() {
        #if !UNITY_ANDROID
            if (Input.GetKey(KeyCode.Space))
                IsHoldingBreath = true;
    //            if (Input.GetKey(KeyCode.Space) && HeartRate > MinHeartRate)
    //                HoldBreath();
    //            else if (Input.GetKeyUp("Space") && HeartRate < NormalHeartRate)
            else if (!Input.GetKey(KeyCode.Space))
                IsHoldingBreath = false;
//                StopHoldingBreath();
//                Recover();
        #elif UNITY_ANDROID
//            if (IsHoldingBreath)
//                HoldBreath();
//            else
//                StopHoldingBreath();
        #endif

        if (IsHoldingBreath) {
            HoldBreath();
        } else {
            Recover();
        }
    }

    private void SetHeartBeatColor() {
        var main = HeartBeatPs.main;

        if (HeartRate <= 18 || HeartRate >= 71)                                             // 18- OR 71+
            main.startColor = _red;
        else if (HeartRate >= 19 && HeartRate <= 23 || HeartRate >= 27 && HeartRate <= 70)  // 19 - 23 OR 27 - 70
            main.startColor = _yellow;
        else if (HeartRate >= 24 && HeartRate <= 26)                                        // 24 - 26
            main.startColor = _green;
    }

    private void SetHeartRateSpeed() {
        var main = HeartBeatPs.main;
        main.simulationSpeed = HeartRate / NormalHeartRate;
    }

    private void SetHeartRateSize() {
        var main = HeartBeatPs.main;
        main.startSize = HeartRate / RingSizeDivisor;
    }

    private void SetSpotlightRadius() {
        Spotlight.spotAngle = HeartRate / SpotlightDivisor;
    }

    private void CheckHeartRate() {
        if (HeartRate >= MaxHeartRate - 1) return;

        DeathTimer = 0;
        AtMaxHeartRate = false;
        foreach (var tickMark in RedTickMarks) {
            tickMark.SetActive(false);
        }
        GrayTickMarks.SetActive(false);
    }

    private void CountUpDeathTimer() {
        if (!AtMaxHeartRate) return;

        // WHILE AT MAX HEART RATE AND NOT DEAD, INCREASE DEATH TIMER AND TICK MARKS
        if (!Dead) {
            DeathTimer++;
            GrayTickMarks.SetActive(true);
            RedTickMarks[DeathTimer - 1].SetActive(true);
        }

        // IF DEATH TIMER REACHES 10, PLAYER DIES
        if (DeathTimer >= 10) {
            FearGameOverText.SetActive(true);
            GetComponent<PlayerController>().enabled = false;
//            GetComponent<Heartbeat>().enabled = false;
            Dead = true;
            StartCoroutine(ShowDeathText(GameOverTextDelay));
        }
    }

    IEnumerator ShowDeathText(int val) {
        yield return new WaitForSeconds(val);

        LC.FadeEffect();
    }

    // WHILE SPRINTING, INCREASE ENEMY COUNT BY 1
    private void CheckSprint() {
        // IF NO INPUT, SKIP THIS
        if (Math.Abs(Input.GetAxis("Horizontal")) < 1f && Math.Abs(Input.GetAxis("Vertical")) < 1f
            || Input.GetKey(KeyCode.LeftShift) && Math.Abs(Input.GetAxis("Horizontal")) < 1f && Math.Abs(Input.GetAxis("Vertical")) < 1f) return;

        if (Input.GetKey(KeyCode.LeftShift)) {
            GetComponent<Movement>().moveSpeed = SprintSpeed;
            IsSprinting = true;
        } if (Input.GetKeyDown(KeyCode.LeftShift)) {
//            Player.GetComponent<PlayerController>().EnemyCount++;
            Player.GetComponent<PlayerController>().IncreaseEnemyCount(1);
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            GetComponent<Movement>().moveSpeed = MoveSpeed;
            IsSprinting = false;
            Player.GetComponent<PlayerController>().DecreaseEnemyCount(1);
//            if (Player.GetComponent<PlayerController>().EnemyCount > 0)
//                Player.GetComponent<PlayerController>().EnemyCount--;
        }
    }
}
