using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zopone.AddOn.PO.Model.Objects;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.FrmParceiroNegocio
{
    public class Frm134 : FormSDK
    {

        public class ItensUIDs
        {

        }

        public Frm134() : base()
        {
            if (oForm == null)
                return;
        }

        internal static bool Interface_FormDataEvent(ref BusinessObjectInfo businessObjectInfo)
        {
            try
            {
                if ((businessObjectInfo.EventType == BoEventTypes. et_FORM_DATA_ADD && businessObjectInfo.ActionSuccess) ||
                             businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                {
                    if (!businessObjectInfo.BeforeAction)
                    {
                        if (businessObjectInfo.ActionSuccess)
                        {
                            var temppo = "";
                        }

                        string FormUID = businessObjectInfo.FormUID;
                        bool isAdd = businessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD;

                        //new Task(() => { EnviarDadosPCIAsync(FormUID); }).Start();

                        new Task(() => { EnviarDadosSeniorAsync(FormUID, isAdd);  }).Start();
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

        private static async Task EnviarDadosPCIAsync(string formUID)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText EdCode = (EditText)oForm.Items.Item("5").Specific;

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZACLIENTE '{EdCode.Value}'";

                SqlUtils.DoNonQueryAsync(SQL_Query);

                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados da tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static async Task EnviarDadosSeniorAsync(string formUID, bool isAdd)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados Senior!");

                using (var client = new SeniorOutraEmpresa.rubi_Syncbr_zopone_integracaoOutraEmpresaClient())
                {
                    Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);
                    //EditText EdCode = (EditText)oForm.Items.Item("5").Specific;
                    string tempppp = ((EditText)oForm.Items.Item("5").Specific).Value;

                    BusinessPartners businessPartner = (BusinessPartners)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                    if (businessPartner.GetByKey(((EditText)oForm.Items.Item("5").Specific).Value))
                    {
                        //businessPartner.SaveXML(@"C:\Temp\pn.xml");

                        //businessPartner.coun

                        client.ClientCredentials.UserName.UserName = ConfiguracoesImportacaoPO.UsuarioSenior;
                        client.ClientCredentials.UserName.Password = ConfiguracoesImportacaoPO.SenhaSenior;

                        SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn dadosEmp = new SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn();

                        #region campos informados sim/não

                        dadosEmp.codOemSpecified = true;
                        dadosEmp.codPaiSpecified = true;
                        dadosEmp.codCepSpecified = true;
                        dadosEmp.tipInsSpecified = true;

                        dadosEmp.insConSpecified = false;
                        dadosEmp.numCgcSpecified = false;

                        dadosEmp.atiIssSpecified = false;
                        dadosEmp.cnaPreSpecified = false;
                        dadosEmp.codAtdSpecified = false;
                        dadosEmp.codAtiSpecified = false;
                        dadosEmp.codAtuSpecified = false;
                        dadosEmp.codBaiSpecified = false;                        
                        dadosEmp.codCidSpecified = false;
                        dadosEmp.codCliSpecified = false;
                        dadosEmp.codEveSpecified = false;
                        dadosEmp.codForSpecified = false;
                        dadosEmp.codFpaSpecified = false;
                        dadosEmp.codMicSpecified = false;
                        dadosEmp.codSinSpecified = false;
                        dadosEmp.colAdmSpecified = false;
                        dadosEmp.colExeSpecified = false;
                        dadosEmp.colOpeSpecified = false;
                        dadosEmp.dddFaxSpecified = false;
                        dadosEmp.dddTelSpecified = false;
                        dadosEmp.ddiFaxSpecified = false;
                        dadosEmp.ddiTelSpecified = false;
                        dadosEmp.empCraSpecified = false;
                        dadosEmp.empProSpecified = false;
                        dadosEmp.estCarSpecified = false;
                        dadosEmp.folAdmSpecified = false;
                        dadosEmp.folExeSpecified = false;
                        dadosEmp.folOpeSpecified = false;
                        dadosEmp.horIncSpecified = false;
                        dadosEmp.indObrSpecified = false;
                        dadosEmp.insProSpecified = false;
                        dadosEmp.NCAEPFSpecified = false;
                        dadosEmp.numCNOSpecified = false;
                        dadosEmp.numCerSpecified = false;                        
                        dadosEmp.perCofSpecified = false;
                        dadosEmp.perCslSpecified = false;
                        dadosEmp.perCsrSpecified = false;
                        dadosEmp.perGcoSpecified = false;
                        dadosEmp.perInsSpecified = false;
                        dadosEmp.perIrfSpecified = false;
                        dadosEmp.perIssSpecified = false;
                        dadosEmp.perPisSpecified = false;
                        dadosEmp.perRetSpecified = false;
                        dadosEmp.qtdCanSpecified = false;
                        dadosEmp.regAnsSpecified = false;
                        dadosEmp.regCodSpecified = false;
                        dadosEmp.retCofSpecified = false;
                        dadosEmp.retCslSpecified = false;
                        dadosEmp.retCsrSpecified = false;
                        dadosEmp.retIrfSpecified = false;
                        dadosEmp.retPisSpecified = false;
                        dadosEmp.staBDCSpecified = false;
                        dadosEmp.TInConSpecified = false;
                        dadosEmp.TInProSpecified = false;
                        dadosEmp.tabEveSpecified = false;
                        dadosEmp.tipFatSpecified = false;                        
                        dadosEmp.tipUsoSpecified = false;
                        dadosEmp.ultPesSpecified = false;
                        dadosEmp.viaCraSpecified = false;
                        dadosEmp.viaProSpecified = false;

                        #endregion

                        #region Dados a serem inseridos/atualizados

                        dadosEmp.nomOem = businessPartner.CardName;
                        dadosEmp.empPub = "N";
                        dadosEmp.conSef = "N";
                        dadosEmp.codPai = 1;//1 = Brasil
                        dadosEmp.apeOem = businessPartner.CardForeignName.Length > 40 ? businessPartner.CardForeignName.Substring(0, 39) : businessPartner.CardForeignName;
                        dadosEmp.codCep = int.Parse(businessPartner.ZipCode.Replace("-",""));
                        //dadosEmp.endNum = businessPartner.EDocStreetNumber;
                        dadosEmp.endOem = businessPartner.Address;
                        dadosEmp.numTel = businessPartner.Phone1;
                        dadosEmp.emaEmp = businessPartner.EmailAddress;
                        //Erro no campo abaixo, foi criado como int e o formato não comporta o tamanho do CNPJ!
                        //dadosEmp.numCgc = int.Parse(businessPartner.FiscalTaxID.TaxId0.Replace(".", "").Replace("-", "").Replace("/", ""));
                        dadosEmp.tipIns = (businessPartner.FiscalTaxID.TaxId4 != null && businessPartner.FiscalTaxID.TaxId4.Trim().Length > 0) ? 3 : 1; //1 = CNPJ | 3 = CPF
                        dadosEmp.iniVal = businessPartner.ValidFrom.Year != 1899 ? businessPartner.ValidFrom.ToString("dd/MM/yyyy") : "";
                        dadosEmp.fimVal = businessPartner.ValidFrom.Year != 1899 ? businessPartner.ValidTo.ToString("dd/MM/yyyy")   : "";
                        dadosEmp.insEst = businessPartner.FiscalTaxID.TaxId1;
                        dadosEmp.codEst = businessPartner.Addresses.State;
                        dadosEmp.endNum = businessPartner.Addresses.StreetNo;
                        dadosEmp.endCpl = businessPartner.Addresses.BuildingFloorRoom;

                        //Aqui definimos se é uma atualização ou inserção na Senior!
                        if (businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString() != "")
                            dadosEmp.codOem = int.Parse(businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString());

                        #endregion

                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------- Aqruivo LOG
                        // Caminho para salvar o arquivo XML
                        string filePath = "\\\\srvsb1\\AnexosSAP1\\Anexos\\LOG_INT\\CLIENTE_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xml";

                        // Serializar o array para XML
                        XmlSerializer serializer = new XmlSerializer(typeof(SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn));
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            serializer.Serialize(writer, dadosEmp);
                        }
                        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                        SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaOut retorno;
                        //return;
                        int loop = 0;
                        do
                        {
                            retorno = client.OutraEmpresa(ConfiguracoesImportacaoPO.UsuarioSenior, ConfiguracoesImportacaoPO.SenhaSenior, 1, dadosEmp);
                            //caso o erro seja de credenciais inválidas, tentar 3 vezes antes de gravar o erro!
                            if (loop == 3)
                                break;
                            loop++;                        
                        }while (retorno.erroExecucao != null && retorno.erroExecucao.Contains("Credenciais inválidas"));                        
                        
                        if(retorno.erroExecucao != null)                        
                            throw new Exception("Falha ao integrar dados do PN na Senior, " + retorno.erroExecucao);   
                        else if (businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString().Trim() != retorno.codOem.ToString())
                        {
                            businessPartner.UserFields.Fields.Item("U_IdSenior").Value = retorno.codOem.ToString();

                            if (businessPartner.Update() != 0)                            
                                throw new Exception("Falha na atualização do PN no SAP, " + SAPDbConnection.oCompany.GetLastErrorDescription());   
                        }

                        Util.GravarLog(EnumList.EnumAddOn.GestaoContratos, EnumList.TipoMensagem.Sucesso, "Dados atualizados na Senior, PN: " + businessPartner.CardCode, new Exception(""));
                        Util.ExibirMensagemStatusBar("Dados atualizados na Senior com suceso, PN: " + businessPartner.CardCode, BoMessageTime.bmt_Medium);
                    }
                    else                    
                        throw new Exception("Falha ao carregar dados do PN, entre em contato com a equipe de desenvolvimento!");
                }       
            }
            catch (Exception Ex)
            {    
                Util.GravarLog(EnumList.EnumAddOn.GestaoContratos, EnumList.TipoMensagem.Erro, Ex.Message, Ex);
                Util.ExibirMensagemStatusBar(Ex.Message, BoMessageTime.bmt_Medium, true);
            }
        }
    }
}