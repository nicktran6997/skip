using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

	private Camera myCam;
	private float left;
	private float right;
	private float top;
	private float bottom;


	// Awake makes sure the CameraController is initialized before anything else.
	// Necessary in order to render the GridManager on Start().
	void Awake () {
		myCam = GetComponent<Camera>();
		calculateBounds ();
	}
	
	// Update is called once per frame
	void Update () {

    }

	/*Updates the Camera's internal boundaries. Could just be put in Update() but
	 *I am just having it be a public method so that the camera isn't constantly making unnecessary updates
	 *to its boundaries. Will probably only be called by the GridManager when the camera is called to zoom.
	 */
    public void UpdateCameraController() {
        calculateBounds();
    }

	//Internal calculation of the Camera's boundaries.
	private void calculateBounds() {
		Vector3 camPos = myCam.transform.position;
		float ortho = myCam.orthographicSize;
		float aspect = myCam.aspect;
		left = camPos.x - ortho*aspect;
		bottom = camPos.y - ortho;
		right = camPos.x + ortho*aspect;
		top = camPos.y + ortho;

	}

	//Checks if the given position is within the Camera's view. Should be accessible to all objects.
	public bool inBounds(Vector3 position, float tolerance) {
		bool val = true;
		float x = position.x;
		float y = position.y;

		if (x > right + tolerance || x < left - tolerance) {
			val = false;
		} else if (y > top + tolerance || y < bottom - tolerance){
			val = false;
		}
		return val;

	}

	public float getMinX(){
		return left;
	}

	public float getMinY(){
		return bottom;
	}

	public float getMaxX(){
		return right;
	}

	public float getMaxY(){
		return top;
	}
}
