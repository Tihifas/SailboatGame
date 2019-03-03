using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour
{

    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Destroy(other.gameObject);
            //Destroy(GameObject.FindGameObjectWithTag("Sail"));
            //other.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            //Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.HitGoal();
        }
    }
}