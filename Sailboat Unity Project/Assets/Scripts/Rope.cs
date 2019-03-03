using UnityEngine;

public class Rope : MonoBehaviour {

    public Rigidbody hook;
    public Rigidbody mainMenuPanel;
    public GameObject LinkPrefab;
    public GameObject connectedTo;
    public int nLinks = 7;

	// Use this for initialization
	void Start () {
        GenerateRope();
    }

    public void GenerateRope()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = connectedTo.transform.position;
        Vector3 startEndVector = endPos - startPos;

        //Make rope as cubes connected with hinges
        Rigidbody prevRB = hook;
        for (int i = 0; i < nLinks; i++)
        {
            //Instatiate a link at i / nLinks the way to connectedTo
            Vector3 pos = startPos + startEndVector * i / nLinks;
            //Transform linkTransform = new Transform(pos, transform.rotation); //Just using this as template

            //Debug.Log("i = " + i + ", pos = " + linkTransform.position);
            GameObject link = GameObject.Instantiate(LinkPrefab, pos, this.transform.rotation);
            //link.transform.parent = this.transform; //This = rope?
            HingeJoint joint = link.GetComponent<HingeJoint>();
            joint.connectedBody = prevRB;
            prevRB = link.GetComponent<Rigidbody>();
        }

        //Connect to menu panel
        //HingeJoint joint = link.GetComponent<HingeJoint>();
        //joint.connectedBody = prevRB;

        connectedTo.GetComponent<HingeJoint>().connectedBody = prevRB;

    }
}