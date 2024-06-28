using sap.dev.core;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Windows.Forms;

namespace Zopone.AddOn.PO.View.Alocação
{
    public class FrmAloca : FormSDK
    {
        #region Propriedades
        EditText EdEtapa { get; set; }
        EditText EdEtapaDescricao { get; set; }

        #endregion

        public FrmAloca() : base()
        {
            if (oForm == null)
                return;

            EdEtapa = (EditText)oForm.Items.Item("EdEtap").Specific;
            EdEtapa.ChooseFromListAfter += EdEtapa_ChooseFromListAfter;
            EdEtapa = (EditText)oForm.Items.Item("EdEtapD").Specific;

            oForm.Visible = true;

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
