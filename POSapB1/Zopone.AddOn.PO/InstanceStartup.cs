using sap.dev.core;
using static sap.dev.core.EnumList;

namespace Zopone.AddOn.PO
{
    public class InstanceStartup
    {
        public SAPConnection lConexao = null;

        public InstanceStartup(string[] aArguments)
        {
            lConexao = new SAPConnection("5645523035496D706C656D656E746174696F6E3A57323032373137333336353B2858173689633AAF602B3BD7E7780008B87783", aArguments[0]);

            sap.dev.core.AddOn oAddOn = new sap.dev.core.AddOn(lConexao, EnumAddOn.SapDevCore);





        }


    }
}
