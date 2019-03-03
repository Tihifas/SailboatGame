using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour {
    // -- Public variables
    private Vector3 wind;

    //public float speed;

    // -- Private variables
    private GameObject sail;
    public GameObject playerExplosion;
    private GameController gameController;

    private Vector3 velocity;
    public float speed;
    private float boatAngle;
    private float sailAngle;
    private float sailAngleReal;
    public float distanceFromSide;

    // Use this for initialization
    void Start () {
        //Sail
        sail = GameObject.FindGameObjectWithTag("Sail");
        if (!sail) Debug.Log("Cannot find sail!");

        UpdateWind();

        sailAngle = 90f;
        // Rotation
        sailAngleReal = GetComponent<Rigidbody>().rotation.eulerAngles.z + sailAngle;
        sail.GetComponent<Rigidbody>().rotation = aToRot(sailAngleReal);

    }

    void FixedUpdate()
    {
        //Make "update variables" function?
        /* -- CONTROLS -- */
        // -- Boat
        float angVelBoat = 2f; //Angular velocity when turning
        if( Input.GetKey(KeyCode.A) )
        {
            if ( !Input.GetKey(KeyCode.D)) { GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, angVelBoat); }
            if (  Input.GetKey(KeyCode.D)) { GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f,      0); }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if ( !Input.GetKey(KeyCode.A)) { GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, -angVelBoat); }
            if (  Input.GetKey(KeyCode.A)) { GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f,       0); }
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        }

        //Sail
        sail.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity; //And then add below.
        float angVelSail = angVelBoat; //Angular velocity when turning
        if (Input.GetKey(KeyCode.Q))
        {
            if (!Input.GetKey(KeyCode.E)) { sail.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, GetComponent<Rigidbody>().angularVelocity.z + angVelSail ); }
            if (Input.GetKey(KeyCode.E)) { sail.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, GetComponent<Rigidbody>().angularVelocity.z); }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (!Input.GetKey(KeyCode.Q)) { sail.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, GetComponent<Rigidbody>().angularVelocity.z - angVelSail); }
            if (Input.GetKey(KeyCode.Q)) { sail.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, GetComponent<Rigidbody>().angularVelocity.z); }
        }
        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            sail.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, GetComponent<Rigidbody>().angularVelocity.z);
        }


        boatAngle = GetComponent<Rigidbody>().rotation.eulerAngles.z;

        //The following depend on "framrate"! Force timestep?
        Vector3 sailRotVec = aToVec(sail.GetComponent<Rigidbody>().rotation.eulerAngles.z);

        //Normal Mode£
        speed += Vector3.Dot(aToVec(boatAngle), Vector3.Dot(sailRotVec, wind) * sailRotVec);

        //My drag
        speed *= 0.998f;

        GetComponent<Rigidbody>().velocity = speed * aToVec(boatAngle);

        // -- Sail
        // Position
        sail.GetComponent<Rigidbody>().position = GetComponent<Rigidbody>().position;
    }

    /// <summary>
    /// Finds the wind object, gets wind vector and sets local version. This is called by oceancontroller when tile is changed.
    /// This way only works if there is only one gameobject with the name "Wind"
    /// </summary>
    public void UpdateWind() {
        WindController windController = GameObject.Find("Wind").gameObject.GetComponent<WindController>();
        if (!windController) throw new UnassignedReferenceException("Cannot find 'WindController' script");
        wind = windController.WindVector;
    }

    //Give Degrees
    Vector3 aToVec(float theta)
    {
        return new Vector3(Mathf.Cos(theta / 360 * 2 * Mathf.PI), Mathf.Sin(theta / 360 * 2 * Mathf.PI), 0);
    }

    //Give Degrees
    Quaternion aToRot(float theta)
    {
        return Quaternion.Euler(0.0f, 0.0f, theta);
    }

    /// <summary>
    /// Explode boat and end game
    /// </summary>
    public void Explode()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            gameController.GameOver();
        }
        if (!gameController) Debug.Log("Cannot find 'GameController' script");
        Instantiate(playerExplosion, this.transform.position, this.transform.rotation);
        Destroy(GameObject.FindGameObjectWithTag("Sail"));
        
        Destroy(this.gameObject); //has to be last because this script stops running
    }

    public enum Direction { Right, Up, Left, Down, Unassigned };

    /// <summary>
    /// Moves boat and sail til tile ij, on the spawnPosition side, fasinc the middle of the tile.
    /// iTile and jTile of oceanController is set.
    /// If tile ij is not found return false
    /// </summary>
    public bool MoveToTile(int i, int j, Direction spawnPosition)
    {
        GameObject tile_i_j = GameObject.Find("" + i + j);

        //If no tile is found explode boat and end game. Then return from OnTriggerEnter
        if (!tile_i_j){
            Debug.Log("tileij not found");
            return false;
        }

        Transform tileijTransform = tile_i_j.transform;
        if (!tileijTransform) throw new UnassignedReferenceException("tileijTransform not found");

        //Debug.Log("new_i = " + new_i + ", new_j = " + new_j);
        //oceanController.iTile = new_i; oceanController.jTile = new_j;
        //debugText.text = "new_i = " + new_i + ", new_j = " + new_j;

        OceanController oceanController = GameObject.Find("OceanController").gameObject.GetComponent<OceanController>();
        oceanController.Set_iTile_jTile(i, j);

        Vector3 unitVecDirSpawnPoint = new Vector3(0f, 0f, 0f);
        switch (spawnPosition)
        {
            case Direction.Right:
                unitVecDirSpawnPoint = Vector3.right;
                break;
            case Direction.Up:
                unitVecDirSpawnPoint = Vector3.up;
                break;
            case Direction.Left:
                unitVecDirSpawnPoint = Vector3.left;
                break;
            case Direction.Down:
                unitVecDirSpawnPoint = Vector3.down;
                break;
            case Direction.Unassigned:
                throw new UnassignedReferenceException("Direction.Unassigned");
                break;
            default:
                break;
        }

        Vector3 tilePos = tileijTransform.position;
        if (tileijTransform.localScale.x - tileijTransform.localScale.y > 0.05) throw new System.Exception("Tile is not square, and here is an implementation that only work for square tiles!");
        Transform backgroundTransform = tile_i_j.transform.Find("Background").gameObject.transform;

        float tileHalfSideL = backgroundTransform.localScale.x / 2; //This way only works for Squre tiles
        Vector3 boatPos = new Vector3(0f, 0f, 0f); //To not refer to other objects
        boatPos += tilePos + unitVecDirSpawnPoint * tileHalfSideL - unitVecDirSpawnPoint * distanceFromSide;
        //boatPos = tilePos - new Vector3(1f, 0f, 0f) * delta_i * (backgroundHalfxSideL - distanceFromSide)
        //- new Vector3(0f, 1f, 0f) * delta_j * (backgroundHalfySideL - distanceFromSide)
        //+ 10 * Vector3.back; //This is not very elegant. Move background or use something else as reference?
        //float xBoat = delta_i * () + delta_j * (); //Explanation

        Quaternion boatQuaternion = Quaternion.identity;
        Quaternion sailQuaternion = Quaternion.identity;

        //Rotating boat
        switch (spawnPosition)
        {
            case Direction.Right:
                boatQuaternion = Quaternion.AngleAxis(180f, Vector3.back); sailQuaternion = Quaternion.AngleAxis(270, Vector3.back);
                break;
            case Direction.Up:
                boatQuaternion = Quaternion.AngleAxis(270f, Vector3.back); sailQuaternion = Quaternion.AngleAxis(0f, Vector3.back);
                break;
            case Direction.Left:
                boatQuaternion = Quaternion.AngleAxis(0f, Vector3.back); sailQuaternion = Quaternion.AngleAxis(90f, Vector3.back);
                break;
            case Direction.Down:
                boatQuaternion = Quaternion.AngleAxis(90f, Vector3.back); sailQuaternion = Quaternion.AngleAxis(180f, Vector3.back);
                break;
            case Direction.Unassigned:
                throw new UnassignedReferenceException("Direction unassigned!");
            //break; unreachble
            default:
                break;
        }

        //Moving camera
        GameObject camera = GameObject.Find("Camera");
        if (!camera) { throw new MissingReferenceException("Camera not found!"); }
        Vector3 cameraPos = camera.transform.position;
        //cameraPos = new Vector3(tile_new_inew_j.transform.position.x, tile_new_inew_j.transform.position.y, -1f); //This works because tile_new_inew_j is found using new_i and new_j
        cameraPos.x = tile_i_j.transform.position.x; cameraPos.y = tile_i_j.transform.position.y;
        camera.transform.position = cameraPos;

        //GameObject sail = GameObject.Instantiate(SailPrefab, boatPos, boatQuaternion);
        //GameObject boat = GameObject.Instantiate(BoatPrefab, boatPos, boatQuaternion);
        this.transform.position = boatPos;
        this.GetComponent<BoatController>().speed = 0f;
        this.GetComponent<Rigidbody>().rotation = boatQuaternion;

        //boatAngle = GetComponent<Rigidbody>().rotation.eulerAngles.z;

        sail.transform.position = boatPos;
        sail.GetComponent<Rigidbody>().rotation = sailQuaternion;

        return true; //If this is reaced the metod call was successful
    }
}
