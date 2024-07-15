using sap.dev.core;
using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace Zopone.AddOn.PO.Model.SAP
{
    public class PurchaseOrderSAP
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int BplID { get; set; }
        public string U_NroPedido { get; set; }
        public double U_Valor { get; set; }
        public DateTime U_Data { get; set; }
        public string U_NroCont { get; set; }
        public DateTime U_DataVenc { get; set; }

        public string U_Status { get; set; }
        public string U_Desc { get; set; }
        public string U_Anexo { get; set; }

        public List<PurchaseOrderLine> Lines { get; set; }

        public PurchaseOrderSAP()
        {
            Lines = new List<PurchaseOrderLine>();
        }

        public class PurchaseOrderLine
        {
            public string U_Candidato { get; set; }
            public string U_Item { get; set; }
            public string U_ItemFat { get; set; }
            public string U_DescItemFat { get; set; }
            public string U_Parcela { get; set; }
            public string U_Tipo { get; set; }
            public DateTime U_DataLanc { get; set; }
            public DateTime U_DataFat { get; set; }
            public string U_NroNF { get; set; }
            public DateTime U_DataSol { get; set; }
            public string U_PrjCode { get; set; }
            public string U_PrjName { get; set; }
            public string U_CardCode { get; set; }
            public string U_CardName { get; set; }
            public string U_ItemCode { get; set; }
            public double U_Valor { get; set; }
            public string U_Obs { get; set; }
        }
        public void Add()
        {

            var generalService = ((CompanyService)Globals.Master.Connection.Database.GetCompanyService()).GetGeneralService("ZPN_ORDR");
            var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

            generalData.SetProperty("U_NroPedido", U_NroPedido);
            generalData.SetProperty("U_Valor", U_Valor);
            generalData.SetProperty("U_Data", U_Data);
            generalData.SetProperty("U_DataVenc", U_DataVenc);
            generalData.SetProperty("U_NroCont", U_NroCont);
            generalData.SetProperty("U_Status", U_Status);
            generalData.SetProperty("U_Desc", U_Desc);
            generalData.SetProperty("U_BplID", BplID);
            generalData.SetProperty("U_Anexo", U_Anexo);


            var lines = generalData.Child("ZPN_RDR1");
            foreach (var line in Lines)
            {
                var lineData = lines.Add();
                lineData.SetProperty("U_Candidato", line.U_Candidato);
                lineData.SetProperty("U_Item", line.U_Item);
                lineData.SetProperty("U_ItemFat", line.U_ItemFat);
                lineData.SetProperty("U_DescItemFat", line.U_DescItemFat);
                lineData.SetProperty("U_Parcela", line.U_Parcela);
                lineData.SetProperty("U_Tipo", line.U_Tipo);
                lineData.SetProperty("U_DataLanc", line.U_DataLanc);
                lineData.SetProperty("U_DataFat", line.U_DataFat);
                lineData.SetProperty("U_NroNF", line.U_NroNF);
                lineData.SetProperty("U_DataSol", line.U_DataSol);
                lineData.SetProperty("U_PrjCode", line.U_PrjCode);
                lineData.SetProperty("U_CardCode", line.U_CardCode);
                lineData.SetProperty("U_CardName", line.U_CardName);
                lineData.SetProperty("U_ItemCode", line.U_ItemCode);
                lineData.SetProperty("U_Valor", line.U_Valor);
                lineData.SetProperty("U_Obs", line.U_Obs);
            }

            generalService.Add(generalData);
        }

        public void Update()
        {
            var generalService = ((CompanyService)Globals.Master.Connection.Database.GetCompanyService()).GetGeneralService("ZPN_ORDR");
            var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            generalDataParams.SetProperty("DocEntry", DocEntry);
            var generalData = generalService.GetByParams(generalDataParams);

            generalData.SetProperty("U_NroPedido", U_NroPedido);
            generalData.SetProperty("U_Valor", U_Valor);
            generalData.SetProperty("U_Data", U_Data);
            generalData.SetProperty("U_DataVenc", U_DataVenc);
            generalData.SetProperty("U_NroCont", U_NroCont);
            generalData.SetProperty("U_Status", U_Status);
            generalData.SetProperty("U_Desc", U_Desc);
            generalData.SetProperty("U_BplID", BplID);
            generalData.SetProperty("U_Anexo", U_Anexo);

            var lines = generalData.Child("ZPN_RDR1");

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                lines.Remove(i);
            }

            foreach (var line in Lines)
            {
                var lineData = lines.Add();
                lineData.SetProperty("U_Candidato", line.U_Candidato);
                lineData.SetProperty("U_Item", line.U_Item);
                lineData.SetProperty("U_ItemFat", line.U_ItemFat);
                lineData.SetProperty("U_DescItemFat", line.U_DescItemFat);
                lineData.SetProperty("U_Parcela", line.U_Parcela);
                lineData.SetProperty("U_Tipo", line.U_Tipo);
                lineData.SetProperty("U_DataLanc", line.U_DataLanc);
                lineData.SetProperty("U_DataFat", line.U_DataFat);
                lineData.SetProperty("U_NroNF", line.U_NroNF);
                lineData.SetProperty("U_DataSol", line.U_DataSol);
                lineData.SetProperty("U_PrjCode", line.U_PrjCode);
                lineData.SetProperty("U_CardCode", line.U_CardCode);
                lineData.SetProperty("U_CardName", line.U_CardName);
                lineData.SetProperty("U_ItemCode", line.U_ItemCode);
                lineData.SetProperty("U_Valor", line.U_Valor);
                lineData.SetProperty("U_Obs", line.U_Obs);
            }

            generalService.Update(generalData);
        }

        public void Delete()
        {
            var generalService = ((CompanyService)Globals.Master.Connection.Database.GetCompanyService()).GetGeneralService("ZPN_ORDR");
            var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
            generalDataParams.SetProperty("DocEntry", DocEntry);
            generalService.Delete(generalDataParams);
        }

        public bool GetByDocEntry(int docEntry)
        {

            try
            {
                var generalService = ((CompanyService)Globals.Master.Connection.Database.GetCompanyService()).GetGeneralService("ZPN_ORDR");
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                GeneralData generalData;
                try
                {
                    generalDataParams.SetProperty("DocEntry", docEntry);
                    generalData = generalService.GetByParams(generalDataParams);
                }
                catch
                {
                    return false;
                }


                DocEntry = docEntry;
                DocNum = int.Parse(generalData.GetProperty("DocNum").ToString());
                BplID = int.Parse(generalData.GetProperty("U_BplID").ToString());
                U_NroPedido = generalData.GetProperty("U_NroPedido").ToString();
                U_Valor = double.Parse(generalData.GetProperty("U_Valor").ToString());
                U_Data = DateTime.Parse(generalData.GetProperty("U_Data").ToString());
                U_DataVenc = DateTime.Parse(generalData.GetProperty("U_DataVenc").ToString());



                U_NroCont = generalData.GetProperty("U_NroCont").ToString();
                U_Status = generalData.GetProperty("U_Status").ToString();
                U_Desc = generalData.GetProperty("U_Desc").ToString();
                U_Anexo = generalData.GetProperty("U_Anexo").ToString();
                Lines = new List<PurchaseOrderLine>();

                var linesPO = generalData.Child("ZPN_RDR1");


                for (int i = 0; i < linesPO.Count; i++)
                {
                    var lineData = linesPO.Item(i);
                    var line = new PurchaseOrderLine
                    {
                        U_Candidato = lineData.GetProperty("U_Candidato").ToString(),
                        U_Item = lineData.GetProperty("U_Item").ToString(),
                        U_ItemFat = lineData.GetProperty("U_ItemFat").ToString(),
                        U_DescItemFat = lineData.GetProperty("U_DescItemFat").ToString(),
                        U_Parcela = lineData.GetProperty("U_Parcela").ToString(),
                        U_Tipo = lineData.GetProperty("U_Tipo").ToString(),
                        U_DataLanc = DateTime.Parse(lineData.GetProperty("U_DataLanc").ToString()),
                        U_DataFat = DateTime.Parse(lineData.GetProperty("U_DataFat").ToString()),
                        U_NroNF = lineData.GetProperty("U_NroNF").ToString(),
                        U_DataSol = DateTime.Parse(lineData.GetProperty("U_DataSol").ToString()),
                        U_PrjCode = lineData.GetProperty("U_PrjCode").ToString(),
                        U_PrjName = lineData.GetProperty("U_PrjName").ToString(),
                        U_CardCode = lineData.GetProperty("U_CardCode").ToString(),
                        U_CardName = lineData.GetProperty("U_CardName").ToString(),
                        U_ItemCode = lineData.GetProperty("U_ItemCode").ToString(),
                        U_Valor = double.Parse(lineData.GetProperty("U_Valor").ToString()),
                        U_Obs = lineData.GetProperty("U_Obs").ToString()
                    };
                    Lines.Add(line);
                }
            }
            catch (Exception Ex)
            {
                string MensagemErro = $"Erro ao carregar dados: {Ex.Message}";

                Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, MensagemErro, Ex);
                throw new Exception(MensagemErro);

            }

            return true;
        }
    }

}
