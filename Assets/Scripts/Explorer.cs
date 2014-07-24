using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Base class for all explorer algorithms in this repository.
public class Explorer {

	// Path Action Delegate
	public delegate void PathActionDelegate();
	public static event PathActionDelegate PathFinishedConstructing;
	public static event PathActionDelegate PathShouldBeTraced;

	public Dictionary<Vector2, GridCell> grid;
	protected Vector2 sourceCell;
	protected Vector2 targetCell;

	// The shortest path back from the target to the source.
	public Stack<Vector2> navPath;

	// Some virtual functions that the base clase should implement
	public virtual void Explore () {}
	public virtual void AdvanceExplorer () {}

	// In order to navigate the grid in a logical fashion, obstacles must
	// be removed from what can be considered the navigable portion of the grid.
	public Dictionary<Vector2, GridCell> NavigableGrid () {
		// A new dictionary is created, and it is instantiated with a copy of the original grid.
		Dictionary<Vector2, GridCell> navGrid = new Dictionary<Vector2, GridCell>(grid);
		// Whenever a grid item's value is "obstacle," that item is removed from the nav grid.
		foreach(KeyValuePair<Vector2, GridCell> cell in grid) {
			if (cell.Value.cellType == "obstacle") {
				navGrid.Remove(cell.Key);
			}
		}
		return navGrid;
	}

	// This method returns a list of a cell's navigable neighbors.
	public List<Vector2> Neighbors (Vector2 cell, Dictionary<Vector2, GridCell> grid) {
		// This list defines the set of possible unit vectors that could point to a neighboring cell.
		List<Vector2> directions = new List<Vector2>(new Vector2[] { 	new Vector2(1,0),
																		new Vector2(0,1),
																		new Vector2(-1,0),
																		new Vector2(0,-1) });
		List<Vector2> matches = new List<Vector2>();

		// each of the directions is tested to see if it is a valid neighbor.
		foreach (Vector2 direction in directions) {
			// a neighbor is defined as the cell in the direction of each direction vector,
			// starting from the original cell.
			Vector2 neighbor = new Vector2(cell.x + direction.x, cell.y + direction.y);
			// If it is a valid neighbor, add it to the matches list.
			if (grid.ContainsKey(neighbor)) {
				matches.Add(neighbor);
			}
		}
		return matches;
	}

	protected static void NotifyPathFinished () {
		if (PathFinishedConstructing != null) {
			PathFinishedConstructing();
		}
	}

	protected static void NotifyPathShouldBeTraced () {
		if (PathShouldBeTraced != null) {
			PathShouldBeTraced();
		}
	}
}
