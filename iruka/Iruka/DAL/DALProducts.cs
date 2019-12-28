using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System.Collections.Generic;
using System.Linq;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class DALProducts
    {
        public static List<ProductDTO> GetAllPendingProducts()
        {
            var products = Global.DB.Product
                .Where(x => x.IsActive && x.EventStatus == EventStatus.Pending)
                .OrderByDescending(x => x.ScheduleDate)
                .ToList();
            var toReturn = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDto = Mapper.Map<Product, ProductDTO>(product);
                productDto.ScheduleDate = Global.DateToString(product.ScheduleDate);

                toReturn.Add(productDto);
            }

            return toReturn;
        }

        public static List<ProductDTO> GetAllOnGoingProducts()
        {
            var products = Global.DB.Product
                .Where(x => x.IsActive && x.EventStatus == EventStatus.OnGoing)
                .OrderBy(x => x.Priority)
                .ToList();
            var toReturn = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDto = Mapper.Map<Product, ProductDTO>(product);
                productDto.ScheduleDate = Global.DateToString(product.ScheduleDate);

                toReturn.Add(productDto);
            }

            return toReturn;
        }

        public static List<ProductDTO> GetAllFinishedProducts()
        {
            var products = Global.DB.Product
                .Where(x => x.IsActive && x.EventStatus == EventStatus.Finished)
                .OrderByDescending(x => x.ModifiedDate)
                .ToList();
            var toReturn = new List<ProductDTO>();

            foreach (var product in products)
            {
                var productDto = Mapper.Map<Product, ProductDTO>(product);
                productDto.ScheduleDate = Global.DateToString(product.ScheduleDate);
                productDto.ModifiedDate = Global.DateToString(product.ModifiedDate);

                toReturn.Add(productDto);
            }

            return toReturn;
        }
    }
}