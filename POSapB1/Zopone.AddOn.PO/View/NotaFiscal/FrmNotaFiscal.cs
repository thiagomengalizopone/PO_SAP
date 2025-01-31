using sap.dev.core;
using sap.dev.core.ApiService_n8n;
using sap.dev.core.DTO;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.NotaFiscal
{
    public class FrmNotaFiscal : FormSDK
    {
        public Matrix oMtItens { get; set; }
        public Matrix oMtImpostos { get; set; }
        public Matrix oMtAlocacao { get; set; }


        public FrmNotaFiscal() : base()
        {
            if (oForm == null)
                return;

            oMtItens = (Matrix)oForm.Items.Item("MtItem").Specific;
            oMtImpostos = (Matrix)oForm.Items.Item("MtImpo").Specific;
            oMtAlocacao = (Matrix)oForm.Items.Item("MtAloca").Specific;

            oMtItens.AutoResizeColumns();
            oMtImpostos.AutoResizeColumns();
            oMtAlocacao.AutoResizeColumns();
        }

    }

}