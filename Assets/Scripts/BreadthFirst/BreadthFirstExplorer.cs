using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// This class implements the Breadth First Search algorithm.
public class BreadthFirstExplorer : Explorer {


	// The queue used to navigate throughout the grid.
	public Queue<Vector2> exploreQueue;
	// The shortest path back from the target to the source.
	public Stack<Vector2> navPath;
	// A dictionary that tracks the order in which cells are visited.
	private Dictionary<Vector2, Vector2> came_from;
	// The navigable grid, which excludes obstacles.
	private Dictionary<Vector2, GridCell> navGrid;
	// Whether or not to exit early
	private bool earlyExit;
	// Whether or not to step manually through each navigation item
	private bool stepNavigation;

	public void Initialize (Dictionary<Vector2, GridCell> grid, bool earlyExit, bool stepNavigation) {
		this.grid = grid;
		this.earlyExit = earlyExit;
		this.stepNavigation = stepNavigation;
		exploreQueue = new Queue<Vector2>();
		navPath = new Stack<Vector2>();
		came_from = new Dictionary<Vector2, Vector2>();
	}

	public void Explore () {
		// Get a reference to the first source cell that can be found.
		// This might change in the future, and multiple sources might be allowed.
		sourceCell = grid.FirstOrDefault(cell => cell.Value.cellType == "source").Key;
		targetCell = grid.FirstOrDefault(cell => cell.Value.cellType == "target").Key;
		// Add the source cell to the queue
		exploreQueue.Enqueue(sourceCell);
		// Note that this cell is the source, or start, so it has no
		// predecessor cells.
		//came_from[sourceCell] = null;
		// get a navigable grid
		navGrid = NavigableGrid();
		// Find the target using a Breadth First Search.
		LookForTarget();
		// Trace back a path to the source cell.
		ConstructPath(sourceCell);
		// Send an event notification that the path has been constructed.
		NotifyPathFinished();
		// Start animating the path navigation if the stepNavigation flag is left unchecked
		if (!stepNavigation) {
			StartCoroutine(TracePathToTarget());	
		}
	}

	void LookForTarget () {
		// When the exploreQueue is empty, no more cells can be explored.
		while (exploreQueue.Count > 0) {
			// dequeue the current cell.
			Vector2 currentCell = exploreQueue.Dequeue();
			// Check for target in case of early exit
			if (earlyExit) {
				if (currentCell == targetCell) {
					return;
				}
			}
			// Loop over each of the current cell's neighbors.
			foreach (Vector2 nextNeighbor in Neighbors(currentCell, navGrid)) {
				// if the cell is unvisited, add it to the queue, and set a path to the current cell.
				if (!came_from.ContainsKey(nextNeighbor)) {
					exploreQueue.Enqueue(nextNeighbor);
					came_from[nextNeighbor] = currentCell;
				}
			}
		}
	}

	void ConstructPath (Vector2 sourceCell) {
		// A reference to the first target cell that is found.
		Vector2 currentCell = grid.FirstOrDefault(cell => cell.Value.cellType == "target").Key;
		// Push the target cell onto the navigation stack.
		navPath.Push(currentCell);

		// the path history is traversed, and the index paths are pushed to a stack.
		while (currentCell != sourceCell) {
			currentCell = came_from[currentCell];
			navPath.Push(currentCell);
		}
	}

	IEnumerator TracePathToTarget () {

		// Advance from the source cell until the target cell is reached
		while (navPath.Count > 0) {
			AdvanceExplorer();
			yield return null;
		}
	}

	// for each new cell, change the color to indicate a path for the visual aid.
	public void AdvanceExplorer () {
			Vector2 currentCell = navPath.Pop();
			navGrid[currentCell].cell.renderer.material.color = Color.green;
	}
}
