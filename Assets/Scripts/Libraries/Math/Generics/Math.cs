namespace DSM {

	namespace Generics {

		using System.Collections.Generic;
		public static class Math {

			public static T Max<T> (T x, T y) {
				return Max (x, y, Comparer<T>.Default);
			}

			public static T Max<T> (T x, T y, IComparer<T> comparer) {
				return (comparer.Compare(x, y) > 0) ? x : y;
			}

			public static T Min<T> (T x, T y) {
				return Min (x, y, Comparer<T>.Default);
			}

			public static T Min<T> (T x, T y, IComparer<T> comparer) {
				return (comparer.Compare(x, y) > 0) ? y : x;
			}
		}	
	}
}