using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;

namespace Zopone.AddOn.PO.View.ContratoAlocacao
{
    public class FrmContAloca : FormSDK
    {
        #region Propriedades
        EditText EdCode { get; set; }
        Matrix MtAlocacao { get; set; }
        
        #endregion

        public FrmContAloca(string Code) : base()
        {
            if (oForm == null)
                return;

            EdCode = (EditText)oForm.Items.Item("EdCode").Specific;
            MtAlocacao = (Matrix)oForm.Items.Item("MtAloc").Specific;
            MtAlocacao.AutoResizeColumns();

            CarregarDadosAlocacao(Code);


            oForm.Visible = true;

        }

        private void CarregarDadosAlocacao(string Code)
        {
            try
            {
                SqlUtils.DoNonQuery($@"SP_ZPN_CRIAALOCACAOCONTRATO {Code}");

                oForm.Mode = BoFormMode.fm_FIND_MODE;
                EdCode.Value = Code;

                oForm.Items.Item("1").Click();

                MtAlocacao.AutoResizeColumns();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao abrir alocação contrato: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
