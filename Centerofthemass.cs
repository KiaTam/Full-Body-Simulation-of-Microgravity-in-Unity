using UnityEngine;
using System.Collections;

public class Centerofthemass : MonoBehaviour {

	//public GameObject Monnequin;
	public KinectModelControllerV2 KMCV;
	public Vector3 Cofmass;// the array pointing to the center of mass
	public Vector3 S_Cofmass; // The first center of the mass: the reference for rotation
	public int meanR;
	public int meanM;
	public int meanI;
	public Vector3 Omega; //Angular velocity
	public Vector3 Om; //Angular velocity
	public Vector3 myVector;
	public Vector3 finalpos;
	public Vector3[] x= new Vector3[64];
	public float MR=0;
	public Vector3 Momentum= Vector3.zero;
	private int start=0;
	public GameObject CoMCube;
	public float dt;
	public float dtf;
	public float tm;
 


	// Use this for initialization
	void Start () {
		tm = Time.time;

	}
	
	// Update is called once per frame
	void  FixedUpdate() {
				Vector3 CoM = Vector3.zero;
		//Vector3[] x= new Vector3[64];
				float c = 0f;


		if((Time.time - tm)< 3) return;
		
		if (start == 0) {

				
			    
				start = 1;
						for (int ii = 1; ii < KMCV._bones.Length; ii++) {
								if (ii == 16)
										ii = 17;
								x [ii] = (KMCV._bones [ii].rigidbody.worldCenterOfMass);
						}


			for (int ii = 1; ii < KMCV._bones.Length; ii++) {
				if (ii == 16)
					ii = 17;
				
				CoM += KMCV._bones [ii].rigidbody.worldCenterOfMass * KMCV._bones [ii].rigidbody.mass;
				c += KMCV._bones [ii].rigidbody.mass;


			}
			
			S_Cofmass = CoM / c;
				}
		

//----------------------------------------------------------------------------
//     Calc: position ofn Center of mass 
//----------------------------------------------------------------------------

				for (int ii = 1; ii < KMCV._bones.Length; ii++) {
						if (ii == 16)
								ii = 17;
			
						CoM += KMCV._bones [ii].rigidbody.worldCenterOfMass * KMCV._bones [ii].rigidbody.mass;
						c += KMCV._bones [ii].rigidbody.mass;
				}

				Cofmass = CoM / c;
				CoMCube.rigidbody.transform.position = Cofmass;
		        //S_Cofmass =Cofmass ;
//----------------------------------------------------------------------------
//		Calc: Sum of Angular momentum calc =  r x mv     (v is dx)
//----------------------------------------------------------------------------
		Momentum = Vector3.zero;
				for (int ii = 1; ii <  KMCV._bones.Length; ii++) {
						if (ii == 16)
								ii = 17;
						//Momentum=new Vector3 (ii,33,33);
						Vector3 dx = (KMCV._bones [ii].rigidbody.worldCenterOfMass - x[ii])*10;
						Vector3 r = (KMCV._bones [ii].rigidbody.worldCenterOfMass - Cofmass)*10;
						Momentum += Vector3.Cross (r, dx)* KMCV._bones [ii].rigidbody.mass;
						
						//Store positions for the next round
						x [ii] = (KMCV._bones [ii].rigidbody.worldCenterOfMass);
				}


//----------------------------------------------------------------------------
//       Calc: I-> Total Angular momentom Inertia
//----------------------------------------------------------------------------
		        MR = 0;
				for (int ii = 1; ii < KMCV._bones.Length; ii++) {
						if (ii == 16)
								ii = 17;
						float r = (KMCV._bones [ii].rigidbody.worldCenterOfMass - Cofmass).magnitude;
						MR += r * KMCV.Elbow_Right.rigidbody.mass;
				}

//-----------------------------------------------------------------------------
//   Calc: Rotation speed
//-----------------------------------------------------------------------------
		float index = 360/(100*2*Mathf.PI);
		Omega = -1 * index * Momentum / MR;
		if(Omega != Vector3.zero) Om=Omega;
		
		finalpos = KMCV.body.transform.position - Cofmass;
		finalpos = Quaternion.Euler(Omega) * finalpos;
		

//-----------------------------------------------------------------------------
//checking the timer
//-----------------------------------------------------------------------------
		dtf = Time.time - dt;
		dt = Time.time;

		}




}
