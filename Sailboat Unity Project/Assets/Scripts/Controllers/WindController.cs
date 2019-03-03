using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{

    public GameObject particle;

    private float windVectorAngle;

    private Vector3 toSpawnLineVector;
    private float diagonalLength;
    private Vector3 windOrtogonalVec;
    private float xTileMin, xTileMax, yTileMin, yTileMax;
    private float timeBetweenParts;
    public Vector3 _windVector; //bad form, but I want to set it from editor on script component

    public Vector3 WindVector
    {
        get
        {
            return _windVector;
        }
        set
        {
            _windVector = value;
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        windVectorAngle = vecToA(_windVector);

        timeBetweenParts = 0.02f;
        //int partNo = 
        diagonalLength = Mathf.Sqrt(Mathf.Pow(transform.localScale.x, 2) + Mathf.Pow(transform.localScale.y, 2)); ; /*Length of diagonal of world*/

        /*Vector to middle of line from which wind particles spawn*/
        Vector3 windVectorUnit = _windVector / _windVector.magnitude;
        toSpawnLineVector = transform.position - windVectorUnit * diagonalLength / 2.0f;
        windOrtogonalVec = new Vector3(-windVectorUnit.y, windVectorUnit.x, 0); /*orthogonal unit vector to wind direction in x-y plane*/

        float xThis = this.transform.position.x; float yThis = this.transform.position.y;
        //0.51 because problems arise when it's on the line
        xTileMin = xThis - 0.51f * transform.localScale.x;
        xTileMax = xThis + 0.51f * transform.localScale.x;
        yTileMin = yThis - 0.51f * transform.localScale.y;
        yTileMax = yThis + 0.51f * transform.localScale.y;
        //Debug.Log("zTileMin = " + xTileMin);
        //Debug.Log("zTileMin = " + xTileMax);
        //Debug.Log("zTileMin = " + yTileMin);
        //Debug.Log("zTileMin = " + yTileMax);

        StartCoroutine(spawnParticles());

    }

    IEnumerator spawnParticles()
    {
        yield return new WaitForSeconds(0);
        while (true)
        {
            Vector3 spawnPosition = toSpawnLineVector + Random.Range(-diagonalLength / 2.0f, diagonalLength / 2.0f) * windOrtogonalVec;
            //Only spawn if "inside" tile
            //if (spawnPosition.x >= xTileMin && spawnPosition.x <= xTileMax && spawnPosition.y >= yTileMin && spawnPosition.y <= yTileMax)
            //{ 
            //Detroy all wond particles when changing schene instread
            Quaternion spawnRotation = aToRot(windVectorAngle);
            GameObject particle0 = Instantiate(particle, spawnPosition, spawnRotation);
            Rigidbody particle0rb = particle0.GetComponent<Rigidbody>();
            particle0rb.velocity = _windVector * 350;
            float scaleF = Random.Range(0.4f, 1f);
            particle0.transform.localScale = new Vector3(1f, 0.15f, 1f) * scaleF;
            particle0.transform.parent = this.transform;
            yield return new WaitForSeconds(timeBetweenParts);
            //}
            //else { yield return new WaitForSeconds(0); }



        }
    }

    //public Vector3 getWindVector()
    //{
    //    return windVector;
    //}

    //Vector3 -> angle about z axis
    float vecToA(Vector3 vec)
    {
        float angle = Vector3.Angle(Vector3.right, vec);
        return angle;
    }

    //Give Degrees
    Quaternion aToRot(float theta)
    {
        return Quaternion.Euler(0.0f, 0.0f, theta);
    }
}
