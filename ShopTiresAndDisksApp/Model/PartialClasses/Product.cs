using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTiresAndDisksApp.Model
{
    public partial class Product
    {
        public string ImagePath { get {
                if (Image==null)
                {
                    return "..\\Images\\picture.png";
                }
                else
                {
                    return "..\\Images\\" + Image;
                }
                
            } }
        public string MaterialList
        {
            get
            {
                string materials = "";
                return materials;
            }
        }
    }
}
