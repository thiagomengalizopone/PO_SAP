using sap.dev.core;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;

namespace Zopone.AddOn.PO.View.Alocação
{
    public class FrmAloca : FormSDK
    {
        #region Propriedades
        EditText EdEtapa { get; set; }
        EditText EdEtapaDescricao { get; set; }

        EditText EdItemCode { get; set; }
        EditText EdItemName { get; set; }
        ComboBox CbFilial { get; set; }

        #endregion

        public FrmAloca() : base()
        {
            if (oForm == null)
                return;

            EdEtapa = (EditText)oForm.Items.Item("EdEtap").Specific;
            EdEtapa.ChooseFromListAfter += EdEtapa_ChooseFromListAfter;
            EdEtapa = (EditText)oForm.Items.Item("EdEtapD").Specific;

            EdItemCode = (EditText)oForm.Items.Item("EdItemCode").Specific;
            EdItemCode.ChooseFromListAfter += EdItemCode_ChooseFromListAfter;
            EdItemName = (EditText)oForm.Items.Item("EdItemName").Specific;

            CbFilial = (ComboBox)oForm.Items.Item("CbFilial").Specific;

            CarregarDadosAlocacao();

            oForm.Visible = true;

        }

        private void CarregarDadosAlocacao()
        {
            try
            {
                Util.ComboBoxSetValoresValidosPorSQL(CbFilial, UtilScriptsSQL.SQL_Filial);
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

                EdItemCode.Value = ItemCode;
                EdItemName.Value = ItemName;
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
                string Descricao = Convert.ToString(aEvent.SelectedObjects.GetValue("Name", 0));

                EdEtapa.Value = Code;
                EdEtapaDescricao.Value = Descricao;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Etapa: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
