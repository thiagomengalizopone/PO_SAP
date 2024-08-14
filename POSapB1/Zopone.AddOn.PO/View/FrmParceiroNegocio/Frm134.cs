using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.FrmParceiroNegocio
{
    public class Frm134 : FormSDK
    {

        public class ItensUIDs
        {

        }


        public Frm134() : base()
        {
            if (oForm == null)
                return;




        }

        internal static bool Interface_FormDataEvent(ref BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD ||
                             businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (!businessObjectInfo.BeforeAction)
                    {
                        string FormUID = businessObjectInfo.FormUID;

                        new Task(() => { EnviarDadosPCIAsync(FormUID); }).Start();

                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao salvar registro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }
        }


        private static async Task EnviarDadosPCIAsync(string formUID)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText EdCode = (EditText)oForm.Items.Item("5").Specific;

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZACLIENTE '{EdCode.Value}'";

                SqlUtils.DoNonQueryAsync(SQL_Query);

                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados da tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}