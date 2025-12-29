using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastracture.Keyclock
{
    public class KeyclockOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string MetadataAddress { get; set; } = default!;
        public bool RequireHttpsMetadata { get; set; }
    }
}
