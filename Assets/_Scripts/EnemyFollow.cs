using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour {
    //enemy speed
	public float speed;

	//initial position
	private Vector3 initialPos;

	//to store player Position's
    public GameObject Player;
	private Vector3 playerPos;

	//script to manipulate player's heartRate
	private PlayerController playerController;

	//distance between enemy and player
	private Vector3 distance;

	//if enemy is within this range of player it will follow and scare
	public int huntingRange;
	RaycastHit hit;

	//if the raycast hits the player then this is true, if not false
	private bool inLineOfSight;

	//if enemy is on the hunt he follows the player, if not MISSION FAILED return to position
	//he was instantiated in
	private bool isHunting;

	private NavMeshAgent agent;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
		playerController = Player.GetComponent<PlayerController> ();
		initialPos = gameObject.transform.position;
	    agent = GetComponent<NavMeshAgent>();
		startHunt ();
    }


    void Update () {
	    transform.rotation = Quaternion.Euler(Vector3.zero);
		if (isHunting) {
//			transform.position = Vector3.MoveTowards (transform.position, playerPos, speed * Time.deltaTime);
			agent.SetDestination(Player.transform.position);

		} else {
//			transform.position = Vector3.MoveTowards (transform.position, initialPos, speed * Time.deltaTime);
			agent.SetDestination(initialPos);
		}
    }

	//determines if player is close by and weather or not to scare player depending on if player is in line of sight
	public void startHunt() {
		StartCoroutine (ExecuteAfterTime (0.5f));
	}

	//helper method
	IEnumerator ExecuteAfterTime (float time)
	{
		yield return new WaitForSeconds(time);
		playerPos = Player.transform.position;
		distance = gameObject.transform.position - playerPos;
		if (distance.magnitude < huntingRange) {
			isHunting = true;
			if (Physics.Raycast (transform.position, (Player.transform.position - transform.position), out hit, huntingRange)) {
				//print (hit.collider.tag);
				if (hit.collider.tag == "Player" && inLineOfSight == false) { //if enemy sees player, player.count (which counts enemies near by incrememnts)
//					playerController.EnemyCount++;
					playerController.IncreaseEnemyCount(1);
					inLineOfSight = true;
				} else if (hit.collider.tag != "Player" && inLineOfSight) { //if enemy loses track of player, player.count decrements)
//					playerController.EnemyCount--;
					playerController.DecreaseEnemyCount(1);
					inLineOfSight = false;
				}
			}
		} else { // if enemy had a line of sight on player but player runs out of enemy's hunting range, then decrement player.count
			if (inLineOfSight) {
				playerController.EnemyCount--;
				inLineOfSight = false;
			}
			isHunting = false;
		}
		startHunt ();
	}
}
