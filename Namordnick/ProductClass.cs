﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Namordnick
{
    public partial class Product
    {
        public string GetIcon //картинка продукта
        {
            get
            {
                if (Image != null)
                    return Image;
                return "Images/picture.png";
            }
        }
        public string GetTypeAndNameProduct //тип и название продукта
        {
            get
            {
                return ProductType.Title + " | " + Title;
            }
        }
        public string GetMaterials //список всех материалов
        {
            get
            {
                List<ProductMaterial> materials = ProductMaterial.Where(x => x.ProductID == ID).ToList();
                if (materials.Count > 0)
                {
                    string stroke = "";
                    foreach (ProductMaterial pm in materials)
                    {
                        stroke += pm.Material.Title + ", ";
                    }
                    stroke = stroke.Substring(0, stroke.Length - 2);
                    return stroke;
                }
                return "нет данных.";
            }
        }

        public string CostProduct
        {
            get
            {
                List<ProductMaterial> materials = ProductMaterial.Where(x => x.ProductID == ID).ToList();
                if (materials.Count > 0)
                {
                    float price = 0;
                    foreach (ProductMaterial pm in materials)
                    {
                        price += ((float)pm.Material.Cost / pm.Material.CountInPack) * (float)pm.Count;
                    }
                    return "" + price;
                }
                return "?";
            }
        }

        public SolidColorBrush ColorBack
        {
            get
            {
                if (MinCostForAgent > 2000) return new SolidColorBrush(Color.FromArgb(80, 255, 51, 51));
                else return new SolidColorBrush(Color.FromArgb(90, 176, 229, 253));
            }
        }
    }
}
