using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace EntityLayer.DbEntity
{
    [Table("options", ColModeEnum.PointedOnly)]
    public class Options
    {
        [Col("option_id")]
        public ulong OptionId { get; set; }

        [Col("option_name")]
        public string OptionName { get; set; }

        [Col("option_value")]
        public string OptionValue { get; set; }
    }

}
