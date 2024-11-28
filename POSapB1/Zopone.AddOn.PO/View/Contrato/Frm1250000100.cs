using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Alocacao;
using Zopone.AddOn.PO.View.ContratoAlocacao;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.Contrato
{
    public class Frm1250000100 : FormSDK
    {

        public class ItensUIDs
        {

        }

        public Frm1250000100() : base()
        {
            if (oForm == null)
                return;




        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            try
            {
                if (!pVal.BeforeAction)
                {
                    if (pVal.EventType == BoEventTypes.et_FORM_DRAW)
                    {
                        PosicionaTela(pVal.FormUID);
                    }
                    else if (pVal.EventType == BoEventTypes.et_FORM_RESIZE)
                    {
                        FormAutoResize(pVal.FormUID);
                    }

                }

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro nos eventos: {Ex.Message}");
            }
        }

        private static void PosicionaTela(string formUID)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);


                Item oItemRef = oFormContrato.Items.Item("1250000036");

                int Width = oItemRef.Width;


                oItemRef = oFormContrato.Items.Item("1250000039");

                Item oItem;
                EditText oEditText;
                StaticText oStaticText;

                Button BtAlocacao;

                int iTop = 205;



                oItem = oFormContrato.Items.Add("EdNroRH", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 390;
                oItem.Top = iTop;
                oItem.Width = 109;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;
                oItem.LinkTo = "StNroRH";

                oEditText = ((SAPbouiCOM.EditText)(oItem.Specific));
                oEditText.DataBind.SetBound(true, "OOAT", "U_CodigoRH");

                oItemRef = oFormContrato.Items.Item("1250000038");
                oItem = oFormContrato.Items.Add("StNroRH", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 280;
                oItem.Top = iTop;
                oItem.Width = 90;

                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;

                oItem.LinkTo = "EdNroRH";

                oItemRef = oFormContrato.Items.Item("EdNroRH");

                oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                oStaticText.Caption = "Número RH";


                oItem = oFormContrato.Items.Add("CbReg", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);

                oItem.Left = 390;
                oItem.Top = iTop + 19;
                oItem.Width = 109;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;
                oItem.LinkTo = "StNroRH";


                ComboBox CbRegional = ((SAPbouiCOM.ComboBox)(oItem.Specific));
                CbRegional.Item.DisplayDesc = true;
                CbRegional.ExpandType = BoExpandType.et_DescriptionOnly;

                CbRegional.DataBind.SetBound(true, "OOAT", "U_Regional");

                oItemRef = oFormContrato.Items.Item("StNroRH");
                oItem = oFormContrato.Items.Add("StReg", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 280;
                oItem.Top = iTop + 19;
                oItem.Width = 90;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;
                oItem.LinkTo = "CbReg";


                oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                oStaticText.Caption = "Regional";

                Util.ComboBoxSetValoresValidosPorSQL(CbRegional, UtilScriptsSQL.SQL_Regionais);

                Item oFolderRef = oFormContrato.Items.Item("1320000072");

                Item oNewFolderItem = oFormContrato.Items.Add("FldObra", BoFormItemTypes.it_FOLDER);
                oNewFolderItem.Left = oFolderRef.Left;
                oNewFolderItem.Top = oFolderRef.Top;
                oNewFolderItem.Width = oFolderRef.Width;
                oNewFolderItem.Height = oFolderRef.Height;

                Folder oNewFolder = (Folder)oNewFolderItem.Specific;
                oNewFolder.Caption = "Obras";
                oNewFolder.Pane = 99;
                oNewFolder.AutoPaneSelection = true;
                oNewFolder.GroupWith(oFolderRef.UniqueID);

                Item iGridObras = oFormContrato.Items.Add("GdObras", BoFormItemTypes.it_GRID);
                iGridObras.Left = 20;
                iGridObras.Top = oFolderRef.Top + 30;
                iGridObras.Width = 600;
                iGridObras.Height = 350;

                iGridObras.FromPane = 99;
                iGridObras.ToPane = 99;

                Grid oGridObras = (Grid)iGridObras.Specific;

                oGridObras.SelectionMode = BoMatrixSelect.ms_Single;

                DataTable DtObras = oFormContrato.DataSources.DataTables.Add("DtObras");

                oGridObras.DataTable = DtObras;

                oGridObras.DoubleClickAfter += OGridObras_DoubleClickAfter;

                CarregarObrasContrato(oFormContrato, "XXXX");

                oNewFolder.ClickAfter += ONewFolder_ClickAfter;

                oItemRef = oFormContrato.Items.Item("1250000002");

                oItem = oFormContrato.Items.Add("BtAlocacao", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = oItemRef.Left + oItemRef.Width + 10;
                oItem.Top = oItemRef.Top;
                oItem.Width = oItemRef.Width;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = 0;
                oItem.ToPane = 0;

                BtAlocacao = ((SAPbouiCOM.Button)(oItem.Specific));
                BtAlocacao.Caption = "Alocação";
                BtAlocacao.PressedAfter += BtAlocacao_PressedAfter;

                oFormContrato.Update();

                FormAutoResize(formUID);

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao abrir tela: {Ex.Message}");
            }
        }

        private static void FormAutoResize(string formUID)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);
                try
                {
                    oFormContrato.Freeze(true);

                    Item oItemGridObra = oFormContrato.Items.Item("GdObras");
                    Item oItemGridRef = oFormContrato.Items.Item("1250000044");

                    oItemGridObra.Top = oFormContrato.Items.Item("1250000031").Top;
                    oItemGridObra.Left = oItemGridRef.Left;
                    oItemGridObra.Width = oItemGridRef.Width;
                    oItemGridObra.Height = oItemGridRef.Height;

                    Item oItemAloca = oFormContrato.Items.Item("BtAlocacao");

                    oItemAloca.Top = oFormContrato.Items.Item("1250000002").Top;
                    oItemAloca.Left = oFormContrato.Items.Item("1250000002").Left + oFormContrato.Items.Item("1250000002").Width + 5;

                    Item oItem = oFormContrato.Items.Item("1250000043");
                    oItem.Top = oItem.Top + 40;

                    oItem = oFormContrato.Items.Item("1250000044");
                    oItem.Top = oItem.Top + 40;
                    oItem.Height = oItem.Height - 40;

                    oItem = oFormContrato.Items.Item("1320000055");
                    int iLeftLabel = oItem.Left;
                    int iTopLabel = oItem.Top;

                    oItem = oFormContrato.Items.Item("1320000056");
                    int iLeftText = oItem.Left;
                    int iTopText = oItem.Top;

                    iTopLabel += 17;
                    iTopText += 17;

                    oItem = oFormContrato.Items.Item("StNroRH");
                    oItem.Top = iTopLabel;
                    oItem.Left = iLeftLabel;

                    oItem = oFormContrato.Items.Item("EdNroRH");
                    oItem.Top = iTopText;
                    oItem.Left = iLeftText;

                    iTopLabel += 17;
                    iTopText += 17;

                    oItem = oFormContrato.Items.Item("StReg");
                    oItem.Top = iTopLabel;
                    oItem.Left = iLeftLabel;

                    oItem = oFormContrato.Items.Item("CbReg");
                    oItem.Top = iTopText;
                    oItem.Left = iLeftText;

                }
                catch (Exception Ex)
                {
                    Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, Ex.Message, Ex);
                }
                finally
                {
                    oFormContrato.Freeze(false);
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao posicionar form: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void BtAlocacao_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(pVal.FormUID);

                if (oFormContrato.Mode != BoFormMode.fm_OK_MODE)
                {
                    Util.ExibirDialogo("Formulário não pode estar em modo de edição!");
                    return;
                }

                EditText oEditText = ((SAPbouiCOM.EditText)(oFormContrato.Items.Item("1250000004").Specific));

                new FrmContAloca(oEditText.Value);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao abrir alocação: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void OGridObras_DoubleClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(pVal.FormUID);

                Grid oGridObras = (Grid)oFormContrato.Items.Item("GdObras").Specific;

                int iRow = oGridObras.Rows.SelectedRows.Item(0, BoOrderType.ot_RowOrder);

                new FrmObra(oGridObras.DataTable.GetValue(0, iRow).ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao editar dados obras: {Ex.Message}");
            }
        }

        private static void EnviarDadosPCIAsync(string AbsIdContrato)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZACONTRATO '{AbsIdContrato}'";

                SqlUtils.DoNonQuery(SQL_Query);
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados da tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        static int GetMonthsDifference(DateTime start, DateTime end)
        {
            // Calcula a diferença total de anos e meses
            int yearDifference = end.Year - start.Year;
            int monthDifference = end.Month - start.Month;

            // Soma os meses totais (anos * 12 + meses)
            return (yearDifference * 12) + monthDifference;
        }

        private static void EnviarDadosSeniorAsync(string AbsIdContrato)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados Senior!");

                using (var client = new SeniorContrato.rubi_Syncbr_zopone_integracaoContratoClient())
                {
                    System.Data.DataTable Contrato = SqlUtils.ExecuteCommand("SELECT " +
                        "T1.BpCode AS CardCode, " +     
                        "T2.U_IdSenior AS CodPnSenior, " +                        
                        "T1.CntctCode AS numContrato, " +  
                        "T1.StartDate AS datIni, " +  
                        "T1.EndDate AS datFim, " +    
                        "T1.Descript AS desCon, " +   
                        "T1.Remarks AS obsCon, " +     
                        "T3.U_IdSenior AS empRes, " + 
                        "T1.AbsID AS numCon, " +      
                        "T4.PlanAmtLC AS PlanTotal " +  
                        "FROM OOAT T1 " +                        
                        "INNER JOIN OCRD T2 ON T1.BpCode = T2.CardCode " +
                        "JOIN OBPL T3 ON T3.MainBPL = 'Y' " +
                        "JOIN OAT1 T4 ON T1.AbsID = T4.AgrNo " +
                        $"WHERE AbsID = '{AbsIdContrato}'");

                    if (Contrato.Rows.Count > 0)
                    {
                        string IdSeniorPN = Contrato.Rows[0]["CodPnSenior"].ToString().Trim();

                        #region cadastro de PN
                        if (IdSeniorPN.Equals(""))
                        {
                            Util.ExibirMensagemStatusBar("Cadastrando o PN na Senior, aguarde!", BoMessageTime.bmt_Medium, false);

                            using (var clientCont = new SeniorOutraEmpresa.rubi_Syncbr_zopone_integracaoOutraEmpresaClient())
                            {
                                BusinessPartners businessPartner = (BusinessPartners)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                                if (businessPartner.GetByKey(Contrato.Rows[0]["CardCode"].ToString()))
                                {
                                    client.ClientCredentials.UserName.UserName = ConfiguracoesImportacaoPO.UsuarioSenior;
                                    client.ClientCredentials.UserName.Password = ConfiguracoesImportacaoPO.SenhaSenior;

                                    var dadosEmpCont = new SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn();

                                    //Aqui definimos se é uma atualização ou inserção na Senior!
                                    //if (businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString() != "")
                                    //    dadosEmpCont.codOem = int.Parse(businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString());

                                    #region campos informados sim/não                                                                       

                                    dadosEmpCont.codOemSpecified = true;
                                    dadosEmpCont.codPaiSpecified = true;
                                    dadosEmpCont.codCepSpecified = true;
                                    dadosEmpCont.tipInsSpecified = true;
                                    
                                    dadosEmpCont.insConSpecified = false;
                                    dadosEmpCont.numCgcSpecified = false;
                                    
                                    dadosEmpCont.atiIssSpecified = false;
                                    dadosEmpCont.cnaPreSpecified = false;
                                    dadosEmpCont.codAtdSpecified = false;
                                    dadosEmpCont.codAtiSpecified = false;
                                    dadosEmpCont.codAtuSpecified = false;
                                    dadosEmpCont.codBaiSpecified = false;
                                    dadosEmpCont.codCidSpecified = false;
                                    dadosEmpCont.codCliSpecified = false;
                                    dadosEmpCont.codEveSpecified = false;
                                    dadosEmpCont.codForSpecified = false;
                                    dadosEmpCont.codFpaSpecified = false;
                                    dadosEmpCont.codMicSpecified = false;
                                    dadosEmpCont.codSinSpecified = false;
                                    dadosEmpCont.colAdmSpecified = false;
                                    dadosEmpCont.colExeSpecified = false;
                                    dadosEmpCont.colOpeSpecified = false;
                                    dadosEmpCont.dddFaxSpecified = false;
                                    dadosEmpCont.dddTelSpecified = false;
                                    dadosEmpCont.ddiFaxSpecified = false;
                                    dadosEmpCont.ddiTelSpecified = false;
                                    dadosEmpCont.empCraSpecified = false;
                                    dadosEmpCont.empProSpecified = false;
                                    dadosEmpCont.estCarSpecified = false;
                                    dadosEmpCont.folAdmSpecified = false;
                                    dadosEmpCont.folExeSpecified = false;
                                    dadosEmpCont.folOpeSpecified = false;
                                    dadosEmpCont.horIncSpecified = false;
                                    dadosEmpCont.indObrSpecified = false;
                                    dadosEmpCont.insProSpecified = false;
                                    dadosEmpCont.NCAEPFSpecified = false;
                                    dadosEmpCont.numCNOSpecified = false;
                                    dadosEmpCont.numCerSpecified = false;
                                    dadosEmpCont.perCofSpecified = false;
                                    dadosEmpCont.perCslSpecified = false;
                                    dadosEmpCont.perCsrSpecified = false;
                                    dadosEmpCont.perGcoSpecified = false;
                                    dadosEmpCont.perInsSpecified = false;
                                    dadosEmpCont.perIrfSpecified = false;
                                    dadosEmpCont.perIssSpecified = false;
                                    dadosEmpCont.perPisSpecified = false;
                                    dadosEmpCont.perRetSpecified = false;
                                    dadosEmpCont.qtdCanSpecified = false;
                                    dadosEmpCont.regAnsSpecified = false;
                                    dadosEmpCont.regCodSpecified = false;
                                    dadosEmpCont.retCofSpecified = false;
                                    dadosEmpCont.retCslSpecified = false;
                                    dadosEmpCont.retCsrSpecified = false;
                                    dadosEmpCont.retIrfSpecified = false;
                                    dadosEmpCont.retPisSpecified = false;
                                    dadosEmpCont.staBDCSpecified = false;
                                    dadosEmpCont.TInConSpecified = false;
                                    dadosEmpCont.TInProSpecified = false;
                                    dadosEmpCont.tabEveSpecified = false;
                                    dadosEmpCont.tipFatSpecified = false;
                                    dadosEmpCont.tipUsoSpecified = false;
                                    dadosEmpCont.ultPesSpecified = false;
                                    dadosEmpCont.viaCraSpecified = false;
                                    dadosEmpCont.viaProSpecified = false;

                                    #endregion

                                    #region Dados a serem inseridos/atualizados

                                    dadosEmpCont.nomOem = businessPartner.CardName;
                                    dadosEmpCont.empPub = "N";
                                    dadosEmpCont.conSef = "N";
                                    dadosEmpCont.codPai = 1;//1 = Brasil
                                    dadosEmpCont.apeOem = businessPartner.CardForeignName.Length > 40 ? businessPartner.CardForeignName.Substring(0, 39) : businessPartner.CardForeignName;
                                    dadosEmpCont.codCep = int.Parse(businessPartner.ZipCode.Replace("-", ""));
                                    //dadosEmpCont.endNum = businessPartner.EDocStreetNumber;
                                    dadosEmpCont.endOem = businessPartner.Address;
                                    dadosEmpCont.numTel = businessPartner.Phone1;
                                    dadosEmpCont.emaEmp = businessPartner.EmailAddress;
                                    //Erro no campo abaixo, foi criado como int e o formato não comporta o tamanho do CNPJ!
                                    //dadosEmpCont.numCgc = int.Parse(businessPartner.FiscalTaxID.TaxId0.Replace(".", "").Replace("-", "").Replace("/", ""));
                                    dadosEmpCont.tipIns = (businessPartner.FiscalTaxID.TaxId4 != null && businessPartner.FiscalTaxID.TaxId4.Trim().Length > 0) ? 3 : 1; //1 = CNPJ | 3 = CPF
                                    dadosEmpCont.iniVal = businessPartner.ValidFrom.Year != 1899 ? businessPartner.ValidFrom.ToString("dd/MM/yyyy") : "";
                                    dadosEmpCont.fimVal = businessPartner.ValidFrom.Year != 1899 ? businessPartner.ValidTo.ToString("dd/MM/yyyy") : "";
                                    dadosEmpCont.insEst = businessPartner.FiscalTaxID.TaxId1;
                                    dadosEmpCont.codEst = businessPartner.Addresses.State;
                                    dadosEmpCont.endNum = businessPartner.Addresses.StreetNo;
                                    dadosEmpCont.endCpl = businessPartner.Addresses.BuildingFloorRoom;

                                    #endregion

                                    SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaOut retornoCont;

                                    int loopCont = 0;
                                    do
                                    {
                                        retornoCont = clientCont.OutraEmpresa(ConfiguracoesImportacaoPO.UsuarioSenior, ConfiguracoesImportacaoPO.SenhaSenior, 1, dadosEmpCont);

                                        if (loopCont == 3)
                                            break;
                                        loopCont++;
                                        //caso o erro seja de credenciais inválidas, tentar 3 vezes antes de gravar o erro!
                                    } while (retornoCont.erroExecucao != null && retornoCont.erroExecucao.Contains("Credenciais inválidas"));

                                    if (retornoCont.erroExecucao != null)
                                        throw new Exception("Falha ao integrar dados do PN na Senior, " + retornoCont.erroExecucao);
                                    else
                                    {
                                        businessPartner.UserFields.Fields.Item("U_IdSenior").Value = retornoCont.codOem.ToString();

                                        if (businessPartner.Update() != 0)
                                            throw new Exception("Falha na atualização do PN no SAP, " + SAPDbConnection.oCompany.GetLastErrorDescription());
                                        else
                                        {
                                            Util.GravarLog(EnumList.EnumAddOn.GestaoContratos, EnumList.TipoMensagem.Sucesso, "Dados atualizados na Senior, PN: " + businessPartner.CardCode, new Exception(""));
                                            Util.ExibirMensagemStatusBar("Dados atualizados na Senior com suceso, PN: " + businessPartner.CardCode, BoMessageTime.bmt_Medium);
                                            IdSeniorPN = retornoCont.codOem.ToString();
                                        }
                                    }
                                }
                                else
                                    throw new Exception("Falha ao carregar dados do PN, entre em contato com a equipe de desenvolvimento!");
                            }
                        }
                        #endregion

                        client.ClientCredentials.UserName.UserName = ConfiguracoesImportacaoPO.UsuarioSenior;
                        client.ClientCredentials.UserName.Password = ConfiguracoesImportacaoPO.SenhaSenior;

                        var dadosContrato = new SeniorContrato.integracaoContratoContratoIn();

                        #region campos informados sim/não

                        dadosContrato.codOemSpecified = true;
                        dadosContrato.empResSpecified = true;                        
                        dadosContrato.valConSpecified = true;

                        dadosContrato.tabOrgSpecified = false;
                        dadosContrato.cadResSpecified = false;  
                        dadosContrato.numLocSpecified = false;                        
                        dadosContrato.tclResSpecified = false;                        

                        #endregion

                        #region Dados a serem inseridos/atualizados

                        dadosContrato.codOem = int.Parse(IdSeniorPN);                       
                        dadosContrato.datIni = DateTime.Parse(Contrato.Rows[0]["datIni"].ToString()).ToString("dd/MM/yyyy");
                        dadosContrato.datFim = DateTime.Parse(Contrato.Rows[0]["datFim"].ToString()).ToString("dd/MM/yyyy");
                        dadosContrato.empRes = int.Parse(Contrato.Rows[0]["empRes"].ToString().Trim());
                        dadosContrato.numCon = Contrato.Rows[0]["numCon"].ToString();
                        dadosContrato.desCon = Contrato.Rows[0]["desCon"].ToString();
                        dadosContrato.obsCon = Contrato.Rows[0]["obsCon"].ToString() + " - Valor Total Planejado: " + double.Parse(Contrato.Rows[0]["PlanTotal"].ToString()).ToString("C");

                        int meses = GetMonthsDifference(DateTime.Parse(Contrato.Rows[0]["datIni"].ToString()), DateTime.Parse(Contrato.Rows[0]["datFim"].ToString()));

                        dadosContrato.valCon = double.Parse(Contrato.Rows[0]["PlanTotal"].ToString()) / meses;
                        dadosContrato.tclRes = 1;
                        dadosContrato.tabOrg = 1;
                        dadosContrato.numLoc = 1;
                        dadosContrato.flowInstanceID = "1";
                        dadosContrato.flowName = "1";

                        #endregion

                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------- Aqruivo LOG
                        // Caminho para salvar o arquivo XML
                        string filePath = "\\\\srvsb1\\AnexosSAP1\\Anexos\\LOG_INT\\CONTRATO_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xml";

                        // Serializar o array para XML
                        XmlSerializer serializer = new XmlSerializer(typeof(SeniorContrato.integracaoContratoContratoIn));
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            serializer.Serialize(writer, dadosContrato);
                        }
                        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                        SeniorContrato.integracaoContratoContratoOut retorno;

                        int loop = 0;
                        do
                        {
                            retorno = client.Contrato(ConfiguracoesImportacaoPO.UsuarioSenior, ConfiguracoesImportacaoPO.SenhaSenior, 1, dadosContrato);

                            if (loop == 3)
                                break;
                            loop++;
                            //caso o erro seja de credenciais inválidas, tentar 3 vezes antes de gravar o erro!
                        } while (retorno.erroExecucao != null && retorno.erroExecucao.Contains("Credenciais inválidas"));

                        if (retorno.erroExecucao != null)
                            throw new Exception("Falha ao integrar dados do Contrato na Senior, " + retorno.erroExecucao);
                        else
                        {
                            Util.GravarLog(EnumList.EnumAddOn.GestaoContratos, EnumList.TipoMensagem.Sucesso, "Dados atualizados na Senior, Contrato: " + Contrato.Rows[0]["numContrato"].ToString(), new Exception(""));
                            Util.ExibirMensagemStatusBar("Dados atualizados na Senior com suceso, Contrato: " + Contrato.Rows[0]["numCon"].ToString(), BoMessageTime.bmt_Medium);
                        }
                    }
                    else
                        throw new Exception("Falha ao integrar dados do Contrato na Senior, entre em contato com a equipe de desenvolvimento!" + SAPDbConnection.oCompany.GetLastErrorDescription());
                }
            }
            catch (Exception Ex)
            {
                Util.GravarLog(EnumList.EnumAddOn.GestaoContratos, EnumList.TipoMensagem.Erro, Ex.Message, Ex);
                Util.ExibirMensagemStatusBar(Ex.Message, BoMessageTime.bmt_Medium, true);
            }
        }

        private static void CarregarDadosObra(string formUID)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText oEditContrato = (EditText)oFormContrato.Items.Item("1250000004").Specific;

                CarregarObrasContrato(oFormContrato, oEditContrato.Value);

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao carregar dados da tela: {Ex.Message}");
            }
        }

        private static void CarregarObrasContrato(Form oFormContrato, string CodContrato)
        {
            DataTable DtObras = oFormContrato.DataSources.DataTables.Item("DtObras");
            Grid oGridObras = (Grid)oFormContrato.Items.Item("GdObras").Specific;

            DtObras.ExecuteQuery($"ZPN_SP_LISTAOBRACONTRATO '{CodContrato}'");
            oGridObras.AutoResizeColumns();

            for (int iCol = 0; iCol < oGridObras.Columns.Count; iCol++)
                oGridObras.Columns.Item(iCol).Editable = false;

        }

        private static void ONewFolder_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(pVal.FormUID);

                oFormContrato.PaneLevel = 99;
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao selecionar Folder tela: {Ex.Message}");
            }
        }

        private static bool ValidarDadosContrato(string formUID)
        {
            try
            {
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);

                ComboBox oCbStatus = (ComboBox)(oFormContrato.Items.Item("1250000036").Specific);

                if (oCbStatus.Value != "A")
                {
                    Util.ExibeMensagensDialogoStatusBar("Status do contrato deve ser Autorizado!", BoMessageTime.bmt_Medium, true);
                    return false;
                }

                ComboBox oCbMetodo = (ComboBox)(oFormContrato.Items.Item("1320000060").Specific);

                if (oCbMetodo.Value != "M")
                {
                    Util.ExibeMensagensDialogoStatusBar("Método de acordo deve ser Monetário!", BoMessageTime.bmt_Medium, true);
                    return false;
                }

                Matrix oMtValores = (Matrix)(oFormContrato.Items.Item("1250000045").Specific);
                bool bMontante = true;

                if (oMtValores.RowCount < 1)
                    bMontante = false;

                string valorMontante = Util.MatrixGetValue(oMtValores, 1, "1320000039");

                if (string.IsNullOrEmpty(valorMontante) || Convert.ToDouble(valorMontante.Replace(".", "").Replace(",", ".").Replace("R$ ", "").Trim()) < 999999999999)
                    bMontante = false;

                if (!bMontante)
                {
                    Util.ExibeMensagensDialogoStatusBar("Necessário adicionar no montante de valores um valor superior a R$ 999.999.999.999,99!", BoMessageTime.bmt_Medium, true);
                    return false;
                }



            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao validar dados de contrato: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }
            return true;
        }



        internal static bool Interface_FormDataEvent(ref BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    if (!businessObjectInfo.BeforeAction)
                    {
                        CarregarDadosObra(businessObjectInfo.FormUID);
                    }
                }
                else if (businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD ||
                         businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (!businessObjectInfo.BeforeAction)
                    {

                        if (businessObjectInfo.ActionSuccess)
                        {
                            bool bAdd = businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD;
                            
                            Form oForm = Globals.Master.Connection.Interface.Forms.Item(businessObjectInfo.FormUID);

                            string AbsIdContrato = string.Empty;

                            if (bAdd)
                                AbsIdContrato = SqlUtils.GetValue("SELECT MAX(AbsID) FROM OOAT");
                            else
                                AbsIdContrato = ((EditText)oForm.Items.Item("1250000004").Specific).Value.ToString();


                            CriaContratoAlocacaoItem(AbsIdContrato);

                            EnviarDadosPCIAsync(AbsIdContrato);
                            new Task(() => { EnviarDadosSeniorAsync(AbsIdContrato); }).Start();
                        }
                    }
                    else
                    {
                        return ValidarDadosContrato(businessObjectInfo.FormUID);
                    }
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao executar dados: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);

                return false;
            }

            return true;
        }

        private static void CriaContratoAlocacaoItem(string absIdContrato)
        {
            try
            {
                SqlUtils.DoNonQuery($"SP_ZPN_CRIAALOCACAOCONTRATO {absIdContrato}");
            }
            catch (Exception Ex)
            {
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, $"Erro ao criar alocação contrato: {absIdContrato} - {Ex.Message}", Ex);
            }
        }
    }
}
