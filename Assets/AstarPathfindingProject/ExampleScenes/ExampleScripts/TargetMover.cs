using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

namespace Pathfinding {
	public class TargetMover : MonoBehaviour {
		
		/** Mask for the raycast placement */
		public LayerMask mask;
		
		public Transform target;
		AIPath[] ais2;

		Camera cam;

        Seeker player;
		
		public void Start () {
			//Cache the Main Camera
			cam = Camera.main;
			ais2 = FindObjectsOfType(typeof(AIPath)) as AIPath[];
		}
		
		public void OnGUI () {
            
            if (Input.GetMouseButtonUp(1))
            {
				UpdateTargetPosition ();
			}
		}

		public void UpdateTargetPosition () {
			//Fire a ray through the scene at the mouse position and place the target where it hits
			RaycastHit hit;
			if (Physics.Raycast	(cam.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, mask) && hit.point != target.position) {
				target.position = hit.point;
				
				if (ais2 != null) {
					for (int i=0;i<ais2.Length;i++) {
                        if (ais2[i] != null)
                        {
                            ais2[i].SearchPath();
                            ais2[i].canMove = true;
                        }
					}
				}
			}
		}
		
	}
}