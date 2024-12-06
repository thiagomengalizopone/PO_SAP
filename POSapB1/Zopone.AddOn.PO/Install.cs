using sap.dev.core;
using sap.dev.core.MetaData;
using sap.dev.data;
using System;
using Zopone.AddOn.PO.Controller.Localizacao;
using Zopone.AddOn.PO.Model;
using Zopone.AddOn.PO.Model.SAP;

namespace Zopone.AddOn.PO
{
    public class Install
    {
        public static void VerificaInstalacaoAddOn()
        {
            try
            {
                Globals.Master.CurrentVersion = Util.RecuperaVersaoAtual();

                if (Globals.Master.CurrentVersion < Globals.Master.AddOnVersion)
                {
                    CreateMetaData.CriarMetaDataCore();


                    AtualizaCampoUrln8n();

                    if (SenhaBD.VerificaSenhaBD())
                        System.Windows.Forms.Application.Exit();

                    MetaData.CreateMetaData();

                    Instalar.ExecutarScripts(ScriptSQL.RetornaSQLScripts());

                    Instalar.AtualizarVersaoAtual();

                    Util.ExibeMensagensDialogoStatusBar("AddOn Atualizado para a versão mais recente.");
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao instalador AddOn: {Ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Long, true);
            }
        }

        private static void AtualizaCampoUrln8n()
        {
            try
            {
                SqlUtils.DoNonQuery(@"UPDATE ""@ZPN_SETUP"" SET U_URLn8n = 'http://192.168.9.1:5678' WHERE ISNULL(U_URLn8n,'') =  ''");
            }
            catch
            {

            }
        }
    }
}
