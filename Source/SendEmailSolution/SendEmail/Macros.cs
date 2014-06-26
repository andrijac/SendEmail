using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendEmail
{
	public static class Macros
	{
		private static List<Tuple<string, Func<string>>> MacroValues = new List<Tuple<string, Func<string>>>()
		{
			new Tuple<string, Func<string>>("{current_user}", () => { return System.Security.Principal.WindowsIdentity.GetCurrent().Name;}),
			new Tuple<string, Func<string>>("{now}", () => { return DateTime.Now.ToString("yyyy-MM-dd HH:mm"); })
		};

		public static string FormatMacros(string input)
		{
			string result = input;

			foreach (Tuple<string, Func<string>> kvp in MacroValues)
			{
				result = result.Replace(kvp.Item1, kvp.Item2());
			}

			return result;
		}
	}
}