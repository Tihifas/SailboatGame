using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundary : MonoBehaviour
{
    //private GameController gameController;

    //void Start()
    //{
    //    GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
    //    if (gameControllerObject != null)
    //    {
    //        gameController = gameControllerObject.GetComponent<GameController>();
    //    }
    //    if (gameController == null)
    //    {
    //        Debug.Log("Cannot find 'GameController' script");
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "WindParticle")
        {
            Destroy(other.gameObject);
        }
    }
}