using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class BaseEntity<T>
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }

        public void NewCreatedData(string userId)
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            CreatedBy = userId;
            ModifiedDate = DateTime.Now;
            ModifiedBy = userId;
        }

        public void SetIsActive(bool isActive, string userId)
        {
            IsActive = isActive;
            SetModifiedData(userId);
        }

        public void SetModifiedData(string userId)
        {
            ModifiedBy = userId;
            ModifiedDate = DateTime.Now;
        }

        public BaseEntity()
        {
            IsActive = true;
        }
    }
}
