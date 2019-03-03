using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<BoatController>().Explode(); //also ends game
        }
    }
}
