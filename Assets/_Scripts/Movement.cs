using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Movement : MonoBehaviour {
	public GameObject leftFootPrint;
	public GameObject rightFootPrint;
	//Have to create another placeholder gameObject to instantiate with, or else
	//the destroy method will delete the prefab itself in the assets folder
	private GameObject tempLeft;
	private GameObject tempRight;
	//These placeholders are specifically for the feet dropped when you stand still
	//Since they must be destroyed after a player moves again, and not after a set time, like when player moves
	private GameObject tempLeft2;
	private GameObject tempRight2;
	//Counter to limit stand still feet drops to 2
	private int stillFeet;

	//I need to make this global, because when I stand still, I need to grab the last angle the feet were at.
	private float finalAngle;

	private Vector3 lastPosition;
	public float moveSpeed;
	public float sprintSpeed;
	public float HoldingBreathSpeed;
	private bool onGround;

	//Is used to make sure that the foot doesn't spam drop. If it
	private bool isDropping;
	private bool lOrR;

	public bool IsEnemy;

	private Vector3 currentPosition;
	private Vector3 prevPosition;

	AudioSource audio;

	void Start () {
//		audio = GetComponent<AudioSource>();
//		audio.Play();
		lastPosition = transform.position;
        prevPosition = Vector3.zero;
	}

	void Update () {
        currentPosition = gameObject.transform.position;
//		if(Input.GetButton("Vertical")){
//			if(!audio.isPlaying) {
//				audio.Play();
//			}
//		} else {
//			audio.Stop();
//		}
//		if(Input.GetButton("Horizontal")){
//			if(!audio.isPlaying) {
//				audio.Play();
//			}
//		} else {
//			audio.Stop();
//		}

		//Basic AWSD character movement
//		var x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
//		var z = Input.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
		if (!IsEnemy) {
			var x = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
			var z = CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
			Vector3 temp = transform.position;
			transform.Translate(x, 0f, z);
//		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.S)) {
//                if (CrossPlatformInputManager.GetAxis("Horizontal") || CrossPlatformInputManager.GetAxis("Vertical")) {
			if (x != 0.0f || z != 0.0f) {
				//If you just started moving again, make sure you delete those special footprints
				//that were instantiated when you decided to stand still.
				if (stillFeet == 2) {
					//These two lines, probably can be deleted.
					Destroy(tempLeft);
					Destroy(tempRight);
					//Destroy the still feet in an order that makes sense according to last foot step taken
					if (lOrR == false) {
						Destroy(tempLeft2, .50f);
						Destroy(tempRight2, 1.0f);
					} else {
						Destroy(tempLeft2, 1.0f);
						Destroy(tempRight2, .50f);
					}
					stillFeet = 0;
				}
				startDrop(isDropping);
			} else {
				standStillDrop();
			}
		} else { // ENEMY

			if (!(prevPosition == currentPosition)) {
				//If you just started moving again, make sure you delete those special footprints
				//that were instantiated when you decided to stand still.
				if (stillFeet == 2) {
					//These two lines, probably can be deleted.
					Destroy(tempLeft);
					Destroy(tempRight);
					//Destroy the still feet in an order that makes sense according to last foot step taken
					if (lOrR == false) {
						Destroy(tempLeft2, .50f);
						Destroy(tempRight2, 1.0f);
					} else {
						Destroy(tempLeft2, 1.0f);
						Destroy(tempRight2, .50f);
					}
					stillFeet = 0;
				}
				startDrop(isDropping);
			} else {
				standStillDrop();
			}
		}

		prevPosition = currentPosition;
	}

	public void dropFoot (bool flag, float angle) {
		float unitAngle = angle / 180;
//		print (unitAngle);
		//unitAngle is between (-1 and 1). I split this unitAngle into quadrants repersenting
		//the NSEW direction the player moves, and adjust the spacing between each foot so that
		//the player doesn't walk with mixmatched left/right feet.
		if (flag == false) {
			//LEFT FOOT DROP

			// if (Player is facing relatively forwards)
			// else if (player if facing relatively to the right)
			// else if (player is facing relatively to the left)
			// else if (player is facing relatively backwards)
			if (Mathf.Abs(unitAngle) < .25) {
				tempLeft = Instantiate (leftFootPrint, transform.position - (transform.right * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle > .25 && unitAngle < .75) {
				tempLeft = Instantiate (leftFootPrint, transform.position - (transform.forward * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle < -.25 && unitAngle > -.75) {
				tempLeft = Instantiate (leftFootPrint, transform.position - (transform.forward * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (Mathf.Abs(unitAngle) > .75) {
				tempLeft = Instantiate (leftFootPrint, transform.position - (transform.right * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			}
			//Drop right foot next time
			lOrR = true;
			Destroy (tempLeft, 1.25f);
		} else if (flag == true) {
			//RIGHT FOOT DROP
			if (Mathf.Abs(unitAngle) < .25) {
				tempRight = Instantiate (rightFootPrint, transform.position + (transform.right * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle > .25 && unitAngle < .75) {
				tempRight = Instantiate (rightFootPrint, transform.position + (transform.forward * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle < -.25 && unitAngle > -.75) {
				tempRight = Instantiate (rightFootPrint, transform.position + (transform.forward * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (Mathf.Abs(unitAngle) > .75) {
				tempRight = Instantiate (rightFootPrint, transform.position + (transform.right * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			}
			//Drop left foot next time
			lOrR = false;
			Destroy (tempRight, 1.25f);
		}
	}

	//Drops feet, but pauses in between footsteps (controlled by isDropping flag activated on/off in its helper method)
	public void startDrop(bool flag) {
		//Hey if you dropped the last foot (after you waited a sec), the isDropping will be turned false, and you can
		//drop the next foot
		if (flag == false) {
			isDropping = true;
			StartCoroutine (ExecuteAfterTime (0.5f));
		}
	}

	//Helper method for startDrop
	IEnumerator ExecuteAfterTime(float time)
	{
		//Stores the last position of the player, used to compare with
		//player's position after 1 second to determine direction of footsteps
		lastPosition = transform.position;
		//it seems that whenever player has z component that is positive, the feet fuck up...so
		//i tried to always keep it negative, but that didn't work.

		//lastPosition.z = -Mathf.Abs(lastPosition.z);
		//Wait one second
		yield return new WaitForSeconds(time);

		//Used to calculate angle
		var newPosition = transform.position;
		//newPosition.z = -Mathf.Abs(newPosition.z);

		var dir = newPosition - lastPosition;
		//dir.z = -Mathf.Abs (dir.z);

		//Returns in degrees 0 to 180, PROBLEM, we need to know full 360.
		float degree = Vector3.Angle(dir, transform.forward);

		//To get the sign of the angle (get -180 to 180) we do this:
		Vector3 referenceRight = Vector3.Cross(Vector3.up, lastPosition);
		float sign = Mathf.Sign(Vector3.Dot(newPosition, referenceRight));
//		print (sign);

		//Bamzo...
		finalAngle = sign * degree;

		dropFoot (lOrR, finalAngle);

		//Okay, you're not dropping the foot anymore, you just DROPPED it. So, let's send the next foot in.
		isDropping = false;
	}

	// THESE NEXT THREE FUNCTIONS HANDLE THE SPECIAL CASE OF WHEN THE PLAYER STOPS MOVING.
	// Very similar to functions that handle footprints when player is moving.

	// Drop two footprints
	public void standStillDrop() {
		if (stillFeet < 2) {
			if (isDropping == false) { // is it dropping? if it's not do this.
				isDropping = true;
				StartCoroutine (ExecuteAfterTime_StandStill (0.5f));
				stillFeet++;
			}
		}
	}

	// Helper for dropping when player is still.
	// We don't need to calculate a new angle, we just use the last angle
	IEnumerator ExecuteAfterTime_StandStill(float time)
	{
		yield return new WaitForSeconds(time);
		dropFootStill (lOrR, finalAngle);
		isDropping = false;
	}

	// Turned of the timer that destroys the foot if moving. This baby is going to last
	// Untill player starts moving. Destroy called in update.
	public void dropFootStill (bool flag, float angle) {
		float unitAngle = angle / 180;
//		print (unitAngle);
		//unitAngle is between (-1 and 1). I split this unitAngle into quadrants repersenting
		//the NSEW direction the player moves, and adjust the spacing between each foot so that
		//the player doesn't walk with mixmatched left/right feet.
		if (flag == false) {
			//LEFT FOOT DROP

			// if (Player is facing relatively forwards)
			// else if (player if facing relatively to the right)
			// else if (player is facing relatively to the left)
			// else if (player is facing relatively backwards)
			if (Mathf.Abs(unitAngle) < .25) {
				tempLeft2 = Instantiate (leftFootPrint, transform.position - (transform.right * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle > .25 && unitAngle < .75) {
				tempLeft2 = Instantiate (leftFootPrint, transform.position - (transform.forward * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle < -.25 && unitAngle > -.75) {
				tempLeft2 = Instantiate (leftFootPrint, transform.position - (transform.forward * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (Mathf.Abs(unitAngle) > .75) {
				tempLeft2 = Instantiate (leftFootPrint, transform.position - (transform.right * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			}
			//Drop right foot next time
			lOrR = true;
		} else if (flag == true) {
			//RIGHT FOOT DROP
			if (Mathf.Abs(unitAngle) < .25) {
				tempRight2 = Instantiate (rightFootPrint, transform.position + (transform.right * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle > .25 && unitAngle < .75) {
				tempRight2 = Instantiate (rightFootPrint, transform.position + (transform.forward * 1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (unitAngle < -.25 && unitAngle > -.75) {
				tempRight2 = Instantiate (rightFootPrint, transform.position + (transform.forward * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			} else if (Mathf.Abs(unitAngle) > .75) {
				tempRight2 = Instantiate (rightFootPrint, transform.position + (transform.right * -1), Quaternion.AngleAxis (180 - angle, Vector3.up));
			}
			//Drop left foot next time
			lOrR = false;
		}
	}
}
