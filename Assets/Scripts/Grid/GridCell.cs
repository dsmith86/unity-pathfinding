using UnityEngine;
using System.Collections;

// This class represents the possible state of each cell in the grid.
// A cell could be a floor tile, an obstacle (wall), a source block, or a
// target block.
public class GridCell {

	// a string description of the specific cell type ('floor', 'obstacle', etc.)
	public string cellType;
	// a 2D representation of the cell's position on the grid
	public Vector2 cellPosition;
	// a reference to the actual object that is instantiated, mostly for rendering purposes.
	public GameObject cell;

	public GridCell(char gridItem, Vector2 cellPosition, Transform grid) {
		Vector3 scale;
		Vector3 globalPosition;

		this.cellPosition = cellPosition;

		// This is based on the preferred placement of objects on the grid.
		// At the moment, spaces in the input file will simply place a floor tile.
		// I plan to ignore spaces completely in the future, so that grid layouts can
		// be a bit more customizable. (Octagon, anyone?)
		switch (gridItem) {
			case '0':
				cellType = "floor";
				scale = new Vector3(1f, 0.01f, 1f);
				break;
			case '1':
				cellType = "obstacle";
				scale = new Vector3(1f, 1f, 1f);
				break;
			case 'x':
				cellType = "target";
				scale = new Vector3(1f, 1f, 1f);
				break;
			case 'p':
				cellType = "source";
				scale = new Vector3(1f, 1f, 1f);
				break;
			default:
				cellType = "floor";
				scale = new Vector3(1f, 0f, 1f);
				break;
		}
		
		// The global position is chosen relative to the plane on which the object will reside.
		globalPosition = PositionOnGrid.ForPlane(grid, scale, cellPosition);

		// Get the object from the Resources folder, using its cell type.
		cell = ObjectFromResource("Prefabs/" + cellType, globalPosition);
	}

	GameObject ObjectFromResource(string resource, Vector3 position) {
		// Instantiate the object at its respective position on the plane.
		return Object.Instantiate(Resources.Load(resource), position, Quaternion.identity) as GameObject;
	}
}
