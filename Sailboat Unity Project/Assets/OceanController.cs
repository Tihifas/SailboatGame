using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanController : MonoBehaviour {
    private int _iTile, _jTile;
    public BoatController.Direction boatSpawnSide; //Will get changed when moving to neigbor tiles

    public int iTile
    {
        get { return _iTile; }
        set
        {
            //Debug.Log("OceanController: old i = " + _iTile + ", new i = " + value);
            _iTile = value;
        }
    }

    public int jTile
    {
        get { return _jTile; }
        set
        {
            //Debug.Log("OceanController: old j = " + _jTile + ", new j = " + value);
            _jTile = value;
        }
    }


    // Use this for initialization
    void Start () {
        _iTile = 0; _jTile = 0;
    }
    private void SetActiveTile(int iTile, int jTile, bool active)
    {
        string ij_string = "" + iTile + jTile;
        GameObject tileij = GameObject.Find(ij_string);
        //if (!tileij) Debug.Log("tileij not found");
        if (!tileij) throw new UnassignedReferenceException("tileij not found");
        GameObject activeInactiveObjects = tileij.transform.Find("ActiveInactiveObjects").gameObject;
        if (!tileij) throw new UnassignedReferenceException("activeInactiveObjects not found");
        activeInactiveObjects.SetActive(active);
    }

    private void DeactivateTile(int iTile, int jTile)
    {
        SetActiveTile(iTile, jTile, false);
    }

    private void ActivateTile(int iTile, int jTile)
    {
        SetActiveTile(iTile, jTile, true);
        BoatController boatControllerObject = GameObject.Find("Boat").gameObject.GetComponent<BoatController>();
        if (!boatControllerObject) throw new UnassignedReferenceException("Cannot find boatControllerObject");
        boatControllerObject.UpdateWind();
    }

    public void Set_iTile_jTile(int iTileIN, int jTileIN)
    {
        DeactivateTile(this.iTile, this.jTile); //Deactivating old tile
        this.iTile = iTileIN;
        this.jTile = jTileIN;
        ActivateTile(iTileIN, jTileIN); //Activating new tile
    }

    public void Restart()
    {
        BoatController boatController = GameObject.Find("Boat").gameObject.GetComponent<BoatController>();
        boatController.MoveToTile(iTile, jTile, boatSpawnSide);
    }
}