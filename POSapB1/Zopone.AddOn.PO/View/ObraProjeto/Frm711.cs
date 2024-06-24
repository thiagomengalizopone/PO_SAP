using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Controller.ParceiroNegocio;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.Contrato
{
    public class Frm711 : FormSDK
    {

        public class ItensUIDs
        {

        }


        public Frm711() : base()
        {
            if (oForm == null)
                return;




        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            try
            {
                if (!pVal.BeforeAction)
                {
                    if (pVal.EventType == BoEventTypes.et_DOUBLE_CLICK)
                    {
                        if (pVal.ColUID == "2")
                            CarregarDadosProjeto(pVal.FormUID, pVal.Row);
                    }
                    else if (pVal.EventType == BoEventTypes.et_FORM_DRAW)
                        CarregarTabelaAddOn();
                }

                return true;
            }
            catch (Exception Ex) 
            {
                throw new Exception($"Erro nos eventos: {Ex.Message}");
            }
        }

        private static void CarregarTabelaAddOn()
        {
            try
            {
                SqlUtils.DoNonQuery($@"ZPN_SP_GeraProjetoTabelaB1 '{Globals.Master.Connection.Database.UserName}'");
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao carregar dados do AddOn: {Ex.Message}");
            }
        }

        private static void CarregarDadosProjeto(string formUID, Int32 RowId)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                Matrix MtProjetos = (Matrix)oForm.Items.Item("3").Specific;

                string PrjCode = ((EditText)MtProjetos.Columns.Item("PrjCode").Cells.Item(RowId).Specific).Value;

                new FrmObra(PrjCode);



            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao carregar dados do projeto: {Ex.Message}");
            }
        }
    }
}