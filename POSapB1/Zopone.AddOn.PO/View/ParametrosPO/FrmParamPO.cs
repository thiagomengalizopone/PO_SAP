using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.ParametrosPO
{
    public class FrmParamPO : FormSDK
    {
        #region Propriedades

        Button BtOk { get; set; }
        EditText EdCode { get; set; }

        EditText EdNroPO { get; set; }
        EditText EdNroLinha { get; set; }
        EditText EdQtdeFat { get; set; }
        EditText EdCodServ { get; set; }
        EditText EdItem { get; set; }
        EditText EdValUnit { get; set; }
        EditText EdValTot { get; set; }

        #endregion

        public FrmParamPO() : base()
        {
            if (oForm == null)
                return;

            BtOk = (Button)oForm.Items.Item("1").Specific;
            EdCode = (EditText)oForm.Items.Item("EdCode").Specific;

            EdNroPO = (EditText)oForm.Items.Item("EdNroPO").Specific;
            EdNroLinha = (EditText)oForm.Items.Item("EdNroLinha").Specific;
            EdQtdeFat = (EditText)oForm.Items.Item("EdQtdeFat").Specific;
            EdCodServ = (EditText)oForm.Items.Item("EdCodServ").Specific;
            EdItem = (EditText)oForm.Items.Item("EdItem").Specific;
            EdValUnit = (EditText)oForm.Items.Item("EdValUnit").Specific;
            EdValTot = (EditText)oForm.Items.Item("EdValTot").Specific;

            oForm.Visible = true;

            CarregarDadosParamPO();
        }



        private void CarregarDadosParamPO()
        {
            try
            {

                Boolean bExisteRegistro = SqlUtils.ExistemRegistros(@"SELECT 1 FROM ""@ZPN_PARAMPO"" WHERE ""Code"" = '1'");

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

        internal static bool Interface_FormDataEvent(BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (!businessObjectInfo.BeforeAction)
                        ConfiguracoesImportacaoPO.CarregarConfiguracoesPO();
                }

                return true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }
        }
    }
}
