using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    interface IEntity
    {
        IEnumerable<KeyValuePair<string, string>> ToKeyValuePair();
    }
}
