using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Repository.Entities
{
    public class ItemType
    {
        public int ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemTypeDescription { get; set; }
        public int? ItemParentType { get; set; }
        public int? CostCentreId { get; set; }
        public bool? ItemTypeIsActive { get; set; }
        public int? ItemTypeSortOrder { get; set; }
        public string ItemTypeAddedBy { get; set; }
        public DateTime? ItemTypeAddedWhen { get; set; }
        public DateTime? ItemTypeUpdatedWhen { get; set; }
        public string ItemTypeUpdatedBy { get; set; }
    }
}
