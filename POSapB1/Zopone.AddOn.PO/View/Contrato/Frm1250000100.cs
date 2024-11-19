using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;
using Zopone.AddOn.PO.View.Alocação;
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
                oItem.Top = iTop ;
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
                oItem.Left = oItemRef.Left  + oItemRef.Width + 10;
                oItem.Top = oItemRef.Top ;
                oItem.Width = oItemRef.Width;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = 0;
                oItem.ToPane = 0;

                BtAlocacao = ((SAPbouiCOM.Button)(oItem.Specific));
                BtAlocacao.Caption = "Alocação";
                BtAlocacao.PressedAfter += BtAlocacao_PressedAfter;



                oFormContrato.Update();

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao abrir tela: {Ex.Message}");
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
        private static async Task EnviarDadosPCIAsync(string formUID)
        {
            try
            {
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI!");
                Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);
                EditText oEditContrato = (EditText)oFormContrato.Items.Item("1250000004").Specific;

                string SQL_Query = $"ZPN_SP_PCI_ATUALIZACONTRATO '{oEditContrato.Value}'";

                SqlUtils.DoNonQuery(SQL_Query);
                Util.ExibirMensagemStatusBar($"Atualizando dados PCI - Concluído!");

            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao carregar dados da tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
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
                        string FormUID = businessObjectInfo.FormUID;

                        new Task(() => { EnviarDadosPCIAsync(FormUID); }).Start();                        

                     
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

        
    }
}