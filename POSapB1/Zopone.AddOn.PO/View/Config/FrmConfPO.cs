using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.Config
{
    public class FrmConfPO : FormSDK
    {
        #region Propriedades

        Button BtOk { get; set; }
        EditText EdCode { get; set; }
        EditText EdCardCodeH { get; set; }
        EditText EdCardNameH { get; set; }
        EditText EdItemCode { get; set; }
        EditText EdItemName { get; set; }
        EditText EdCardCodeE { get; set; }
        EditText EdCardNameE { get; set; }
        ComboBox CbUtilizacao { get; set; }
        #endregion

        public FrmConfPO() : base()
        {
            if (oForm == null)
                return;

            BtOk = (Button)oForm.Items.Item("1").Specific;
            EdCode = (EditText)oForm.Items.Item("EdCode").Specific;

            EdCardCodeH = (EditText)oForm.Items.Item("EdCardCode").Specific;
            EdCardCodeH.ChooseFromListAfter += EdCardCodeH_ChooseFromListAfter;
            EdCardNameH = (EditText)oForm.Items.Item("EdCardName").Specific;


            EdCardCodeE = (EditText)oForm.Items.Item("EdCardCodE").Specific;
            EdCardCodeE.ChooseFromListAfter += EdCardCodeE_ChooseFromListAfter;
            EdCardNameE = (EditText)oForm.Items.Item("EdCardNamE").Specific;

            EdItemCode = (EditText)oForm.Items.Item("EdItemCode").Specific;
            EdItemCode.ChooseFromListAfter += EdItemCode_ChooseFromListAfter;

            EdItemName = (EditText)oForm.Items.Item("EdItemName").Specific;

            CbUtilizacao = (ComboBox)oForm.Items.Item("CbUtili").Specific;

            oForm.Visible = true;

            CarregarCampos();

            CarregarDadosConfPO();
        }

        private void CarregarCampos()
        {
            Util.ComboBoxSetValoresValidosPorSQL(CbUtilizacao, UtilScriptsSQL.SQL_Utilizacao);
        }

        private void EdCardCodeE_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("CardCode", 0));
                string Name = Convert.ToString(aEvent.SelectedObjects.GetValue("CardName", 0));

                EdCardCodeE.Value = Code;
                EdCardNameE.Value = Name;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar item: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void EdCardCodeH_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("CardCode", 0));
                string Name = Convert.ToString(aEvent.SelectedObjects.GetValue("CardName", 0));

                EdCardCodeH.Value = Code;
                EdCardNameH.Value = Name;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar item: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void EdItemCode_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string Code = Convert.ToString(aEvent.SelectedObjects.GetValue("ItemCode", 0));
                string Name = Convert.ToString(aEvent.SelectedObjects.GetValue("ItemName", 0));

                EdItemCode.Value = Code;
                EdItemName.Value = Name;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar item: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }

        }


        private void CarregarDadosConfPO()
        {
            try
            {

                Boolean bExisteRegistro = SqlUtils.ExistemRegistros(@"SELECT 1 FROM ""@ZPN_CONFPO"" WHERE ""Code"" = '1'");

                if (bExisteRegistro)
                {
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    EdCode.Value = "1";
                    BtOk.Item.Click();
                }
                else
                {
                    oForm.Mode = BoFormMode.fm_ADD_MODE;
                    EdCode.Value = "1";
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar configurações de Importação PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            try
            {

                if (pVal.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    ConfiguracoesImportacaoPO.CarregarConfiguracoesPO();

                }

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro nos eventos: {Ex.Message}");
            }
        }
    }
}
