using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[Range(5,75)]
	public float cameraSmoothing;
	
	// Update is called once per frame
	void Update () {
		// The total smoothing multiplier for the camera controls.
		float multiplier = Time.deltaTime * cameraSmoothing;

		// The value by which to multiply each axis
		float moveCameraX = -Input.GetAxis("Vertical");
		float moveCameraZoom = Input.GetAxis("Mouse ScrollWheel");
		float moveCameraZ = Input.GetAxis("Horizontal");

		// translate with respect to the world space (parallel to the XZ plane)
		transform.Translate(new Vector3(moveCameraX, 0, moveCameraZ) * multiplier, Space.World);
		// translate with respect to the camera's space (where transform.forward is
		// equivalent to the X axis)
		transform.Translate(new Vector3(0, 0, moveCameraZoom) * multiplier, Space.Self);
	}
}
