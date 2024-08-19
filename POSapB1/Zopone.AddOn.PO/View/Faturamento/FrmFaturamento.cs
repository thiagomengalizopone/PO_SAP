using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zopone.AddOn.PO.View.PO;

namespace Zopone.AddOn.PO.View.Faturamento
{
    public partial class FrmFaturamento : Form
    {
        private static Thread formThread;
        public FrmFaturamento()
        {
            InitializeComponent();
        }

        internal static void MenuFaturamento()
        {
            formThread = new Thread(new ThreadStart(OpenFormFaturamento));
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
        }

        private static void OpenFormFaturamento()
        {
            System.Windows.Forms.Application.Run(new FrmVerificaImportacaoPO());
        }


        private void mskDATA_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
