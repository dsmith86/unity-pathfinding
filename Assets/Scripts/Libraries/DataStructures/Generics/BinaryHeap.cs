namespace DSM {

	namespace DataStructures {

		namespace Generics {

			// SUMMARY
			// =======
			// Arranges comparable items as a complete binary tree.
			// This implementation produces a min heap.
			//
			// TYPE PARAMETERS
			// ===============
			// T : Value Type
			//
			// PUBLIC VARIABLES
			// ================
			// int Count
			//// the number of elements in the binary heap.
			//
			// PUBLIC FUNCTIONS
			// ================
			// void Insert (T newItem)
			//// insert a new item onto the binary heap.
			//
			// T RemoveRoot ()
			//// removes the item at the root of the heap.
			//// RETURNS the item of type T.
			//
			// void Clear ()
			//// removes all items from the binary heap.
			//
			// T Peek ()
			//// peeks at the root item without removing it.
			//// RETURNS the value of the root item.
			//
			// void TrimExcess ()
			//// minimizes memory overhead if less than 90%
			//// of the heap's capacity is filled.
			//
			// IEnumerator GetEnumerator ()
			//// RETURNS the class's enumerator.

			using System;
			using System.Collections;
			using System.Collections.Generic;
			public class BinaryHeap<T> : IEnumerable<T> {

				protected IComparer<T> Comparer;
				// Holds all the items in the heap.
				protected List<T> Items;

				// removes the need to instantiate with an IComparer
				// if the default will suffice.
				public BinaryHeap () : this(Comparer<T>.Default) {}

				// contructs an instance with the appropriate IComparer
				public BinaryHeap (IComparer<T> comparer) {
					Comparer = comparer;
					Items = new List<T>();
				}

				public virtual void Insert (T newItem) {

					int i = Count;

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
				}

				public virtual T RemoveRoot () {
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

					// "Bubble up" the heap if there are other items.
					if (Items.Count > 0) {
						// Start at the first index.
						int i = 0;
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
					// Return the original root value.
					return rootValue;
				}

				public int Count {
					get { return Items.Count; }
				}

				public void Clear () {
					Items.Clear();
				}

				public T Peek () {
					if (Items.Count == 0) {
						throw new InvalidOperationException("The heap is empty.");
					}
					return Items[0];
				}

				public T Back () {
					if (Items.Count == 0) {
						throw new InvalidOperationException("The heap is empty.");
					}
					return Items[Count - 1];
				}

				public void TrimExcess () {
					Items.TrimExcess();
				}

				public virtual IEnumerator GetEnumerator () {
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
