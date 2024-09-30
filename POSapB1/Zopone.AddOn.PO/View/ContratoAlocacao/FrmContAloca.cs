using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace Zopone.AddOn.PO.View.ContratoAlocacao
{
    public class FrmContAloca : FormSDK
    {
        #region Propriedades
        EditText EdCode { get; set; }
        StaticText StCont { get; set; }
        EditText EdCont { get; set; }
        Matrix MtAlocacao { get; set; }
        Button BtCopiar { get; set; }

        DBDataSource oDBAloca { get; set; }

        string AbsID { get; set; }

        #endregion

        public FrmContAloca(string Code) : base()
        {
            if (oForm == null)
                return;

            EdCode = (EditText)oForm.Items.Item("EdCode").Specific;
            MtAlocacao = (Matrix)oForm.Items.Item("MtAloc").Specific;

            BtCopiar = (Button)oForm.Items.Item("BtCopiar").Specific;
            BtCopiar.PressedAfter += BtCopiar_PressedAfter;

            EdCont = (EditText)oForm.Items.Item("EdCont").Specific;
            EdCont.ChooseFromListAfter += EdCont_ChooseFromListAfter;
            EdCont.Item.AffectsFormMode = false;

            StCont = (StaticText)oForm.Items.Item("StCont").Specific;

           
            MtAlocacao.ChooseFromListAfter += MtAlocacao_ChooseFromListAfter;
            MtAlocacao.AutoResizeColumns();

            oDBAloca = oForm.DataSources.DBDataSources.Item("@ZPN_ALOCONI");

            CarregarDadosAlocacao(Code);


            oForm.Visible = true;

        }

        private void BtCopiar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {

                if (!string.IsNullOrEmpty(AbsID))
                {
                    if (!string.IsNullOrEmpty(EdCont.Value) && Util.RetornarDialogo($"Deseja copiar os dados de alocação do Contrato {EdCont.Value} para o contrato atual?"))
                    {
                        oForm.Freeze(true);
                        CopiarDadosContratoOrigem(AbsID);
                        EdCode.Item.Click();


                    }
                    AbsID = string.Empty;
                    EdCont.Value = string.Empty;

                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao copiar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }

        private void EdCont_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                AbsID = aEvent.SelectedObjects.GetValue("AbsID", 0).ToString();
                EdCont.Value = aEvent.SelectedObjects.GetValue("Descript", 0).ToString();             
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Contrato Origem - {Ex.Message}", BoMessageTime.bmt_Medium, true);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void CopiarDadosContratoOrigem(string absID)
        {
            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordSet.DoQuery($@"SP_ZPN_COPIAALOCACAOCONTRATO {EdCode.Value}, {absID}");


            MtAlocacao.FlushToDataSource();

            Util.DBDataSourceRemoveLinhasBranco(oDBAloca, "U_CodAloc");


            while (!oRecordSet.EoF)
            {
                oDBAloca.InsertRecord(oDBAloca.Size);

                oDBAloca.SetValue("U_CodAloc", oDBAloca.Size - 1, oRecordSet.Fields.Item("U_CodAloc").Value.ToString());
                oDBAloca.SetValue("U_DescAloc", oDBAloca.Size - 1, oRecordSet.Fields.Item("U_DescAloc").Value.ToString());
                oDBAloca.SetValue("U_PC", oDBAloca.Size - 1, oRecordSet.Fields.Item("U_PC").Value.ToString());
                oRecordSet.MoveNext();
            }
            oDBAloca.InsertRecord(oDBAloca.Size);

            MtAlocacao.LoadFromDataSourceEx(true);

            oForm.Mode = BoFormMode.fm_UPDATE_MODE;



        }

        private void MtAlocacao_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oForm.Freeze(true);

                MtAlocacao.FlushToDataSource();

                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                Util.DBDataSourceRemoveLinhasBranco(oDBAloca, "U_CodAloc");

                int selectedCount = aEvent.SelectedObjects.Rows.Count;

                for (int iRow = 0; iRow < selectedCount; iRow++)
                {
                    oDBAloca.InsertRecord(oDBAloca.Size);

                    oDBAloca.SetValue("U_CodAloc", oDBAloca.Size - 1, aEvent.SelectedObjects.GetValue("Code", iRow).ToString());
                    oDBAloca.SetValue("U_DescAloc", oDBAloca.Size - 1, aEvent.SelectedObjects.GetValue("U_Desc", iRow).ToString());
                    oDBAloca.SetValue("U_PC", oDBAloca.Size - 1, "Y");
                }
                oDBAloca.InsertRecord(oDBAloca.Size);

                MtAlocacao.LoadFromDataSourceEx(true);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Alocações - {Ex.Message}", BoMessageTime.bmt_Medium, true);
            }
            finally
            {
                oForm.Freeze(false);
            }
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

                Util.MatrixInserirLinha(oForm, MtAlocacao, oDBAloca, true, true);

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao abrir alocação contrato: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}

