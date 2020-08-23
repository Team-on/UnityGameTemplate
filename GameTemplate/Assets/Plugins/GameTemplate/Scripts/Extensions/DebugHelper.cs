using System.Runtime.CompilerServices;
using System.Diagnostics;

public static class DebugHelper {
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCurrentMethod() {
		StackTrace st = new StackTrace();
		StackFrame sf = st.GetFrame(1);

		return sf.GetMethod().Name;
	}
}
