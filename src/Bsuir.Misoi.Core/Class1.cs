using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {
		public static string Hello
		{
			get
			{
				var b = new Bitmap(100, 100);
				return "Hello";
			}
		}

    }
}
