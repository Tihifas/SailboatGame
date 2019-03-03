using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWindParticles : MonoBehaviour {

    void OnTriggerExit (Collider other)
    {
        if (other.tag == "WindParticle")
        {
            Destroy(other.gameObject);
        }
    }
}
