using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Media;
using Zopone.AddOn.PO.Helpers;
using Zopone.AddOn.PO.Model.Objects;

namespace Zopone.AddOn.PO.View.Faturamento
{
    public class FrmGeraLCMDesconto : FormSDK
    {
        EditText EdFatura { get; set; }
        EditText EdObra { get; set; }
        EditText EdCandidato { get; set; }
        EditText EdDataEmissao { get; set; }
        EditText EdDataDocumento { get; set; }
        EditText EdDataVencimento { get; set; }
        EditText EdDocEntry { get; set; }
        EditText EdCliente { get; set; }
        EditText EdClienteNome { get; set; }
        EditText EdValor { get; set; }
        EditText EdContaContabil { get; set; }
        ComboBox CbFilial { get; set; }

        Button BtGerarLCM { get; set; }
        Button BtCancelar { get; set; }

        public FrmGeraLCMDesconto() : base()
        {
            if (oForm == null)
                return;

            EdFatura = (EditText)(oForm.Items.Item("EdFatura").Specific);
            EdObra = (EditText)(oForm.Items.Item("EdObra").Specific);
            EdCandidato = (EditText)(oForm.Items.Item("EdCand").Specific);
            EdDataEmissao = (EditText)(oForm.Items.Item("EdDataE").Specific);
            EdDataDocumento = (EditText)(oForm.Items.Item("EdDataD").Specific);
            EdDataVencimento = (EditText)(oForm.Items.Item("EdDataV").Specific);
            EdCliente = (EditText)(oForm.Items.Item("EdCli").Specific);
            EdClienteNome = (EditText)(oForm.Items.Item("EdCliNome").Specific);
            EdValor = (EditText)(oForm.Items.Item("EdValor").Specific);
            EdDocEntry = (EditText)(oForm.Items.Item("EdDocEntry").Specific);
            EdContaContabil = (EditText)(oForm.Items.Item("EdContaC").Specific);

            CbFilial  = (ComboBox)(oForm.Items.Item("CbFilial").Specific);

            Util.ComboBoxSetValoresValidosPorSQL(CbFilial, UtilScriptsSQL.SQL_Filial);

            EdDocEntry.ChooseFromListAfter += EdDocEntry_ChooseFromListAfter;

            BtGerarLCM = (Button)(oForm.Items.Item("BtGerarLCM").Specific);
            BtGerarLCM.PressedAfter += BtGerarLCM_PressedAfter;

            BtCancelar = (Button)(oForm.Items.Item("BtCanc").Specific);
            BtCancelar.PressedAfter += BtCancelar_PressedAfter;
        }

        private void EdDocEntry_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string docEntry = Convert.ToString(aEvent.SelectedObjects.GetValue("DocEntry", 0));

                EdDocEntry.Value = docEntry;

                CarregarDadosNF(docEntry);
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar documento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void CarregarDadosNF(string docEntry)
        {
            try
            {
                System.Data.DataTable dtDadosNF = SqlUtils.ExecuteCommand($"SP_ZPN_GeraDadosLCMDesconto {docEntry}");

                if (dtDadosNF.Rows.Count >0) 
                {
                    EdObra.Value = dtDadosNF.Rows[0]["Project"].ToString();
                    EdCandidato.Value = dtDadosNF.Rows[0]["U_Candidato"].ToString();
                    EdDataEmissao.Value = Convert.ToDateTime(dtDadosNF.Rows[0]["CreateDate"]).ToString("yyyyMMdd");
                    EdDataDocumento.Value = Convert.ToDateTime(dtDadosNF.Rows[0]["DocDate"]).ToString("yyyyMMdd");
                    EdDataVencimento.Value = Convert.ToDateTime(dtDadosNF.Rows[0]["DocDueDate"]).ToString("yyyyMMdd");
                    EdCliente.Value = dtDadosNF.Rows[0]["CardCode"].ToString();
                    EdClienteNome.Value = dtDadosNF.Rows[0]["CardName"].ToString();
                    EdFatura.Value = dtDadosNF.Rows[0]["fatura"].ToString();
                    EdContaContabil.Value = dtDadosNF.Rows[0]["ContaContabil"].ToString();
                    CbFilial.Select(dtDadosNF.Rows[0]["BplID"].ToString(), BoSearchKey.psk_ByValue);
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados do documento: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void BtGerarLCM_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (!Util.RetornarDialogo($"Deseja gerar o LCM de desconto?"))
                    return;

                if (string.IsNullOrEmpty(EdValor.Value))
                    Util.ExibeMensagensDialogoStatusBar("Atenção: Obrigatório selecionar o Valor!", BoMessageTime.bmt_Medium, true);

                GerarLCMDesconto();
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar LCM desconto: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private void GerarLCMDesconto()
        {
            try
            {
                oForm.Freeze(true);

                SAPbobsCOM.JournalEntries oLCM = (SAPbobsCOM.JournalEntries)Globals.Master.Connection.Database.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);

                oLCM.DueDate = DateTime.ParseExact(EdDataVencimento.Value, "yyyyMMdd", null);
                oLCM.ProjectCode = EdObra.Value;
                oLCM.Reference  = $"{EdFatura.Value}D";

                oLCM.Lines.AccountCode = EdContaContabil.Value;
                oLCM.Lines.ProjectCode = EdObra.Value;
                oLCM.Lines.Reference1 = $"{EdFatura.Value}D";
                oLCM.Lines.Reference2 = EdDocEntry.Value;
                oLCM.Lines.Debit = Convert.ToDouble(EdValor.Value.Replace(",", "").Replace(".", ","));
                oLCM.Lines.BPLID = Convert.ToInt32(CbFilial.Value);
                oLCM.Lines.Add();
                oLCM.Lines.ShortName = EdCliente.Value;
                oLCM.Lines.ProjectCode = EdObra.Value;
                oLCM.Lines.Reference1 = $"{EdFatura.Value}D";
                oLCM.Lines.Reference2 = EdDocEntry.Value;
                oLCM.Lines.Credit = Convert.ToDouble(EdValor.Value.Replace(",", "").Replace(".", ","));
                oLCM.Lines.BPLID = Convert.ToInt32(CbFilial.Value);

                if (oLCM.Add() != 0)
                    throw new Exception($"Erro ao gerar LCM: {Globals.Master.Connection.Database.GetLastErrorDescription()}");
                else
                {
                    Util.ExibirMensagemStatusBar("LCM gerado com sucesso!");
                }

                GerarLancamentoPCI();

                LimparTelaLCM();


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao gerar LCM desconto: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
            finally
            {
                oForm.Freeze(false);
            }

        }

        private void GerarLancamentoPCI()
        {
            Int32 TransID = Convert.ToInt32(SqlUtils.GetValue($"SELECT MAX(TransID) FROM JDT1 WHERE Ref2 = '{EdDocEntry.Value}'"));

            UtilPCI.EnviarDadosLCMDesconto(TransID);
        }

        private void LimparTelaLCM()
        {
            EdDocEntry.Value = string.Empty;
            EdValor.Value = "0";
            EdObra.Value = string.Empty;
            EdCandidato.Value = string.Empty;
            EdDataEmissao.Value = string.Empty;
            EdDataDocumento.Value = string.Empty;
            EdDataVencimento.Value = string.Empty;
            EdCliente.Value = string.Empty;
            EdClienteNome.Value = string.Empty;
            EdFatura.Value = string.Empty;
            EdContaContabil.Value = string.Empty;
        }

        private void BtCancelar_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            if (Util.RetornarDialogo($"Deseja cancelar a inserção do LCM de desconto?"))
                oForm.Close();
        }
    }
}
