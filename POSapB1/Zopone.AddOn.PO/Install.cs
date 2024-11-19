using sap.dev.core;
using sap.dev.core.MetaData;
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

                    if (SenhaBD.VerificaSenhaBD())
                        System.Windows.Forms.Application.Exit();

                    MetaData.CreateMetaData();

                    Instalar.ExecutarScripts(ScriptSQL.RetornaSQLScripts());

                    Instalar.ExecutarScriptsAtualizacao();

                    Instalar.AtualizarVersaoAtual();

                    Util.ExibeMensagensDialogoStatusBar("AddOn Atualizado para a versão mais recente.");
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao instalador AddOn: {Ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Long, true);
            }
        }


    }
}
