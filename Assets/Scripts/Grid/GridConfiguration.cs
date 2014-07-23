using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridConfiguration : MonoBehaviour {

	// The camera that will follow the action
	public Transform mainCamera;
	// The plane to draw the cells on
	public Transform referencePlane;
	// Whether to exit early or not
	public bool earlyExit;
	// Whether or not to step manually through each navigation item
	public bool stepNavigation;
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
		explorer.Initialize(grid, earlyExit, stepNavigation);
		// Explore the grid.
		explorer.Explore();
	}

	void Update () {
		if (!stepNavigation) {
			return;
		}

		// Step through the navigation sequence manually, if that flag is checked in the editor.
		bool nextStep = Input.GetButtonUp("Jump");
		if (nextStep && explorer.navPath.Count > 0) {
			explorer.AdvanceExplorer();
		}
	}

}
