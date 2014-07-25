namespace DSM {

	namespace DataStructures {

		namespace Generics {

			namespace Mutables {

				// SUMMARY
				// =======
				// Queues items according to priority and allows key-value pairs 
				// to mutate.
				//
				// TYPE PARAMETERS
				// ===============
				// T : Value Type; U : Priority Type
				//
				// PUBLIC VARIABLES
				// ================
				//
				// PUBLIC FUNCTIONS
				// ================
				//
				//

				/*using System.Collections;
				using System.Collections.Generic;
				public class MutablePriorityQueue<T, U> : IEnumerable<T> {

					private IComparer<T> Comparer;

					// removes the need to instantiate with an IComparer
					// if the default will suffice.
					public MutablePriorityQueue () : this(Comparer<T>.Default) {}

					// contructs an instance with the appropriate IComparer
					public MutablePriorityQueue (IComparer<T> comparer) {
						Comparer = comparer;
					}

					public void Push(T value, U priority) {
					}

					public IEnumerator GetEnumerator () {
						return GetEnumerator();
					}

					IEnumerator<T> IEnumerable<T>.GetEnumerator () {
						foreach (T i in Items) {
							yield return i;
						}
					}
				}*/
			}
		}
	}
}
