using sap.dev.core;
using sap.dev.core.Controller;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zopone.AddOn.PO.Helpers;
using Zopone.AddOn.PO.UtilAddOn;

namespace Zopone.AddOn.PO.View.Obra
{
    public class FrmGerarObra : FormSDK
    {
        #region Propriedades
        public EditText EdPrefix { get; set; }
        public EditText EdCodIni { get; set; }
        public EditText EdQtde { get; set; }
        public EditText EdSufix { get; set; }
        public DataTable DtObras { get; set; }
        public Grid GdObras { get; set; }
        public Button BtGerar { get; set; }


        #endregion

        public FrmGerarObra(string CodigoObra = "") : base()
        {
            if (oForm == null)
                return;

            EdPrefix = (EditText)oForm.Items.Item("EdPrefix").Specific;
            EdPrefix.LostFocusAfter += EdPrefix_LostFocusAfter;

            EdCodIni = (EditText)oForm.Items.Item("EdCodIni").Specific;
            EdCodIni.LostFocusAfter += EdCodIni_LostFocusAfter;

            EdQtde = (EditText)oForm.Items.Item("EdQtde").Specific;
            EdQtde.LostFocusAfter += EdQtde_LostFocusAfter;

            EdSufix = (EditText)oForm.Items.Item("EdSufix").Specific;
            EdSufix.LostFocusAfter += EdSufix_LostFocusAfter;

            GdObras = (Grid)oForm.Items.Item("GdObras").Specific;

            DtObras = oForm.DataSources.DataTables.Item("DtObras");

            BtGerar = (Button)oForm.Items.Item("BtGerar").Specific;
            BtGerar.PressedAfter += BtGerar_PressedAfter;


            GerarCodigosObra(string.Empty, string.Empty, "0", string.Empty);


            oForm.Visible = true;

        }

        private void EdSufix_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            EnviarGerarCodigoObra();
        }

        private void EdQtde_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            EnviarGerarCodigoObra();
        }

        private void EdCodIni_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            EnviarGerarCodigoObra();
        }

        private void EdPrefix_LostFocusAfter(object sboObject, SBOItemEventArg pVal)
        {
            EnviarGerarCodigoObra();
        }

        public void EnviarGerarCodigoObra()
        {

            if (!string.IsNullOrEmpty(EdSufix.Value) &&
                !string.IsNullOrEmpty(EdCodIni.Value) &&
                !string.IsNullOrEmpty(EdQtde.Value) &&
                !string.IsNullOrEmpty(EdSufix.Value)
                )
                GerarCodigosObra(EdPrefix.Value, EdCodIni.Value, EdSufix.Value, EdQtde.Value);
        }

        private void GerarCodigosObra(string Prefixo, string CodigoInicial, string Sufixo, string Qtde)
        {
            try
            {
                DtObras.ExecuteQuery($"SP_ZPN_GeraCodigosObra '{Prefixo}', '{CodigoInicial}', '{Sufixo}','{Qtde}'");

                GdObras.Columns.Item(0).Editable = false;
                GdObras.Columns.Item(1).Editable = false;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar Código Obras: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }

        }

        private void BtGerar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo("Deseja gerar as obras no SAP B1? \n Obras já geradas, serão ignoradas!"))
                    return;

                Globals.Master.Connection.Database.StartTransaction();
                GerarProjetosSAPB1();
                Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_Commit);
                
                EnviarGerarCodigoObra();

                Util.ExibeMensagensDialogoStatusBar($"Fim da geração de obras!");

            }
            catch (Exception Ex)
            {
                if (Globals.Master.Connection.Database.InTransaction)
                    Globals.Master.Connection.Database.EndTransaction(BoWfTransOpt.wf_RollBack);

                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar Obras: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void GerarProjetosSAPB1()
        {
            string Code = string.Empty;

            GeneralService oGeneralService = null;
            GeneralData oGeneralData = null;
            GeneralDataParams oGeneralParams = null;
            CompanyService oCompanyService = null;
            oCompanyService = Globals.Master.Connection.Database.GetCompanyService();

            oGeneralService = oCompanyService.GetGeneralService("ZPN_OPRJ");

            for (int iRow = 0; iRow < DtObras.Rows.Count; iRow++)
            {
                if (string.IsNullOrEmpty(DtObras.GetValue(1, iRow).ToString()))
                {
                    Code = DtObras.GetValue(0, iRow).ToString();

                    Util.ExibirMensagemStatusBar($"Gerando obra {Code}");

                    oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                    oGeneralData.SetProperty("Code", Code);
                    oGeneralData.SetProperty("Code", Code);

                    oGeneralParams = oGeneralService.Add(oGeneralData);


                   UtilProjetos.SalvarProjeto(Code, Code);
                }
            }

        }


    }
}
