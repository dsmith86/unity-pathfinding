namespace DSM {

	namespace DataStructures {

		namespace Generics {
			
			// SUMMARY
			// =======
			// Queues items in a binary heap based on their
			// priority. Returns items first for items with smaller
			// priority values (min heap).
			//
			// TYPE PARAMETERS
			// ===============
			// TValue : Value Type; TPriority : Priority Type
			//
			// PUBLIC VARIABLES
			// ================
			// bool Empty
			//// RETURNS true if the queue is empty.
			//
			// TPriority Front
			//// RETURNS the item at the front of the queue.
			//
			// int Count
			//// RETURNS the total count of items in the queue.
			//
			// PUBLIC FUNCTIONS
			// ================
			// void Push (TValue newValue, TPriority priority)
			//// Pushes the new item into the queue and assigns
			//// its position in the queue based on its priority.
			//
			// TValue Pop ()
			//// Pops the front item from the queue and RETURNS it.
			//
			// void Clear ()
			//// removes all items from the binary heap.
			//
			// void TrimExcess ()
			//// minimizes memory overhead if less than 90%
			//// of the heap's capacity is filled.
			//
			// IEnumerator GetEnumerator ()
			//// RETURNS the class's enumerator.

			using System.Collections;
			using System.Collections.Generic;
			using DSM.DataStructures.Generics;
			public class PriorityQueue<TValue, TPriority> : IEnumerable<TValue> {

				protected IComparer<TPriority> Comparer;
				// Holds all the items in the queue.
				private BinaryHeap<TPriority> Items;
				// Maps keys to their respective priorities.
				protected Dictionary<TPriority, TValue> KeyMap;

				// removes the need to instantiate with an IComparer
				// if the default will suffice.
				public PriorityQueue () : this(Comparer<TPriority>.Default) {}

				// contructs an instance with the appropriate IComparer
				public PriorityQueue (IComparer<TPriority> comparer) {
					Comparer = comparer;
					Items = new BinaryHeap<TPriority>(Comparer);
					KeyMap = new Dictionary<TPriority, TValue>();
				}

				public void Push (TValue newValue, TPriority priority) {
					// Add the priority to the heap.
					Items.Insert(priority);
					// Map the priority to its key.
					KeyMap.Add(priority, newValue);
				}

				public TValue Pop () {
					// Remove the item from the binary heap.
					TPriority priority = Items.RemoveRoot();
					// Get the key using the KeyMap dictionary.
					TValue key = KeyMap[priority];
					// Remove the mapped key from the dictionary.
					KeyMap.Remove(priority);
					// Return the key.
					return key;
				}

				public void Clear () {
					Items.Clear();
					KeyMap.Clear();
				}

				public void TrimExcess () {
					Items.TrimExcess();
				}

				public bool Empty {
					get {return (Items.Count == 0); }
				}

				public TPriority Front {
					get { return Items.Peek(); }
				}

				public int Count {
					get { return Items.Count; }
				}

				public virtual IEnumerator GetEnumerator () {
					return GetEnumerator();
				}

				IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator () {
					foreach (TValue i in Items) {
						yield return i;
					}
				}
			}
		}
	}
}