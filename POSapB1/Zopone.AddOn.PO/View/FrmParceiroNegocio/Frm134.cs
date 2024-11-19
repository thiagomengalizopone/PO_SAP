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

                        new Task(() => { EnviarDadosPCIAsync(FormUID); }).Start();

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
                        client.ClientCredentials.UserName.UserName = ConfiguracoesImportacaoPO.UsuarioSenior;
                        client.ClientCredentials.UserName.Password = ConfiguracoesImportacaoPO.SenhaSenior;

                        SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn dadosEmp = new SeniorOutraEmpresa.integracaoOutraEmpresaOutraEmpresaIn();

                        #region campos informados sim/não

                        dadosEmp.codOemSpecified = true;

                        dadosEmp.atiIssSpecified = false;
                        dadosEmp.cnaPreSpecified = false;
                        dadosEmp.codAtdSpecified = false;
                        dadosEmp.codAtiSpecified = false;
                        dadosEmp.codAtuSpecified = false;
                        dadosEmp.codBaiSpecified = false;
                        dadosEmp.codCepSpecified = false;
                        dadosEmp.codCidSpecified = false;
                        dadosEmp.codCliSpecified = false;
                        dadosEmp.codEveSpecified = false;
                        dadosEmp.codForSpecified = false;
                        dadosEmp.codFpaSpecified = false;
                        dadosEmp.codMicSpecified = false;                        
                        dadosEmp.codPaiSpecified = false;
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
                        dadosEmp.insConSpecified = false;
                        dadosEmp.insProSpecified = false;
                        dadosEmp.NCAEPFSpecified = false;
                        dadosEmp.numCNOSpecified = false;
                        dadosEmp.numCerSpecified = false;
                        dadosEmp.numCgcSpecified = false;
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
                        dadosEmp.tipInsSpecified = false;
                        dadosEmp.tipUsoSpecified = false;
                        dadosEmp.ultPesSpecified = false;
                        dadosEmp.viaCraSpecified = false;
                        dadosEmp.viaProSpecified = false;

                        #endregion

                        #region Dados a serem inseridos/atualizados

                        dadosEmp.nomOem = businessPartner.CardName;
                        dadosEmp.empPub = "N";
                        dadosEmp.conSef = "S";                        

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

                            if (loop == 3)
                                break;
                            loop++;
                        //caso o erro seja de credenciais inválidas, tentar 3 vezes antes de gravar o erro!
                        }while (retorno.erroExecucao != null && retorno.erroExecucao.Contains("Credenciais inválidas"));                        
                        
                        if(retorno.erroExecucao != null)                        
                            throw new Exception("Falha ao integrar dados do PN na Senior, " + retorno.erroExecucao);   
                        else if (businessPartner.UserFields.Fields.Item("U_IdSenior").Value.ToString().Trim() != retorno.codOem.ToString())
                        {
                            businessPartner.UserFields.Fields.Item("U_IdSenior").Value = retorno.codOem.ToString();

                            if (businessPartner.Update() != 0)                            
                                throw new Exception("Falha na atualização do PN no SAP, " + SAPDbConnection.oCompany.GetLastErrorDescription());                                                            
                            else
                            {
                                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Sucesso, "Dados atualizados na Senior, PN: " + businessPartner.CardCode, new Exception(""));
                                Util.ExibirMensagemStatusBar("Dados atualizados na Senior com suceso, PN: " + businessPartner.CardCode, BoMessageTime.bmt_Medium);
                            }
                        }                        
                    }
                    else                    
                        throw new Exception("Falha ao carregar dados do PN, entre em contato com a equipe de desenvolvimento!");
                }       
            }
            catch (Exception Ex)
            {    
                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, Ex.Message, Ex);
                Util.ExibirMensagemStatusBar(Ex.Message, BoMessageTime.bmt_Medium, true);
            }
        }
    }
}