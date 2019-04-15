using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Client
{
    class ClientInfo
    {
        public int ID { get; set; }

        public int LocationX { get; set; }

        public int LocationY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string TargetID { get; set; }
    }
}
