using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DbEntity
{
    [Table("ProductCategory", ColModeEnum.PointedFirst)]
    public class ProductCategory
    {
        [Col("Id", ColType.PK_AI)]
        public int Id { get; set; }

        public int WebId { get; set; }

        public string CategoryName { get; set; }

    }
}
