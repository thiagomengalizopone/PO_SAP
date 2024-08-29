﻿using sap.dev.core;
using sap.dev.core.Controller;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;
using Zopone.AddOn.PO.UtilAddOn;

namespace Zopone.AddOn.PO.View.Obra
{
    public class FrmObra : FormSDK
    {
        #region Propriedades
        public EditText EdCodeObra { get; set; }
        public EditText EdDescObra { get; set; }

        public EditText EdCodeContrato { get; set; }
        public EditText EdDescContrato { get; set; }

        public ComboBox CbFilial { get; set; }

        public ComboBox CbPais { get; set; }
        public ComboBox CbEstado { get; set; }
        public ComboBox CbCidade { get; set; }

        public ComboBox CbRegional { get; set; }
        public ComboBox CbPCG { get; set; }

        public ComboBox CbPaisCandidato { get; set; }
        public ComboBox CbEstadoCandidato { get; set; }
        public ComboBox CbCidadeCandidato { get; set; }

        public ComboBox CbClassificacaoObra { get; set; }

        public Matrix MtCandidato { get; set; }

        public Button BtOk { get; set; }

        public DBDataSource DBObra { get; set; }
        public DBDataSource DBObraCandidato { get; set; }

        public EditText EdIdent { get; set; }
        public EditText EdTipo { get; set; }

        public EditText EdNome { get; set; }
        public EditText EdDetent { get; set; }
        public EditText EdIdDete { get; set; }
        public EditText EdCEP { get; set; }
        public EditText EdTipoLog { get; set; }
        public EditText EdRua { get; set; }
        public EditText EdNum { get; set; }
        public EditText EdComp { get; set; }
        public EditText EdBairro { get; set; }

        public EditText EdLatitude { get; set; }

        public EditText EdLongitude { get; set; }

        public EditText EdAltitude { get; set; }
        public EditText EdEquipamento { get; set; }

        public Button BtAddCandidato { get; set; }

        public UserDataSource UsRowId { get; set; }

        public Grid GdListPO { get; set; }
        public DateTime DtListPO { get; set; }

        #endregion

        public FrmObra(string CodigoObra = "") : base()
        {
            if (oForm == null)
                return;

            EdCodeObra = (EditText)oForm.Items.Item("EdCode").Specific;
            EdDescObra = (EditText)oForm.Items.Item("EdDescOb").Specific;

            EdCodeContrato = (EditText)oForm.Items.Item("EdCodCont").Specific;
            EdDescContrato = (EditText)oForm.Items.Item("EdDescCont").Specific;
            EdDescContrato.ChooseFromListAfter += EdDescContrato_ChooseFromListAfter;

            MtCandidato = (Matrix)oForm.Items.Item("MtCandi").Specific;

            CbFilial = (ComboBox)oForm.Items.Item("CbFilial").Specific;

            CbPais = (ComboBox)oForm.Items.Item("CbPais").Specific;
            CbPais.ComboSelectAfter += CbPais_ComboSelectAfter;

            CbEstado = (ComboBox)oForm.Items.Item("CbEst").Specific;
            CbEstado.ComboSelectAfter += CbEstado_ComboSelectAfter;

            CbCidade = (ComboBox)oForm.Items.Item("CbCid").Specific;




            CbClassificacaoObra = (ComboBox)oForm.Items.Item("CbClassO").Specific;


            CbRegional = (ComboBox)oForm.Items.Item("CbRegion").Specific;
            CbPCG = (ComboBox)oForm.Items.Item("CbPCG").Specific;

            GdListPO = (Grid)oForm.Items.Item("GdObra").Specific;
            GdListPO.DoubleClickAfter += GdListPO_DoubleClickAfter;


            #region Aba Candidato
            EdIdent = (EditText)oForm.Items.Item("EdIdent").Specific;
            EdTipo = (EditText)oForm.Items.Item("EdTipo").Specific;
            EdNome = (EditText)oForm.Items.Item("EdNome").Specific;
            EdDetent = (EditText)oForm.Items.Item("EdDetent").Specific;
            EdIdDete = (EditText)oForm.Items.Item("EdIdDete").Specific;

            EdCEP = (EditText)oForm.Items.Item("EdCEP").Specific;
            EdTipoLog = (EditText)oForm.Items.Item("EdTipoLog").Specific;
            EdRua = (EditText)oForm.Items.Item("EdRua").Specific;
            EdNum = (EditText)oForm.Items.Item("EdNum").Specific;
            EdComp = (EditText)oForm.Items.Item("EdComp").Specific;
            EdBairro = (EditText)oForm.Items.Item("EdBairro").Specific;

            EdLatitude = (EditText)oForm.Items.Item("EdLat").Specific;
            EdLongitude = (EditText)oForm.Items.Item("EdLong").Specific;
            EdAltitude = (EditText)oForm.Items.Item("EdAlt").Specific;

            BtAddCandidato = (Button)oForm.Items.Item("BtAddCA").Specific;
            BtAddCandidato.PressedAfter += BtAddCandidato_PressedAfter;

            CbPaisCandidato = (ComboBox)oForm.Items.Item("CbPaisC").Specific;
            CbPaisCandidato.ComboSelectAfter += CbPaisCandidato_ComboSelectAfter;

            CbEstadoCandidato = (ComboBox)oForm.Items.Item("CbEstC").Specific;
            CbEstadoCandidato.ComboSelectAfter += CbEstadoCandidato_ComboSelectAfter;

            CbCidadeCandidato = (ComboBox)oForm.Items.Item("CbCidC").Specific;

            UsRowId = oForm.DataSources.UserDataSources.Add("UsRowId", BoDataType.dt_SHORT_NUMBER, 5);
            #endregion


            BtOk = (Button)oForm.Items.Item("1").Specific;
            BtOk.PressedBefore += BtOk_PressedBefore;

            CarregarDadosTela();

            CarregarObra(CodigoObra);

            PreencheCamposTela();

            DBObra = oForm.DataSources.DBDataSources.Item("@ZPN_OPRJ");
            DBObraCandidato = oForm.DataSources.DBDataSources.Item("@ZPN_OPRJ_CAND");

            MtCandidato.AutoResizeColumns();
            MtCandidato.DoubleClickAfter += MtCandidato_DoubleClickAfter;

            oForm.Visible = true;

        }

        private void GdListPO_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                FrmPO.MenuPO(GdListPO.DataTable.GetValue(0, 0).ToString());
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void MtCandidato_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.ColUID != "#")
                    return;

                if (pVal.Row < 0)
                    return;

                if (!Util.RetornarDialogo("Deseja editar o registro de candidato?"))
                    return;


                EdIdent.Value = DBObraCandidato.GetValue("U_Identif", pVal.Row - 1);
                EdTipo.Value = DBObraCandidato.GetValue("U_Tipo", pVal.Row - 1);
                EdNome.Value = DBObraCandidato.GetValue("U_Nome", pVal.Row - 1);
                EdDetent.Value = DBObraCandidato.GetValue("U_Detentora", pVal.Row - 1);
                EdIdDete.Value = DBObraCandidato.GetValue("U_IdDetentora", pVal.Row - 1);
                CbPais.Select(DBObraCandidato.GetValue("U_Pais", pVal.Row - 1));
                CbEstado.Select(DBObraCandidato.GetValue("U_Estado", pVal.Row - 1));
                CbCidade.Select(DBObraCandidato.GetValue("U_Cidade", pVal.Row - 1));
                EdRua.Value = DBObraCandidato.GetValue("U_Rua", pVal.Row - 1);
                EdTipoLog.Value = DBObraCandidato.GetValue("U_TipoLog", pVal.Row - 1);
                EdNum.Value = DBObraCandidato.GetValue("U_Numero", pVal.Row - 1);
                EdComp.Value = DBObraCandidato.GetValue("U_Complemento", pVal.Row - 1);
                EdCEP.Value = DBObraCandidato.GetValue("U_CEP", pVal.Row - 1);
                EdBairro.Value = DBObraCandidato.GetValue("U_Bairro", pVal.Row - 1);
                EdLatitude.Value = DBObraCandidato.GetValue("U_Latitude", pVal.Row - 1);
                EdLongitude.Value = DBObraCandidato.GetValue("U_Longitude", pVal.Row - 1);
                EdEquipamento.Value = DBObraCandidato.GetValue("U_Equip", pVal.Row - 1);
                oForm.DataSources.UserDataSources.Item("Altit").ValueEx = DBObraCandidato.GetValue("U_Altitude", pVal.Row - 1);

                UsRowId.ValueEx = pVal.Row.ToString();

                BtAddCandidato.Caption = "Atualizar";

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar candidato: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }

        private void AdicionaCandidato()
        {
            try
            {

                MtCandidato.FlushToDataSource();

                Util.DBDataSourceRemoveLinhasBranco(DBObraCandidato, "U_Identif");

                if (string.IsNullOrEmpty(EdIdent.Value))
                    return;

                int RowId = -1;

                if (!string.IsNullOrEmpty(UsRowId.ValueEx) && Convert.ToInt32(UsRowId.ValueEx) >= 0)
                    RowId = Convert.ToInt32(UsRowId.ValueEx) - 1;
                else
                {
                    DBObraCandidato.InsertRecord(DBObraCandidato.Size);
                    RowId = DBObraCandidato.Size - 1;
                }

                oForm.Freeze(true);

                DBObraCandidato.SetValue("U_Identif", RowId, EdIdent.Value);
                DBObraCandidato.SetValue("U_Tipo", RowId, EdTipo.Value);
                DBObraCandidato.SetValue("U_Nome", RowId, EdNome.Value);
                DBObraCandidato.SetValue("U_Detentora", RowId, EdDetent.Value);
                DBObraCandidato.SetValue("U_IdDetentora", RowId, EdIdDete.Value);
                DBObraCandidato.SetValue("U_Pais", RowId, CbPaisCandidato.Value);
                DBObraCandidato.SetValue("U_Estado", RowId, CbEstadoCandidato?.Value);
                DBObraCandidato.SetValue("U_Cidade", RowId, CbCidadeCandidato?.Value);
                DBObraCandidato.SetValue("U_Rua", RowId, EdRua.Value);
                DBObraCandidato.SetValue("U_TipoLog", RowId, EdTipoLog.Value);
                DBObraCandidato.SetValue("U_Numero", RowId, EdNum.Value);
                DBObraCandidato.SetValue("U_Complemento", RowId, EdComp.Value);
                DBObraCandidato.SetValue("U_CEP", RowId, EdCEP.Value);
                DBObraCandidato.SetValue("U_Bairro", RowId, EdBairro.Value);
                DBObraCandidato.SetValue("U_Latitude", RowId, EdLatitude.Value);
                DBObraCandidato.SetValue("U_Longitude", RowId, EdLongitude.Value);
                DBObraCandidato.SetValue("U_Altitude", RowId, EdAltitude.Value);
                DBObraCandidato.SetValue("U_Equip", RowId, EdEquipamento.Value);


                if (string.IsNullOrEmpty(DBObraCandidato.GetValue("U_Codigo", RowId)?.ToString()))
                    DBObraCandidato.SetValue("U_Codigo", RowId, SqlUtils.GetValue("SELECT NEXT VALUE FOR ZPN_SEQ_Candidato;"));


                EdIdent.Value = string.Empty;
                EdNome.Value = string.Empty;
                EdTipo.Value = string.Empty;
                EdDetent.Value = string.Empty;
                EdIdDete.Value = string.Empty;
                EdTipoLog.Value = string.Empty;
                EdRua.Value = string.Empty;
                EdNum.Value = string.Empty;
                EdComp.Value = string.Empty;
                EdCEP.Value = string.Empty;
                EdBairro.Value = string.Empty;
                EdLatitude.Value = string.Empty;
                EdLongitude.Value = string.Empty;

                oForm.DataSources.UserDataSources.Item("Altit").ValueEx = string.Empty;

                MtCandidato.LoadFromDataSourceEx();

                UsRowId.ValueEx = "-1";

                BtAddCandidato.Caption = "Adicionar";
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao adicionar candidato: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private void BtAddCandidato_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            AdicionaCandidato();
        }

        private void CbEstado_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(CbEstado.Value))
                Util.ComboBoxSetValoresValidosPorSQL(CbCidade, UtilScriptsSQL.SQL_Cidade(CbPais.Value, CbEstado.Value));
        }
        private void CbPais_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(CbPais.Value))
                Util.ComboBoxSetValoresValidosPorSQL(CbEstado, UtilScriptsSQL.SQL_Estado(CbPais.Value));
        }
        private void CbEstadoCandidato_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                oForm.Freeze(true);

                if (!string.IsNullOrEmpty(CbEstadoCandidato.Value))
                {
                    Util.ComboBoxSetValoresValidosPorSQL(CbCidadeCandidato, UtilScriptsSQL.SQL_Cidade(CbPaisCandidato.Value, CbEstadoCandidato.Value));
                    Util.MatrixComboBoxSetValoresValidosPorSQL(MtCandidato, UtilScriptsSQL.SQL_Cidade(CbPaisCandidato.Value, CbEstadoCandidato.Value), "CbCid");
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar cidades: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }
        private void CbPaisCandidato_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (!string.IsNullOrEmpty(CbPaisCandidato.Value))
            {
                Util.ComboBoxSetValoresValidosPorSQL(CbEstadoCandidato, UtilScriptsSQL.SQL_Estado(CbPaisCandidato.Value));
                Util.MatrixComboBoxSetValoresValidosPorSQL(MtCandidato, UtilScriptsSQL.SQL_Cidade(CbPaisCandidato.Value, CbCidadeCandidato.Value), "CbCid");
            }
        }

        private void CarregarObra(string codigoObra)
        {
            try
            {
                bool bExistente = SqlUtils.ExistemRegistros($@"SELECT 1 FROM ""@ZPN_OPRJ"" WHERE ""Code"" = '{codigoObra}'");

                if (bExistente)
                {
                    oForm.Mode = BoFormMode.fm_FIND_MODE;

                    EdCodeObra.Value = codigoObra;
                    BtOk.Item.Click();
                }
                else
                {
                    oForm.Mode = BoFormMode.fm_ADD_MODE;
                    EdCodeObra.Value = codigoObra;
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar obra: {Ex.Message}");
            }
        }

        private void PreencheCamposTela()
        {
            if (string.IsNullOrEmpty(CbPais.Value))
            {
                CbPais.Select("BR", BoSearchKey.psk_ByValue);

                if (oForm.Mode == BoFormMode.fm_OK_MODE)
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;
            }


            if (string.IsNullOrEmpty(CbPaisCandidato.Value))
            {
                CbPaisCandidato.Select("BR", BoSearchKey.psk_ByValue);
                Util.ComboBoxSetValoresValidosPorSQL(CbEstadoCandidato, UtilScriptsSQL.SQL_Estado(CbPaisCandidato.Value));

                if (oForm.Mode == BoFormMode.fm_OK_MODE)
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;

            }
        }

        private void CarregarDadosTela()
        {
            try
            {
                Util.ComboBoxSetValoresValidosPorSQL(CbFilial, UtilScriptsSQL.SQL_Filial);
                Util.ComboBoxSetValoresValidosPorSQL(CbPais, UtilScriptsSQL.SQL_Pais);
                Util.ComboBoxSetValoresValidosPorSQL(CbPaisCandidato, UtilScriptsSQL.SQL_Pais);
                Util.ComboBoxSetValoresValidosPorSQL(CbClassificacaoObra, UtilScriptsSQL.SQL_ClassificacaoObra);
                Util.ComboBoxSetValoresValidosPorSQL(CbRegional, UtilScriptsSQL.SQL_Regionais);
                Util.ComboBoxSetValoresValidosPorSQL(CbPCG, UtilScriptsSQL.SQL_PCG);


                Util.MatrixComboBoxSetValoresValidosPorSQL(MtCandidato, UtilScriptsSQL.SQL_Pais, "CbPais");
                Util.MatrixComboBoxSetValoresValidosPorSQL(MtCandidato, UtilScriptsSQL.SQL_Estado("BR"), "CbEst");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados: {Ex.Message}");
            }
        }

        private void EdDescContrato_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string CodeContrato = Convert.ToString(aEvent.SelectedObjects.GetValue("Number", 0));
                string DescContrato = Convert.ToString(aEvent.SelectedObjects.GetValue("Descript", 0));

                EdCodeContrato.Value = CodeContrato;
                EdDescContrato.Value = DescContrato;

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar contrato: {Ex.Message}");
            }

        }

        private void BtOk_PressedBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            try
            {
                BubbleEvent = true;
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao salvar registro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                BubbleEvent = false;
            }
        }


        private static async Task EnviarDadosPCIAsync(string formUID, bool bUpdate)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                Form oFormObra = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText EdCodeObra = (EditText)oFormObra.Items.Item("EdCode").Specific;

                string operacao = bUpdate ? "U" : "A";

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZAOBRA '{EdCodeObra.Value}', '{operacao}'";

                SqlUtils.DoNonQueryAsync(SQL_Query);

                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados da tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        internal static bool Interface_FormDataEvent(ref BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if (!businessObjectInfo.BeforeAction)
                {
                    if ((businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE))
                    {
                        SalvarProjeto(businessObjectInfo.FormUID);

                        SalvarCentroCusto(businessObjectInfo.FormUID);

                        string FormUID = businessObjectInfo.FormUID;
                        bool bUpdate = businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE;
                        new Task(() => { EnviarDadosPCIAsync(FormUID, bUpdate); }).Start();
                    }
                    else if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                    {
                        CarregarPO(businessObjectInfo.FormUID);
                    }
                }
                else
                {

                    if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD || businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        return ValidarDadosObra(businessObjectInfo.FormUID);
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



        private static bool ValidarDadosObra(string formUID)
        {
            try
            {
                string MensagemErro = string.Empty;

                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                DBDataSource oDB = oForm.DataSources.DBDataSources.Item("@ZPN_OPRJ");

                if (string.IsNullOrEmpty(oDB.GetValue("U_CodContrato", 0)))
                    MensagemErro += "\n Obrigatório selecionar Contrato";

                if (string.IsNullOrEmpty(oDB.GetValue("U_BPLId", 0)))
                    MensagemErro += "\n Obrigatório selecionar Filial";

                if (string.IsNullOrEmpty(oDB.GetValue("U_ClassOb", 0)))
                    MensagemErro += "\n Obrigatório selecionar Classificação da Obra";

                if (string.IsNullOrEmpty(oDB.GetValue("U_Regional", 0)))
                    MensagemErro += "\n Obrigatório selecionar Regional";

                if (!string.IsNullOrEmpty(MensagemErro))
                {
                    Util.ExibeMensagensDialogoStatusBar(MensagemErro, BoMessageTime.bmt_Medium, true);
                    return false;
                }

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar registro: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }

            return true;
        }

        private static void CarregarPO(string formUID)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                EditText EdCodObra = (EditText)oForm.Items.Item("EdCode").Specific;

                Grid oGridPO = (Grid)oForm.Items.Item("GdObra").Specific;
                DataTable DtObra = oForm.DataSources.DataTables.Item("DtObra");

                DtObra.ExecuteQuery($"ZPN_SP_LISTAOBRAPO '{EdCodObra.Value}'");

                oGridPO.DataTable = DtObra;

                for (int iCol = 0; iCol < oGridPO.Columns.Count; iCol++)
                    oGridPO.Columns.Item(iCol).Editable = false;

                oGridPO.SelectionMode = BoMatrixSelect.ms_Single;

                oGridPO.AutoResizeColumns();

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados de PO: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void SalvarCentroCusto(string formUID)
        {
            Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

            EditText EdCodeObra = (EditText)oForm.Items.Item("EdCode").Specific;
            EditText EdDescObra = (EditText)oForm.Items.Item("EdDescOb").Specific;

            if (SqlUtils.ExistemRegistros($@"SELECT 1 FROM OPRC WHERE ""U_Obra"" = '{EdCodeObra.Value}'"))
                return;

            Int32 Dimensao = Convert.ToInt32(SqlUtils.GetValue(@"SELECT Max(T0.""DimCode"") FROM ODIM T0 WHERE T0.""DimDesc"" = 'OBRA'"));
            string TipoCentroCusto = SqlUtils.GetValue(@"SELECT maX(CctCode) FROM OCCT WHERE CctName = 'Receitas'");
            CentroCusto.CriaCentroCusto(EdDescObra.Value, Dimensao, TipoCentroCusto, "", "", EdCodeObra.Value);


        }

        private static void SalvarProjeto(string FormUID)
        {
            Form oForm = Globals.Master.Connection.Interface.Forms.Item(FormUID);

            EditText EdCodeObra = (EditText)oForm.Items.Item("EdCode").Specific;
            EditText EdDescObra = (EditText)oForm.Items.Item("EdDescOb").Specific;
            ComboBox CbFilial = (ComboBox)oForm.Items.Item("CbFilial").Specific;

            if (CbFilial.Selected == null || string.IsNullOrEmpty(CbFilial.Value))
            {
                throw new Exception("Selecione a Filial!");
            }

            if (SqlUtils.ExistemRegistros($@"SELECT 1 FROM OPRJ WHERE ""PrjCode"" = '{EdCodeObra.Value}'"))
                return;

            CompanyService oCmpSrv = Globals.Master.Connection.Database.GetCompanyService();

            // Obter o serviço de projetos
            ProjectsService projectService = (ProjectsService)oCmpSrv.GetBusinessService(ServiceTypes.ProjectsService);

            // Criar um novo projeto
            Project project = (Project)projectService.GetDataInterface(ProjectsServiceDataInterfaces.psProject);

            project.Code = EdCodeObra.Value;
            project.Name = EdDescObra.Value;

            project.UserFields.Item("U_BPLName").Value = CbFilial.Selected.Description;

            // Adicionar o projeto
            projectService.AddProject(project);


        }
    }
}
