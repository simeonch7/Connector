using System.Web;

namespace Connector
{
    internal class ProcesInfo : ProcessInfo
    {
        private string v1;
        private string v2;

        public ProcesInfo(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}