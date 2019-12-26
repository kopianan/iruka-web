using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Enum
    {
        public enum FeeType
        {
            [Display(Name = "Rp")] Rupiah,
            [Display(Name = "%")] Percent
        }

        public enum EventStatus
        {
            Pending,
            OnGoing,
            Finished
        }

        public enum InternalRoleEnum
        {
            Admin,
            Finance,
            [Display(Name = "Content Manager")]
            ContentManager,
        }

        public enum EndClientEnum
        {
            Groomer,
            Owner,
            Customer
        }

        public enum UserRoleEnum
        {
            None,
            SuperAdmin,
            Admin,
            Finance,
            [Display(Name = "Content Manager")]
            ContentManager,
            Groomer,
            Owner,
            Customer
        }

        public enum CouponType
        {
            Discount,
            Product
        }

        public enum TransactionType
        {
            Shop,
            Salon,
            Hotel,
            [Display(Name = "Vet Corner")]
            VetCorner,
            Others
        }
    }
}
