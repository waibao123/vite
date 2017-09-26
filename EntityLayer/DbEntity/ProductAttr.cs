using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DbEntity
{
    [Table("ProductAttr", ColModeEnum.PointedFirst)]
    public class ProductAttr
    {
        [Col("Id", ColType.PK_AI)]
        public int Id { get; set; }

        public string AttrName { get; set; }

    }
}
