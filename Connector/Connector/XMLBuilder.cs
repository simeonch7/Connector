using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    class XMLBuilder
    {
        private static readonly string queryItems = "SELECT * FROM Goods";

        static public string XMLBuild(bool isMssql)
        {
            if (isMssql)
            {
                return queryItems;

            }
            else
            {
                return queryItems;
            }
        }
    }
}
