using sap.dev.core;
using sap.dev.core.ApiService;
using sap.dev.core.Controller;
using sap.dev.core.DTO;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.UtilAddOn
{
    public static class UtilWarehouses
    {
        public static async Task CriaDepositosRAAsync()
        {
            try
            {

                Wharehouse wharehouse = new Wharehouse();

                DTOWarehousesSL warehousesSL = new DTOWarehousesSL();

                string SQL_Query = $"ZPN_SP_CRIADEPOSITORA";

                DataTable DtResultados = SqlUtils.ExecuteCommand(SQL_Query);

                foreach (DataRow dr in DtResultados.Rows)
                {
                    warehousesSL = new DTOWarehousesSL();

                    warehousesSL.WarehouseCode = dr["WhsCode"].ToString();
                    warehousesSL.WarehouseName = dr["WhsName"].ToString();
                    warehousesSL.BusinessPlaceID = Convert.ToInt32(dr["BPLId"]);
                    warehousesSL.U_DepositoRA = "Y";

                    await wharehouse.SendWarehouseDataAsync(warehousesSL, Globals.Master.Connection.Database.CompanyDB);

                    Util.ExibirMensagemStatusBar($"Criando depósito de RA: {dr["WhsName"].ToString()}");
                }

                Util.ExibirMensagemStatusBar($"Fim da criação de depósitos!");
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao criar depósitos de RA: {Ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Long, true, Ex);
            }
        }
    }
}
