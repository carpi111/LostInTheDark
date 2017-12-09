/*
 * Akash Arora
 * arora110@mail.chapman.edu
 * CPSC 344-01
 * Lost in the Dark - Beta
 *
 * Initial footprint controller;
 *     most work done in Movement.cs now.
 */

using System.Collections;
using UnityEngine;

public class FootPrint : MonoBehaviour {
    public GameObject thisFoot;
    public GameObject tester;
    private GameObject target;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

    void Start () {
        //gameObject.name = "left";
        //DestroyMe ();
        //thisFoot = GameObject.FindWithTag("left");
//        print ("HI");
        //Debug.Log ("Debug");
        target = GameObject.FindWithTag("Player");
        targetPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        targetRotation = Quaternion.LookRotation (-targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
//        print (targetRotation);
//        print (targetPoint);
//        print (transform.rotation);
        //print ("HI");
        //Debug.Log ("Debug");
    }

    void Update () {

    }

    //Triggers Shield Method
    public void DestroyMe() {
        StartCoroutine(ExecuteAfterTime(3));
    }
    //Helper method for activateShield
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        print (gameObject.name);
        //print (thisFoot);
        //print ("ji");
        //Destroy(thisFoot);
    }
}
