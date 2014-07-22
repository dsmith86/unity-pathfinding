using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// This class implements the Breadth First Search algorithm.
public class BreadthFirstExplorer : Explorer {

	// The queue used to navigate throughout the grid.
	public Queue<Vector2> exploreQueue;
	// A dictionary that tracks all visited cells.
	private Dictionary<Vector2, bool> visited;
	// The navigable grid, which excludes obstacles.
	private Dictionary<Vector2, GridCell> navGrid;

	public void Initialize (Dictionary<Vector2, GridCell> grid) {
		this.grid = grid;
		exploreQueue = new Queue<Vector2>();
		visited = new Dictionary<Vector2, bool>();
	}

	public void Explore () {
		// Get a reference to the first source cell that can be found.
		// This might change in the future, and multiple sources might be allowed.
		sourceCell = grid.FirstOrDefault(cell => cell.Value.cellType == "source").Key;
		// Add the source cell to the queue
		exploreQueue.Enqueue(sourceCell);
		// Note that this cell is now 'visited'.
		visited[sourceCell] = true;
		// get a navigable grid
		navGrid = NavigableGrid();
		StartCoroutine(LookForTarget());
	}

	IEnumerator LookForTarget () {
		// When the exploreQueue is empty, no more cells can be explored.
		while (exploreQueue.Count > 0) {
			// dequeue the current cell.
			Vector2 current = exploreQueue.Dequeue();
			// Loop over each of the current cell's neighbors.
			foreach (Vector2 nextNeighbor in Neighbors(current, navGrid)) {
				// if the cell is unvisited, add it to the queue, and change its material color to blue.
				if (!visited.ContainsKey(nextNeighbor)) {
					exploreQueue.Enqueue(nextNeighbor);
					visited[nextNeighbor] = true;
					grid[nextNeighbor].cell.renderer.material.color = Color.blue;
				}
			}
			// Once the cell is explored, change its color to green.
			grid[current].cell.renderer.material.color = Color.green;
			yield return null;
		}
	}
}
