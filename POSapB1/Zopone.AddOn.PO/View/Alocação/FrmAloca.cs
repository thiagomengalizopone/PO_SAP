using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.View.Alocação
{
    public class FrmAloca : FormSDK
    {
        #region Propriedades
        EditText EdEtapa { get; set; }
        EditText EdEtapaDescricao { get; set; }
        ComboBox CbFilial { get; set; }

        ComboBox CbEtapaFaturamento { get; set; }

        #endregion

        public FrmAloca() : base()
        {
            if (oForm == null)
                return;

            EdEtapa = (EditText)oForm.Items.Item("EdEtap").Specific;
            EdEtapa.ChooseFromListAfter += EdEtapa_ChooseFromListAfter;
            EdEtapaDescricao = (EditText)oForm.Items.Item("EdEtapD").Specific;

            CbEtapaFaturamento = (ComboBox)oForm.Items.Item("CbEtFat").Specific;
            

            CbFilial = (ComboBox)oForm.Items.Item("CbFilial").Specific;

            CarregarDadosAlocacao();

            oForm.Visible = true;

        }

        

        private void CarregarDadosAlocacao()
        {
            try
            {
                Util.ComboBoxSetValoresValidosPorSQL(CbFilial, UtilScriptsSQL.SQL_Filial);
                Util.ComboBoxSetValoresValidosPorSQL(CbEtapaFaturamento, UtilScriptsSQL.SQL_EtapaFaturamento);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados Etapa: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void EdItemCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string ItemCode = Convert.ToString(aEvent.SelectedObjects.GetValue("ItemCode", 0));
                string ItemName = Convert.ToString(aEvent.SelectedObjects.GetValue("ItemName", 0));

                
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Etapa: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void EdEtapa_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("Code", 0));
                string Descricao = Convert.ToString(aEvent.SelectedObjects.GetValue("U_Desc", 0));

                EdEtapa.Value = Code;
                EdEtapaDescricao.Value = Descricao;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Etapa: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
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

                Form oFormAloca = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText EdCodeAloca = (EditText)oFormAloca.Items.Item("EdCode").Specific;

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZAETAPA '{EdCodeAloca.Value}'";

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
