using sap.dev.core;
using sap.dev.data;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.Helpers
{
    public static class UtilPCI
    {

        public static async Task EnviarDadosPCIAsync(string CodeObra, DateTime dataCriacao)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZAOBRA '{CodeObra}', '{dataCriacao.ToString("yyyyMMdd")}'";

                await SqlUtils.DoNonQueryAsync(SQL_Query);

                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao atualizar dados de obra: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }
}
