using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.DAL
{
    public class DALProducts
    {
        public static List<ProductDTO> GetAllProduct()
        {
            var products = Global.DB.Product.Where(x => x.isActive == true).ToList();
            var productDTOList = Mapper.Map<List<Product>, List<ProductDTO>>(products);

            return productDTOList;
        }
    }
}