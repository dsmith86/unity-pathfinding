﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The different types of supported explorers
	public enum ExplorerType {
		BFS, Dijkstra
	}

public class GridConfiguration : MonoBehaviour {

	// The camera that will follow the action
	public Transform mainCamera;
	// Whether to exit early or not
	public bool earlyExit;
	// Whether or not to step manually through each navigation item
	public bool stepNavigation;
	// A grid of cells to navigate
	private Dictionary<Vector2, GridCell> grid;
	// The type of explorer selected in the editor
	public ExplorerType explorerType;
	// The list of classes
	private Dictionary<ExplorerType, Explorer> explorers;

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

		// Initialize the explorers.
		explorers = new Dictionary<ExplorerType, Explorer>() {
			{ ExplorerType.BFS, new BreadthFirstExplorer(grid, earlyExit, stepNavigation) },
			{ ExplorerType.Dijkstra, new DijkstraExplorer() }
		};

		// Explore the grid.
		explorers[explorerType].Explore();
	}

	void BeginTracingPath () {
		StartCoroutine(TracePathToTarget());
	}

	IEnumerator TracePathToTarget () {
		// Advance from the source cell until the target cell is reached
		while (explorers[explorerType].navPath.Count > 0) {
			explorers[explorerType].AdvanceExplorer();
			yield return null;
		}
	}

	void Update () {
		if (!stepNavigation) {
			return;
		}

		// Step through the navigation sequence manually, if that flag is checked in the editor.
		bool nextStep = Input.GetButtonUp("Jump");
		if (nextStep && explorers[explorerType].navPath.Count > 0) {
			explorers[explorerType].AdvanceExplorer();
		}
	}

	void OnEnable () {
		Explorer.PathShouldBeTraced += BeginTracingPath;
	}

	void OnDisable () {
		Explorer.PathShouldBeTraced -= BeginTracingPath;
	}

}
