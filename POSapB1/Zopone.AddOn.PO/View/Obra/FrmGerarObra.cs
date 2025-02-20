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
        public EditText EdQtde { get; set; }
        public DataTable DtObras { get; set; }
        public Matrix MtObras { get; set; }
        public Button BtGerar { get; set; }
        public Button BtValidar { get; set; }
        public UserDataSource USValidou { get; set; }


        public Button BtAtualizar { get; set; }


        #endregion

        public FrmGerarObra(string CodigoObra = "") : base()
        {
            if (oForm == null)
                return;

            EdPrefix = (EditText)oForm.Items.Item("EdPrefix").Specific;

            EdQtde = (EditText)oForm.Items.Item("EdQtde").Specific;

            MtObras = (Matrix)oForm.Items.Item("MtObras").Specific;

            DtObras = oForm.DataSources.DataTables.Item("DtObras");

            BtGerar = (Button)oForm.Items.Item("BtGerar").Specific;
            BtGerar.PressedAfter += BtGerar_PressedAfter;

            BtAtualizar = (Button)oForm.Items.Item("BtAtu").Specific;
            BtAtualizar.PressedAfter += BtAtualizar_PressedAfter;

            BtValidar = (Button)oForm.Items.Item("BtValidar").Specific;
            BtValidar.PressedAfter += BtValidar_PressedAfter;

            USValidou = oForm.DataSources.UserDataSources.Item("Validou");

            MtObras.AutoResizeColumns();
            MtObras.DoubleClickAfter += MtObras_DoubleClickAfter;

            oForm.Visible = true;

        }


        private void MtObras_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.Row == 0)
                    return;

                string CodigoObra = DtObras.GetValue("CodObra", pVal.Row - 1).ToString().Trim();

                if (string.IsNullOrEmpty(CodigoObra))
                    return;

                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                oRecordSet.DoQuery($@"SELECT 1 FROM ""@ZPN_OPRJ"" WHERE ""Code"" = '{CodigoObra}' ");

                if (oRecordSet.EoF)
                    return;

                new FrmObra(CodigoObra);



            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar obra - {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);

            }
        }

        private void BtValidar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oForm.Freeze(true);

                ValidarDados();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar códigos te obra - {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void ValidarDados()
        {
            var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            MtObras.FlushToDataSource();

            string ErroValida = string.Empty;

            USValidou.Value = "";

            for (int iRow = 0; iRow < DtObras.Rows.Count; iRow++)
            {
                ErroValida = string.Empty;

                string CodigoObra = DtObras.GetValue("CodObra", iRow).ToString().Trim();

                oRecordSet.DoQuery($@"SELECT 1 FROM ""@ZPN_OPRJ"" WHERE Code = '{CodigoObra}'");

                if (!oRecordSet.EoF)
                {
                    ErroValida += "Obra já existente | ";
                }


                string CodigoCliente = DtObras.GetValue("CodCli", iRow).ToString().Trim();

                oRecordSet.DoQuery($@"SELECT CardCode, CardName FROM OCRD WHERE CardCode Like '%0{CodigoCliente}'");

                if (!oRecordSet.EoF && !string.IsNullOrEmpty(CodigoCliente))
                {
                    DtObras.SetValue("Cliente", iRow, oRecordSet.Fields.Item("CardName").Value.ToString());
                    DtObras.SetValue("IdCliente", iRow, oRecordSet.Fields.Item("CardCode").Value.ToString());
                }
                else
                {
                    DtObras.SetValue("Cliente", iRow, string.Empty);
                    DtObras.SetValue("IdCliente", iRow, string.Empty);
                    ErroValida += "Cliente não encontrado | ";
                }


                string CodRegional = DtObras.GetValue("Regional", iRow).ToString().Trim();

                oRecordSet.DoQuery($@"SELECT T0.[PrcCode] FROM OPRC T0 WHERE T0.[PrcCode] like '%{CodRegional}%' AND T0.[DimCode] = 3");

                if (!oRecordSet.EoF && !string.IsNullOrEmpty(CodRegional))
                {
                    DtObras.SetValue("IdRegional", iRow, oRecordSet.Fields.Item("PrcCode").Value.ToString());
                }
                else
                {
                    DtObras.SetValue("IdRegional", iRow, string.Empty);
                    ErroValida += "Regional não encontrada | ";
                }

                string CodContrato = DtObras.GetValue("Contrato", iRow).ToString().Trim();

                oRecordSet.DoQuery($@"SELECT T0.[AbsID] FROM OOAT T0 WHERE T0.[Descript] like '%{CodContrato}%'");

                if (!oRecordSet.EoF && !string.IsNullOrEmpty(CodContrato))
                {
                    DtObras.SetValue("IdContrato", iRow, oRecordSet.Fields.Item("AbsID").Value.ToString());
                }
                else
                {
                    DtObras.SetValue("IdContrato", iRow, string.Empty);

                    ErroValida += "Contrato não encontrado | ";
                }


                string IdSite = DtObras.GetValue("Site", iRow).ToString().Trim();
                string Regional = DtObras.GetValue("IdRegional", iRow).ToString().Trim();

                string Localizacao = $"{IdSite}";

                DtObras.SetValue("Localizacao", iRow, Localizacao);


                if (!string.IsNullOrEmpty(ErroValida))
                {
                    DtObras.SetValue("Validacao", iRow, ErroValida);
                    MtObras.CommonSetting.SetCellFontColor(iRow + 1, 8, 255);
                    USValidou.Value = "E";
                }
                else
                    DtObras.SetValue("Validacao", iRow, string.Empty);
            }

            if (string.IsNullOrEmpty(USValidou.Value))
            {
                USValidou.Value = "V";
            }

            MtObras.LoadFromDataSourceEx();
            MtObras.AutoResizeColumns();

            if (USValidou.Value == "E")
                Util.ExibirMensagemStatusBar("Dados validados com erro. Verifique os dados informados!", BoMessageTime.bmt_Medium, true);
            else
                Util.ExibirMensagemStatusBar("Dados validados com sucesso!", BoMessageTime.bmt_Medium, false);

        }

        private void BtAtualizar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {

                if (!ValidarUsuarioObra())
                    return;

                if (!Util.RetornarDialogo("Deseja gerar os códigos em tela? \n A tabela abaixo será limpa e criada novamente!"))
                    return;

                EnviarGerarCodigoObra();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar códigos te obra - {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }



        public void EnviarGerarCodigoObra()
        {

            if (
                    !string.IsNullOrEmpty(EdPrefix.Value) &&
                    !string.IsNullOrEmpty(EdQtde.Value)
                )
                GerarCodigosObra(EdPrefix.Value, EdQtde.Value);
        }

        private void GerarCodigosObra(string Prefixo, string Qtde)
        {
            try
            {
                DtObras.ExecuteQuery($"SP_ZPN_GeraCodigosObra '{Prefixo}', '{Qtde}'");

                MtObras.Columns.Item("Col_0").DataBind.Bind("DtObras", "Site");
                MtObras.Columns.Item("Col_1").DataBind.Bind("DtObras", "CodCli");
                MtObras.Columns.Item("Col_2").DataBind.Bind("DtObras", "Cliente");
                MtObras.Columns.Item("Col_3").DataBind.Bind("DtObras", "Contrato");
                MtObras.Columns.Item("Col_4").DataBind.Bind("DtObras", "CodObra");
                MtObras.Columns.Item("Col_5").DataBind.Bind("DtObras", "Cadastro");
                MtObras.Columns.Item("Col_6").DataBind.Bind("DtObras", "Regional");
                MtObras.Columns.Item("Col_7").DataBind.Bind("DtObras", "IdContrato");
                MtObras.Columns.Item("Col_8").DataBind.Bind("DtObras", "IdRegional");
                MtObras.Columns.Item("Col_9").DataBind.Bind("DtObras", "IdCliente");
                MtObras.Columns.Item("Col_10").DataBind.Bind("DtObras", "Validacao");
                MtObras.Columns.Item("Col_11").DataBind.Bind("DtObras", "Localizacao");

                MtObras.LoadFromDataSourceEx();


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
                if (string.IsNullOrEmpty(USValidou.ValueEx))
                {
                    ValidarDados();
                }

                if (USValidou.ValueEx == "E")
                {
                    Util.ExibeMensagensDialogoStatusBar("Verifique e corrija as linhas com erro antes de prosseguir!", BoMessageTime.bmt_Medium, true);
                    return;
                }

                if (!Util.RetornarDialogo("Deseja gerar as obras no SAP B1? \n Obras já geradas, serão ignoradas!"))
                    return;

                GerarProjetosSAPB1Async();

                SqlUtils.DoNonQuery($"ZPN_SP_PCI_ATUALIZAOBRAPCG '', '{DateTime.Now.ToString("yyyy-MM-dd")}'");

                Util.ExibeMensagensDialogoStatusBar($"Fim da geração de obras!");

                USValidou.ValueEx = string.Empty;

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar Obras: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                FinalizarGerarObras();
            }
        }

        private bool ValidarUsuarioObra()
        {
            try
            {
                string Usuario = SqlUtils.GetValue("SELECT maX(Usuario) FROM ZPN_GERAROBRA");

                if (!string.IsNullOrEmpty(Usuario) && Usuario != Globals.Master.Connection.Database.UserName)
                {
                    if (!Util.RetornarDialogo($"ATENÇÃO: Usuário {Usuario} trabalhando com a tela. \n Deseja prosseguir com a tela?"))
                        return false;
                }

                SqlUtils.DoNonQuery($"INSERT INTO ZPN_GERAROBRA (Usuario, Hora) values ('{Globals.Master.Connection.Database.UserName}', Getdate());");

                return true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar usuários Gerar Obra: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }

        }

        private static void FinalizarGerarObras()
        {
            try
            {
                SqlUtils.DoNonQuery($"delete from ZPN_GERAROBRA where Usuario = '{Globals.Master.Connection.Database.UserName}'");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar liberar tela Gerar Obra: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private async Task GerarProjetosSAPB1Async()
        {
            string Code = string.Empty;
            string Localizacao = string.Empty;

            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'OBRA'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");

            string BplName = SqlUtils.GetValue("SELECT T0.[BPLName] FROM OBPL T0 WHERE T0.[BPLId] = 1");

            GeneralService oGeneralService = null;
            GeneralData oGeneralData = null;
            GeneralDataParams oGeneralParams = null;
            CompanyService oCompanyService = null;
            oCompanyService = Globals.Master.Connection.Database.GetCompanyService();

            oGeneralService = oCompanyService.GetGeneralService("ZPN_OPRJ");

            for (int iRow = 0; iRow < DtObras.Rows.Count; iRow++)
            {
                if (!string.IsNullOrEmpty(DtObras.GetValue("CodObra", iRow).ToString()))
                {
                    Code = DtObras.GetValue("CodObra", iRow).ToString();
                    Localizacao = DtObras.GetValue("Localizacao", iRow).ToString();


                    Util.ExibirMensagemStatusBar($"Gerando obra {Code}");

                    oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                    oGeneralData.SetProperty("Code", Code);
                    oGeneralData.SetProperty("Name", Localizacao);

                    oGeneralData.SetProperty("U_IdSite", DtObras.GetValue("Site", iRow).ToString());
                    oGeneralData.SetProperty("U_CodContrato", DtObras.GetValue("IdContrato", iRow).ToString());
                    oGeneralData.SetProperty("U_DescContrato", DtObras.GetValue("Contrato", iRow).ToString());
                    oGeneralData.SetProperty("U_BPLId", 1);
                    oGeneralData.SetProperty("U_VisPCI", "Y");

                    oGeneralData.SetProperty("U_CardCode", DtObras.GetValue("IdCliente", iRow).ToString());
                    oGeneralData.SetProperty("U_CardName", DtObras.GetValue("Cliente", iRow).ToString());
                    oGeneralData.SetProperty("U_Regional", DtObras.GetValue("IdRegional", iRow).ToString());
                    oGeneralData.SetProperty("U_Pais", "BR");

                    oGeneralParams = oGeneralService.Add(oGeneralData);

                    UtilProjetos.SalvarProjeto(Code, Code, BplName);

                    CentroCusto.CriaCentroCusto(Code, Dimensao, TipoCentroCusto, "", "", Code);

                    await UtilPCI.EnviarDadosObraPCIAsync(Code, DateTime.Now); //envia para pci a cada caso.

                }
            }


        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            try
            {
                if (pVal.EventType == BoEventTypes.et_FORM_CLOSE && !pVal.BeforeAction && pVal.ActionSuccess)
                    FinalizarGerarObras();
            }
            catch (Exception ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao executar eventos Gerar Obra: {ex.Message}", BoMessageTime.bmt_Medium, true, ex);
                return false;
            }

            return true;
        }
    }
}
