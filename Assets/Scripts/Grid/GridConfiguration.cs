using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridConfiguration : MonoBehaviour {

	// The camera that will follow the action
	public Transform mainCamera;
	// The plane to draw the cells on
	public Transform referencePlane;
	// A grid of cells to navigate
	private Dictionary<Vector2, GridCell> grid;
	// A reference to the explorer class
	private BreadthFirstExplorer explorer;

	void Start () {
		grid = new Dictionary<Vector2, GridCell>();

		// Load the layout from file
		TextAsset gridLayout = Resources.Load("layout_medium") as TextAsset;

		// Split the layout string by newline and add each line to a list.
		List<string> gridLines = new List<string>(gridLayout.text.Replace("\r", "").Split('\n'));

		// For each line, increment the gridLine
		for (int gridLine = 0; gridLine < gridLines.Count; gridLine++) {
			// For each item, increment the gridItem
			for (int gridItem = 0; gridItem < gridLines[gridLine].Length; gridItem++) {

				// get the item at the current index path.
				char gridItemChar = gridLines[gridLine][gridItem];

				// construct an index path.
				Vector2 indexPath = new Vector2(gridLine, gridItem);
				// Add the cell using its type character, its index path, and the origin of the grid.
				grid.Add(indexPath, new GridCell(gridItemChar, indexPath, transform));
			}
		}

		// Add the explorer as a component
		explorer = gameObject.AddComponent("BreadthFirstExplorer") as BreadthFirstExplorer;
		// When adding a component (necessary for the Coroutine for now), a custom Initialize method must be used.
		explorer.Initialize(grid);
		// Explore the grid.
		explorer.Explore();
	}

	void Update () {
		// Return if the grid is fully explored (as much as it can be)
		if (explorer.navPath.Count == 0) {
			return;
		}

		// Get the Z value for the camera's target position
		float targetZ = explorer.navPath.Peek().y;
		// Construct the camera's target position
		Vector3 destinationPosition = new Vector3(mainCamera.position.x, mainCamera.position.y, targetZ);

		// Lerp kind of smoothly from the camera's current position to the target position.
		// This will be optimized later.
		mainCamera.position = Vector3.Lerp(mainCamera.position, destinationPosition, Time.deltaTime * 0.5f);
	}

}
