using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private bool inputEnabled;
	public float cameraSmoothing;

	void Start () {
		inputEnabled = false;
	}

	void OnEnable () {
		Explorer.PathFinishedConstructing += EnableKeyboardInteraction;
	}

	void OnDisable () {
		Explorer.PathFinishedConstructing -= EnableKeyboardInteraction;
	}

	void EnableKeyboardInteraction () {
		Debug.Log("enabled");
		inputEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inputEnabled) {
			return;
		}

		float multiplier = Time.deltaTime * cameraSmoothing;

		float moveCameraX = -Input.GetAxis("Vertical");
		float moveCameraZoom = Input.GetAxis("Mouse ScrollWheel");
		float moveCameraZ = Input.GetAxis("Horizontal");

		transform.Translate(new Vector3(moveCameraX, 0, moveCameraZ) * multiplier, Space.World);
		transform.Translate(new Vector3(0, 0, moveCameraZoom) * multiplier, Space.Self);
	}
}
