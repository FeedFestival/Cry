using UnityEngine;
using System.Collections;

public class Stats_BaseStats : MonoBehaviour {

    AIPath aiPath;

    public float MovementSpeed;

	// Use this for initialization
	void Awake () {
        aiPath = this.transform.GetComponent<AIPath>();
        
        MovementSpeed = 2.5f;

        aiPath.speed = MovementSpeed; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
