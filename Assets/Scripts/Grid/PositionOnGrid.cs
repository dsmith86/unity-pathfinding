using UnityEngine;
using System.Collections;

public static class PositionOnGrid {

	// This method determines the position at which to place a block, relative to a plane. It accounts
	// for the bounds of the plane, as well as the relative scale of the object being placed. Currently
	// only works for standard Unity cubes, and other primitives that have the dimensions 1 unit, cubed.
	public static Vector3 ForPlane(Transform referencePlane, Vector3 objectScale, Vector2 destination) {
		// The center of the plane
		Vector3 planeOrigin = referencePlane.position;
		// the relative width (X) and depth (Z) of the plane, based on the default unit scale of 10 units
		float planeUnitWidth = referencePlane.lossyScale.x * 10;
		float planeUnitDepth = referencePlane.lossyScale.z * 10;
		// the relative dimensions of the object to place, based on the default unit scale of 1 unit
		float objectUnitWidth = objectScale.x;
		float objectUnitHeight = objectScale.y;
		float objectUnitDepth = objectScale.z;
		
		// For each of the 3 coordinates in coordinate space, starting at the plane's origin, unit length
		// is subtracted, then have of the object's length in the respective axis. Then, assuming the
		// plane's and object's relative scales are whole numbers, the destination coordinates are
		// added to fit each object snugly within the map.
		Vector3 coordinateOrigin = new Vector3(planeOrigin.x - planeUnitWidth + objectUnitWidth/2 + destination.x, planeOrigin.y + objectUnitHeight/2, planeOrigin.z - planeUnitDepth + objectUnitDepth/2 + destination.y);
		return coordinateOrigin;
	}
	
}