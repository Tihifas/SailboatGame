using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNeighborTile : MonoBehaviour {
    //public GameObject BoatPrefab;
    //public GameObject SailPrefab;
    public GameObject boat;
    public GameObject sail;
    public int delta_i;
    public int delta_j;
    private int old_i, old_j, new_i, new_j;
    public float distanceFromSide;
    private OceanController oceanController;


    //public bool active;

    // Use this for initialization
    void Start () {
        //GUIText debugText = GameObject.Find("DebugText").GetComponent<GUIText>();

        //active = true;

        //GameObject tile00 = GameObject.Find("00");
        //if (tile00) { Debug.Log("tile00 found"); }
        //else { Debug.Log("tile00 not found"); }

        //GameObject camera00 = tile00.transform.Find("Camera").gameObject;
        //if (!camera00) throw new UnassignedReferenceException();
        //camera00.SetActive(true);
    }

    //enum Direction { Right, Up, Left, Down, Unassigned};

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BoatController boatController = other.gameObject.GetComponent<BoatController>();

            GameObject oceanControllerObject = GameObject.Find("OceanController");
            if (oceanControllerObject != null) oceanController = oceanControllerObject.GetComponent<OceanController>();
            if (oceanController == null) Debug.Log("Cannot find 'OceanController' script");
            old_i = oceanController.iTile; old_j = oceanController.jTile;
            
            new_i = old_i + delta_i;
            new_j = old_j + delta_j;

            BoatController.Direction spawnSide = BoatController.Direction.Unassigned;
            if (delta_i == 1 & delta_j == 0) { spawnSide = BoatController.Direction.Left; }
            if (delta_i == 0 & delta_j == 1) { spawnSide = BoatController.Direction.Down; }
            if (delta_i == -1 & delta_j == 0) { spawnSide = BoatController.Direction.Right; }
            if (delta_i == 0 & delta_j == -1) { spawnSide = BoatController.Direction.Up; }

            bool movedToTile = boatController.MoveToTile(new_i, new_j, spawnSide);

            //If moving to tile is not successful there is no neighbor so explode boat and end game. Then return from OnTriggerEnter
            if (!movedToTile) {
                GameObject.Find("Boat").gameObject.GetComponent<BoatController>().Explode(); //also game over
                return; //End OnTriggerEnter() because there is no tile to move to
            }
        }
    }

    //public GameObject FindCamera_ij(int i, int j)
    //{
    //    string ij_string = "" + i + j;
    //    GameObject tileij = GameObject.Find(ij_string);
    //    if (!tileij) Debug.Log("tileij not found");
    //    GameObject cameraij = tileij.transform.Find("Camera").gameObject;
    //    if (!cameraij) Debug.Log("cameraij not found");
    //    return cameraij;
    //}

}
