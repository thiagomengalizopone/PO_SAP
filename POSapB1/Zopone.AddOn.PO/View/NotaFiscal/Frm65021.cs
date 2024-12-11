using sap.dev.core;
using sap.dev.core.ApiService_n8n;
using sap.dev.core.DTO;
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
    public class Frm65021 : FormSDK
    {

        public class ItensUIDs
        {

        }

        public Frm65021() : base()
        {
            if (oForm == null)
                return;
        }

        internal static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            if (pVal.EventType == BoEventTypes.et_FORM_LOAD && !pVal.BeforeAction)
            {
                CarregarForm(pVal.FormUID);
            }
            else if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST && !pVal.BeforeAction)
            {
                CarregarChooseFromList(pVal);
            }

            return true;
        }

        private static void CarregarChooseFromList(ItemEvent pVal)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(pVal.FormUID);

                SAPbouiCOM.IChooseFromListEvent oCFLEvento = null;
                oCFLEvento = ((SAPbouiCOM.IChooseFromListEvent)(pVal));
                string sCFL_ID = null;
                sCFL_ID = oCFLEvento.ChooseFromListUID;
                SAPbouiCOM.ChooseFromList oCFL = null;
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID);
                if (oCFLEvento.BeforeAction == false)
                {
                    SAPbouiCOM.DataTable oDataTable = null;
                    oDataTable = oCFLEvento.SelectedObjects;

                    Matrix oMtParcelas = (Matrix)oForm.Items.Item("3").Specific;

                    string Code = Convert.ToString(oDataTable.GetValue("Code", 0));
                    string Descricao = Convert.ToString(oDataTable.GetValue("U_Desc", 0));

                    EditText oEditText = (SAPbouiCOM.EditText)oMtParcelas.Columns.Item("U_DescItemFat").Cells.Item(1).Specific;
                    oEditText.Value = Descricao;

                    oEditText = (SAPbouiCOM.EditText)oMtParcelas.Columns.Item("U_ItemFat").Cells.Item(1).Specific;
                    oEditText.Value = Code;
                }
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Obra: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

        private static void CarregarForm(string formUID)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(formUID);

                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Globals.Master.Connection.Interface.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));


                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "ZPN_ALOCA";
                oCFLCreationParams.UniqueID = "CFL_ALOC";

                oCFL = oCFLs.Add(oCFLCreationParams);

                Matrix oMtParcelas = (Matrix)oForm.Items.Item("3").Specific;

                oMtParcelas.Columns.Item("U_ItemFat").ChooseFromListUID = "CFL_ALOC";
                oMtParcelas.Columns.Item("U_ItemFat").ChooseFromListAlias = "Code";

                var oConds = new SAPbouiCOM.Conditions();
                var oCfLs = oForm.ChooseFromLists;

                var cfl = oCfLs.Item("CFL_ALOC");

                if (cfl.GetConditions().Count > 0)
                {
                    SAPbouiCOM.Conditions emptyCon = new SAPbouiCOM.Conditions();

                    cfl.SetConditions(emptyCon);
                }

                SAPbouiCOM.EditText oEditText = (SAPbouiCOM.EditText)oMtParcelas.Columns.Item("U_Project").Cells.Item(1).Specific;
                string CodeObra = oEditText.Value;




                var oRecordSet = (Recordset)SAPDbConnection.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                string sql_query = $"SP_ZPN_LISTAALOCACOESOBRA '{CodeObra}'";
                oRecordSet.DoQuery(sql_query);

                int iRow = 1;

                while (!oRecordSet.EoF)
                {
                    var oCond = oConds.Add();

                    if (oConds.Count == 1)
                        oCond.BracketOpenNum = 1;


                    oCond.Alias = "Code";
                    oCond.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCond.CondVal = oRecordSet.Fields.Item("Codigo").Value.ToString();

                    if (oRecordSet.RecordCount > 1 && iRow != oRecordSet.RecordCount)
                        oCond.Relationship = BoConditionRelationship.cr_OR;

                    if (iRow == oRecordSet.RecordCount)
                        oCond.BracketCloseNum = 1;

                    oRecordSet.MoveNext();

                    iRow++;
                }




                cfl.SetConditions(oConds);


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar formulário: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }
    }

}