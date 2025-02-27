using sap.dev.core;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.View.LancContManual
{
    public class Frm392 : FormSDK
    {
        public Frm392() : base()
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
                    else if (pVal.EventType == BoEventTypes.et_FORM_RESIZE)
                    {
                        FormAutoResize(pVal.FormUID);
                    }
                    if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (pVal.ItemUID == "BtExcel")
                        {
                            //abrir a seleção de arquivo excel
                        }
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
                //Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);


                //Item oItemRef = oFormContrato.Items.Item("1250000036");

                //int Width = oItemRef.Width;


                //oItemRef = oFormContrato.Items.Item("1250000039");

                //Item oItem;
                //EditText oEditText;
                //StaticText oStaticText;

                //Button BtAlocacao;

                //int iTop = 205;

                //oItem = oFormContrato.Items.Add("EdNroRH", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //oItem.Left = 390;
                //oItem.Top = iTop;
                //oItem.Width = 109;
                //oItem.Height = oItemRef.Height;
                //oItem.FromPane = oItemRef.FromPane;
                //oItem.ToPane = oItemRef.ToPane;
                //oItem.LinkTo = "StNroRH";

                //oEditText = ((SAPbouiCOM.EditText)(oItem.Specific));
                //oEditText.DataBind.SetBound(true, "OOAT", "U_CodigoRH");

                //oItemRef = oFormContrato.Items.Item("1250000038");
                //oItem = oFormContrato.Items.Add("StNroRH", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                //oItem.Left = 280;
                //oItem.Top = iTop;
                //oItem.Width = 90;

                //oItem.Height = oItemRef.Height;
                //oItem.FromPane = oItemRef.FromPane;
                //oItem.ToPane = oItemRef.ToPane;

                //oItem.LinkTo = "EdNroRH";

                //oItemRef = oFormContrato.Items.Item("EdNroRH");

                //oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                //oStaticText.Caption = "Número RH";


                //oItem = oFormContrato.Items.Add("CbReg", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);

                //oItem.Left = 390;
                //oItem.Top = iTop + 19;
                //oItem.Width = 109;
                //oItem.Height = oItemRef.Height;
                //oItem.FromPane = oItemRef.FromPane;
                //oItem.ToPane = oItemRef.ToPane;
                //oItem.LinkTo = "StNroRH";


                //ComboBox CbRegional = ((SAPbouiCOM.ComboBox)(oItem.Specific));
                //CbRegional.Item.DisplayDesc = true;
                //CbRegional.ExpandType = BoExpandType.et_DescriptionOnly;

                //CbRegional.DataBind.SetBound(true, "OOAT", "U_Regional");

                //oItemRef = oFormContrato.Items.Item("StNroRH");
                //oItem = oFormContrato.Items.Add("StReg", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                //oItem.Left = 280;
                //oItem.Top = iTop + 19;
                //oItem.Width = 90;
                //oItem.Height = oItemRef.Height;
                //oItem.FromPane = oItemRef.FromPane;
                //oItem.ToPane = oItemRef.ToPane;
                //oItem.LinkTo = "CbReg";


                //oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                //oStaticText.Caption = "Regional";

                //Util.ComboBoxSetValoresValidosPorSQL(CbRegional, UtilScriptsSQL.SQL_Regionais);

                //Item oFolderRef = oFormContrato.Items.Item("1320000072");

                //Item oNewFolderItem = oFormContrato.Items.Add("FldObra", BoFormItemTypes.it_FOLDER);
                //oNewFolderItem.Left = oFolderRef.Left;
                //oNewFolderItem.Top = oFolderRef.Top;
                //oNewFolderItem.Width = oFolderRef.Width;
                //oNewFolderItem.Height = oFolderRef.Height;

                //Folder oNewFolder = (Folder)oNewFolderItem.Specific;
                //oNewFolder.Caption = "Obras";
                //oNewFolder.Pane = 99;
                //oNewFolder.AutoPaneSelection = true;
                //oNewFolder.GroupWith(oFolderRef.UniqueID);

                //Item iGridObras = oFormContrato.Items.Add("GdObras", BoFormItemTypes.it_GRID);
                //iGridObras.Left = 20;
                //iGridObras.Top = oFolderRef.Top + 30;
                //iGridObras.Width = 600;
                //iGridObras.Height = 350;

                //iGridObras.FromPane = 99;
                //iGridObras.ToPane = 99;

                //Grid oGridObras = (Grid)iGridObras.Specific;

                //oGridObras.SelectionMode = BoMatrixSelect.ms_Single;

                //DataTable DtObras = oFormContrato.DataSources.DataTables.Add("DtObras");

                //oGridObras.DataTable = DtObras;

                //oGridObras.DoubleClickAfter += OGridObras_DoubleClickAfter;

                //CarregarObrasContrato(oFormContrato, "XXXX");

                //oNewFolder.ClickAfter += ONewFolder_ClickAfter;

                //oItemRef = oFormContrato.Items.Item("1250000002");

                //oItem = oFormContrato.Items.Add("BtAlocacao", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                //oItem.Left = oItemRef.Left + oItemRef.Width + 10;
                //oItem.Top = oItemRef.Top;
                //oItem.Width = oItemRef.Width;
                //oItem.Height = oItemRef.Height;
                //oItem.FromPane = 0;
                //oItem.ToPane = 0;

                //BtAlocacao = ((SAPbouiCOM.Button)(oItem.Specific));
                //BtAlocacao.Caption = "Alocação";

                //oFormContrato.Update();

                //FormAutoResize(formUID);

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao abrir tela: {Ex.Message}");
            }
        }

        private static void FormAutoResize(string formUID)
        {
            try
            {
                //Form oFormContrato = Globals.Master.Connection.Interface.Forms.Item(formUID);
                //try
                //{
                //    oFormContrato.Freeze(true);

                //    Item oItemGridObra = oFormContrato.Items.Item("GdObras");
                //    Item oItemGridRef = oFormContrato.Items.Item("1250000044");

                //    oItemGridObra.Top = oFormContrato.Items.Item("1250000031").Top;
                //    oItemGridObra.Left = oItemGridRef.Left;
                //    oItemGridObra.Width = oItemGridRef.Width;
                //    oItemGridObra.Height = oItemGridRef.Height;

                //    Item oItemAloca = oFormContrato.Items.Item("BtAlocacao");

                //    oItemAloca.Top = oFormContrato.Items.Item("1250000002").Top;
                //    oItemAloca.Left = oFormContrato.Items.Item("1250000002").Left + oFormContrato.Items.Item("1250000002").Width + 5;

                //    Item oItem = oFormContrato.Items.Item("1250000043");
                //    oItem.Top = oItem.Top + 40;

                //    oItem = oFormContrato.Items.Item("1250000044");
                //    oItem.Top = oItem.Top + 40;
                //    oItem.Height = oItem.Height - 40;

                //    oItem = oFormContrato.Items.Item("1320000055");
                //    int iLeftLabel = oItem.Left;
                //    int iTopLabel = oItem.Top;

                //    oItem = oFormContrato.Items.Item("1320000056");
                //    int iLeftText = oItem.Left;
                //    int iTopText = oItem.Top;

                //    iTopLabel += 17;
                //    iTopText += 17;

                //    oItem = oFormContrato.Items.Item("StNroRH");
                //    oItem.Top = iTopLabel;
                //    oItem.Left = iLeftLabel;

                //    oItem = oFormContrato.Items.Item("EdNroRH");
                //    oItem.Top = iTopText;
                //    oItem.Left = iLeftText;

                //    iTopLabel += 17;
                //    iTopText += 17;

                //    oItem = oFormContrato.Items.Item("StReg");
                //    oItem.Top = iTopLabel;
                //    oItem.Left = iLeftLabel;

                //    oItem = oFormContrato.Items.Item("CbReg");
                //    oItem.Top = iTopText;
                //    oItem.Left = iLeftText;

                //}
                //catch (Exception Ex)
                //{
                //    Util.GravarLog(EnumList.EnumAddOn.CadastroPO, EnumList.TipoMensagem.Erro, Ex.Message, Ex);
                //}
                //finally
                //{
                //    oFormContrato.Freeze(false);
                //}
            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao posicionar form: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }

    }
}
