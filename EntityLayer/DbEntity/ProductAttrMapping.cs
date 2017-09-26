using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DbEntity
{
    [Table("ProductAttrMapping", ColModeEnum.PointedFirst)]
    public class ProductAttrMapping
    {
        public int ProductId { get; set; }

        public int AttrId { get; set; }

        public string AttrValue { get; set; }

        public int Idx { get; set; }

    }
}
