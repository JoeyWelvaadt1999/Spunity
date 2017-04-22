using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions {
	public static class ListExtension {
		public static List<T> Combine<T>(this List<T> listOne, List<T> listTwo) {
			List<T> newList = new List<T> ();

			for (int i = 0; i < listOne.Count; i++) {
				newList.Add (listOne [i]);
			}

			for (int j = 0; j < listTwo.Count; j++) {
				newList.Add (listTwo [j]);
			}

			return newList;
		}
	}
}
