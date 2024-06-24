using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace Zopone.AddOn.PO.View.ConfiguracaoImportWMS
{
    public class FrmConfigImpWMS : FormSDK
    {
        #region Propriedades
        public EditText EdCode { get; set; }

        public Matrix MtConfiguraoesSequencial { get; set; }
        public Button BtAddSerie { get; set; }
        public Button BtDelSerie { get; set; }

        public Button BtOk { get; set; }
        
        public SAPbouiCOM.DBDataSource DbConfigWMS;
        public SAPbouiCOM.DBDataSource DbConfWLSLinhas;

        #endregion

        public FrmConfigImpWMS() : base()
        {
            if (oForm == null)
                return;

            EdCode = (EditText)oForm.Items.Item("EdCode").Specific;
            
            MtConfiguraoesSequencial = (Matrix)oForm.Items.Item("MtConfSQ").Specific;
           
            BtAddSerie = (Button)oForm.Items.Item("BtAddS").Specific;
            BtAddSerie.PressedAfter += BtAddUsuario_PressedAfter;
            BtDelSerie = (Button)oForm.Items.Item("BtDelS").Specific;
            MtConfiguraoesSequencial.AutoResizeColumns();

            BtOk = (Button)oForm.Items.Item("1").Specific;
            BtOk.PressedBefore += BtOk_PressedBefore;

            DbConfigWMS = oForm.DataSources.DBDataSources.Item("@ZPN_CONFWMS");
            DbConfWLSLinhas = oForm.DataSources.DBDataSources.Item("@ZPN_CONFWMSL");

            oForm.Visible = true;

            CarregarDadosTela();
        }

       

        private void CarregarDadosTela()
        {
            try
            {
                Util.MatrixComboBoxSetValoresValidosPorSQL(MtConfiguraoesSequencial, UtilScriptsSQL.SQL_Filial , "Filial");
                Util.MatrixComboBoxSetValoresValidosPorSQL(MtConfiguraoesSequencial, UtilScriptsSQL.SQL_SerieDocumento , "Serie");

                Recordset oRs = (Recordset)Globals.Master.Connection.Database.GetBusinessObject(BoObjectTypes.BoRecordset);

                oRs.DoQuery(@"SELECT 1 FROM ""@ZPN_CONFWMS"" WHERE ""Code"" = '1'");

                if (oRs.EoF)
                {
                    oForm.Mode = BoFormMode.fm_ADD_MODE;
                    EdCode.Value = "1";
                }
                else
                {
                    oForm.Mode = BoFormMode.fm_FIND_MODE;
                    EdCode.Value = "1";
                    BtOk.Item.Click();
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados em tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtOk_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                if (string.IsNullOrEmpty(EdCode.Value))
                {
                    EdCode.Value = "1";
                }
                BubbleEvent = true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao salvar registro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                BubbleEvent = false;
            }
        }


        private void BtAddUsuario_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                MtConfiguraoesSequencial.FlushToDataSource();
                DbConfWLSLinhas.InsertRecord(DbConfWLSLinhas.Size);
                MtConfiguraoesSequencial.LoadFromDataSourceEx();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao adicionar linha de usuário: {Ex.Message}", BoMessageTime.bmt_Medium, true);
            }
        }
    }
}
