using System.Web;

namespace Connector
{
    internal class ProcesInfo : ProcessInfo
    {
        private string v1;
        private string v2;

        public ProcesInfo(string v1, string v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public string V1
        {
            get => v1;

            set
            {
                v1 = value;
            }
        }

        public string V2
        {
            get => v2;

            set
            {
                v2 = value;
            }
        }

    }
}