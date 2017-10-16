using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DbEntity
{
    [Table("Product", ColModeEnum.PointedFirst)]
    public class Product
    {
        [Col("Id", ColType.PK_AI)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public decimal Price { get; set; }

        public string PurchaseUrl { get; set; }

        public int? IsRecommand { get; set; }
    }
}
