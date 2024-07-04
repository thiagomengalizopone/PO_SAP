using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zopone.AddOn.PO.Controller.ParceiroNegocio;

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

                Item oItemRef = oFormContrato.Items.Item("1250000039");

                Item oItem;
                EditText oEditText;
                StaticText oStaticText;

                oItem = oFormContrato.Items.Add("EdNroRH", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = oItemRef.Left;
                oItem.Top = oItemRef.Top + 19; 
                oItem.Width = 150;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;

                oEditText = ((SAPbouiCOM.EditText)(oItem.Specific));
                oEditText.DataBind.SetBound(true, "OOAT", "U_CodigoRH");

                oItemRef = oFormContrato.Items.Item("1250000038");
                oItem = oFormContrato.Items.Add("StNroRH", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = oItemRef.Left;
                oItem.Top = oItemRef.Top+19; 
                oItem.Width = 147;
                oItem.Height = oItemRef.Height;
                oItem.FromPane = oItemRef.FromPane;
                oItem.ToPane = oItemRef.ToPane;


                oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                oStaticText.Caption = "Número RH";

                Item oFolderRef = oFormContrato.Items.Item("1320000072");

                Item oNewFolderItem = oFormContrato.Items.Add("FldObra", BoFormItemTypes.it_FOLDER);
                oNewFolderItem.Left = oFolderRef.Left; // Definir posição da nova pasta
                oNewFolderItem.Top = oFolderRef.Top;
                oNewFolderItem.Width = oFolderRef.Width;
                oNewFolderItem.Height = oFolderRef.Height;

                Folder oNewFolder = (Folder)oNewFolderItem.Specific;
                oNewFolder.Caption = "Obras";
                oNewFolder.Pane = 99;
                oNewFolder.AutoPaneSelection= true;
                oNewFolder.GroupWith(oFolderRef.UniqueID);

                oNewFolder.ClickAfter += ONewFolder_ClickAfter;

                oFormContrato.Update();

            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro ao abrir tela: {Ex.Message}");
            }
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
    }
}