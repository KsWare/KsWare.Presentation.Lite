using System.Runtime.CompilerServices;


namespace KsWare.Presentation.Lite {

	// TODO move to Common project 
	internal static class ObjectDebugExtensions {

		//private static ObjectIDGenerator _idGenerator=new ObjectIDGenerator();
		private static readonly ConditionalWeakTable<object, object> _weakTable=new ConditionalWeakTable<object, object>();

		public static void SetDebugInformation(this object o, string information) {
			if(!_weakTable.TryGetValue(o, out var value))
			{
				_weakTable.Add(o, information);
			}
			else {
#if NETCOREAPP3x
				_weakTable.AddOrUpdate(o, value + "\n" + information);
#else
				_weakTable.Remove(o);
				_weakTable.Add(o, value + "\n" + information);
#endif

			}
		}

		public static string GetDebugInformation(this object o) {
			return _weakTable.TryGetValue(o, out var value) ? value as string : null;
		}

		public static bool TryGetDebugInformation(this object o, out string s) {
			var result = _weakTable.TryGetValue(o, out var value);
			s = result ? value as string : null;
			return result;
		}

		public static void DeleteDebugInformation(this object o) {
			_weakTable.Remove(o);
		}

		#region delete if ConditionalWeakTable is tested succesfully

		// private static List<Entry> _entries = new List<Entry>();

		// public static void SetDebugInformation(this object o, string information) {
		// 	var deathEntries = new List<Entry>();
		// 	foreach (var entry in _entries) {
		// 		if (!entry.WeakReference.IsAlive) {
		// 			deathEntries.Add(entry);
		// 			continue;
		// 		}
		//
		// 		if (ReferenceEquals(entry.WeakReference.Target, o)) {
		// 			var s = entry.Value as string;
		// 			s = s + (string.IsNullOrWhiteSpace(s) ? "" : "\n") + information;
		// 			entry.Value = s;
		// 		}
		// 	}
		//
		// 	foreach (var deathEntry in deathEntries) { _entries.Remove(deathEntry); }
		// }

		// public static string GetDebugInformation(this object o) {
		// 	var deathEntries = new List<Entry>();
		// 	var result = (string)null;
		// 	foreach (var entry in _entries) {
		// 		if (!entry.WeakReference.IsAlive) {
		// 			deathEntries.Add(entry);
		// 			continue;
		// 		}
		//
		// 		if (ReferenceEquals(entry.WeakReference.Target, o)) {
		// 			result = entry.Value as string;
		// 		}
		// 	}
		// 	foreach (var deathEntry in deathEntries) { _entries.Remove(deathEntry); }
		// 	return result;
		// }

		// public static bool TryGetDebugInformation(this object o, out string s) {
		// 	var deathEntries = new List<Entry>();
		// 	s = null;
		// 	var found = false;
		// 	foreach (var entry in _entries) {
		// 		if (!entry.WeakReference.IsAlive) {
		// 			deathEntries.Add(entry);
		// 			continue;
		// 		}
		//
		// 		if (ReferenceEquals(entry.WeakReference.Target, o)) {
		// 			s = entry.Value as string;
		// 			found = true;
		// 		}
		// 	}
		// 	foreach (var deathEntry in deathEntries) { _entries.Remove(deathEntry); }
		// 	return found;
		// }

		// public static void DeleteDebugInformation(this object o) {
		// 	var deathEntries = new List<Entry>();
		// 	var found = false;
		// 	foreach (var entry in _entries) {
		// 		if (!entry.WeakReference.IsAlive) {
		// 			deathEntries.Add(entry);
		// 			continue;
		// 		}
		//
		// 		if (!found && ReferenceEquals(entry.WeakReference.Target, o)) {
		// 			deathEntries.Add(entry);
		// 			found = true;
		// 		}
		// 	}
		//
		// 	foreach (var deathEntry in deathEntries) { _entries.Remove(deathEntry); }
		// }

		// private class Entry {
		// 	public WeakReference WeakReference { get; set; }
		// 	public object Value { get; set; }
		//
		// }
		#endregion
	}
}
