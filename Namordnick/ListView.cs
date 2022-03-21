using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Namordnick
{
    public partial class Product
    {
        public string MainImagePath
        {
            get
            {
                string path = Image;
                if (path == null)
                {
                    path = "\\products\\default.jpg";
                }
                return path;
            }
        }

        public string FullTitle
        {
            get
            {
                return ProductType.Title + " | " + Title;
            }
        }
    }
}
