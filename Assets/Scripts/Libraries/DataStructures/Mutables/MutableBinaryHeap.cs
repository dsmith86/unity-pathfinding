namespace DSM {

	namespace DataStructures {

		namespace Generics {

			namespace Mutables {
				// SUMMARY
				// =======
				// Mutable version of DSM.DataStructures.Generics.BinaryHeap.
				// Allows updating of arbitrary elements in the heap, provided
				// that a class lower in the execution stack maintains a map of
				// heap items.
				//
				// TYPE PARAMETERS
				// ===============
				// T : Value Type
				//
				// PUBLIC FUNCTIONS (for unchanged methods, see the base class)
				// ================
				// [Override] void Insert (T newItem)
				//// inserts an item like in the base class but additionally
				//// sends an event notification with the insertion index as
				//// well as the index of the affected branch's highest point.
				//
				// [Override] T RemoveRoot ()
				//// removes the root like in the base class but additionally
				//// sends an event notification with the removal index (0) as
				//// well as the index of the affected branch's highest point.
				//
				// [New] void UpdateItem (int originalItemIndex, T updatedItem)
				//// updates the chosen item's value in the heap, thereby 
				//// possibly affecting its index as well. Sends an event
				//// notification with the originally updated item's index, as
				//// well as the affected branch (whose endpoint is the original
				//// item, updated to some new value).

				using System;
				using System.Collections;
				using System.Collections.Generic;
				public class MutableBinaryHeap<T> : BinaryHeap<T>, IEnumerable<T> {

					public delegate void HeapChanged(int mutatedItemIndex, int affectedEndpoint);
					public static event HeapChanged HeapHasInsertedItem;
					public static event HeapChanged HeapHasRemovedItem;
					public static event HeapChanged HeapHasUpdatedItem;

					public override void Insert (T newItem) {

						int i = Count;
						// The index of the leaf node eventually affected by the insert.
						// if this index == i, then the new item was greater in value
						// than everything else in the heap.
						int j = i;

						// Add the new item to the bottom of the heap.
						Items.Add(newItem);

						// Until the new item is greater than its parent item,
						// swap the two
						while (i > 0 && Comparer.Compare(Items[(i - 1) / 2], newItem) > 0) {
							Items[i] = Items[(i - 1) / 2];

							i = (i - 1) / 2;
						}
						// The new index in the list is the appropriate location for
						// the new item.
						Items[i] = newItem;

						if (HeapHasInsertedItem != null) {
							HeapHasInsertedItem(i, j);
						}
					}

					public override T RemoveRoot () {
						// Throw an exception if the heap is empty.
						if (Items.Count == 0) {
							throw new InvalidOperationException("The heap is empty.");
						}

						// Get the root value's reference.
						T rootValue = Items[0];

						// Temporarirly store the last item's value.
						T temporary = Items[Items.Count - 1];
						// Remove the last value.
						Items.RemoveAt(Items.Count - 1);


						// Start at the first index.
						int i = 0;

						// "Bubble up" the heap if there are other items.
						if (Items.Count > 0) {
							// Continue until the halfway point of the heap.
							while (i < Items.Count / 2) {
								// Continue along with the next left child.
								int j = (2 * i) + 1;
								// If j isn't last item AND left child < right child
								if ((j < Items.Count - 1) && (Comparer.Compare(Items[j], Items[j + 1]) > 0)) {
									// Go to the right child
									j++;
								}
								// If the last item is smaller than both siblings at the
								// current height, break.
								if (Comparer.Compare(Items[j], temporary) > 0) {
									break;
								}
								// Move the item at index j up one level.
								Items[i] = Items[j];
								// Move index i to the appropriate branch.
								i = j;
							}
							// Place the temporarily deleted item back at the end of
							// the current branch.
							Items[i] = temporary;
						}

						if (HeapHasRemovedItem != null) {
							HeapHasRemovedItem(0, i);
						}

						// Return the original root value.
						return rootValue;
					}

					public void UpdateItem (int originalItemIndex, T updatedItem) {
						// Throw an exception if the heap is empty.
						if (Items.Count == 0) {
							throw new InvalidOperationException("The heap is empty.");
						}

						// Presever the original index so it can be passed along to
						// subscribers of the HeapHasUpdatedItem event.
						int i = originalItemIndex;

						if (Items.Count > 1) {
							// Move up towards the root if the updated item is smaller
							// than its parent.
							while (i > 0 && (Comparer.Compare(Items[(i - 1) / 2], updatedItem) > 0)) {
								// Swap parent down to current index.
								Items[i] = Items[(i - 1) / 2];
								// Advance into parent's spot.
								i = (i - 1) / 2;	
							}
							// Move down towards the leaves if the updated item is larger
							// than one of its children. Move along the branch of the
							// smaller of the two child items.
							while ((i < Items.Count) && (Comparer.Compare(updatedItem, DSM.Generics.Math.Min(Items[(i * 2) + 1], Items[(i * 2) + 2])) > 0)) {
								// Advance to first child.
								i = (i * 2) + 1;	
								// Advance to second child if necessary.
								if (Comparer.Compare(Items[i], Items[i + 1]) > 0) {
									i++;
								}
 								// Swap child up to parent index.
								Items[(i - 1) / 2] = Items[i];

								// Leave if the leaf layer has been reached.
								if (((i * 2) + 1) > Items.Count) {
									break;
								}
							}
							Items[i] = updatedItem;
						}

						if (HeapHasUpdatedItem != null) {
							HeapHasUpdatedItem(originalItemIndex, i);
						}
					}

					public override IEnumerator GetEnumerator () {
						return GetEnumerator();
					}

					IEnumerator<T> IEnumerable<T>.GetEnumerator () {
						foreach (T i in Items) {
							yield return i;
						}
					}
				}	
			}
		}
	}
}
