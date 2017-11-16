using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DbEntity
{
    [Table("Product", ColModeEnum.PointedOnly)]
    public class Product
    {
        [Col("Id", ColType.PK_AI)]
        public int Id { get; set; }

        [Col("Name")]
        public string Name { get; set; }

        [Col("Title")]
        public string Title { get; set; }

        [Col("SubTitle")]
        public string SubTitle { get; set; }

        [Col("Price")]
        public decimal Price { get; set; }

        [Col("PurchaseUrl")]
        public string PurchaseUrl { get; set; }

        [Col("IsRecommand")]
        public int? IsRecommand { get; set; }

        public List<string> GalleryImages { get; set; }

        public List<string> ContentImages { get; set; }
    }
}
