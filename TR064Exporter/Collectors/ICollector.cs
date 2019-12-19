using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TR064Exporter.Collectors
{
    interface ICollector
    {
        Task CollectAsync();
    }
}
