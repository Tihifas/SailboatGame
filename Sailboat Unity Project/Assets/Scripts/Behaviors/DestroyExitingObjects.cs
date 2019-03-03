using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExitingObjects : MonoBehaviour {
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
