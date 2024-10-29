using sap.dev.core;
using sap.dev.data;
using sap.dev.ui.Forms;
using SAPbouiCOM;
using System;
using System.Threading.Tasks;
using Zopone.AddOn.PO.View.Obra;

namespace Zopone.AddOn.PO.View.Deposito
{
    public class Frm62 : FormSDK
    {

        public class ItensUIDs
        {

        }


        public Frm62() : base()
        {
            if (oForm == null)
                return;        
        }

        public static bool PosicionaTela(string FormUID)
        {
            try
            {
                // Obtendo o formulário
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(FormUID);

                // Referências dos itens existentes
                Item oItemRefStatic = oForm.Items.Item("2005");
                Item oItemRefEdit = oForm.Items.Item("2003");
                Item oItem;

                // Adicionando o item estático
                oItem = oForm.Items.Add("StObra", BoFormItemTypes.it_STATIC);
                StaticText StCodigoObra = (StaticText)oItem.Specific;

                // Configurando propriedades do item estático
                StCodigoObra.Item.FromPane = oItemRefStatic.FromPane;
                StCodigoObra.Item.ToPane = oItemRefStatic.ToPane;
                StCodigoObra.Item.Height = oItemRefStatic.Height;
                StCodigoObra.Item.Width = oItemRefStatic.Width;
                StCodigoObra.Item.Left = oItemRefStatic.Left;
                StCodigoObra.Item.Top = oItemRefStatic.Top + 17; 
                StCodigoObra.Caption = "Cód. Obra";

                // Adicionando o campo de edição
                oItem = oForm.Items.Add("EdObra", BoFormItemTypes.it_EDIT);
                EditText EdCodigoObra = (EditText)oItem.Specific;

                // Configurando propriedades do campo de edição
                EdCodigoObra.Item.FromPane = oItemRefEdit.FromPane;
                EdCodigoObra.Item.ToPane = oItemRefEdit.ToPane;
                EdCodigoObra.Item.Height = oItemRefEdit.Height;
                EdCodigoObra.Item.Width = oItemRefEdit.Width;
                EdCodigoObra.Item.Left = oItemRefEdit.Left;
                EdCodigoObra.Item.Top = oItemRefEdit.Top + 17; 

                EdCodigoObra.DataBind.SetBound(true, "OWHS", "U_CodObra");


                Item oLinkedButtonItem = oForm.Items.Add("btnLinked", BoFormItemTypes.it_LINKED_BUTTON);
                LinkedButton btnLinked = (LinkedButton)oLinkedButtonItem.Specific;

                // Configurando propriedades do LinkedButton
                btnLinked.Item.FromPane = oItemRefEdit.FromPane;
                btnLinked.Item.ToPane = oItemRefEdit.ToPane;
                btnLinked.Item.Height = 15; // Altura do botão
                btnLinked.Item.Width = 15; // Largura do botão
                btnLinked.Item.Left = EdCodigoObra.Item.Left + EdCodigoObra.Item.Width + 5; // Posicionar ao lado do EditText
                btnLinked.Item.Top = EdCodigoObra.Item.Top; // Alinhamento vertical


                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                SAPbouiCOM.Conditions oCons = null;
                SAPbouiCOM.Condition oCon = null;

                oCFLs = oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Globals.Master.Connection.Interface.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "63";
                oCFLCreationParams.UniqueID = "CFL_63";

                oCFL = oCFLs.Add(oCFLCreationParams);

                EdCodigoObra.ChooseFromListUID = "CFL_63";
                EdCodigoObra.ChooseFromListAlias = "PrjCode";

                EdCodigoObra.ChooseFromListAfter += EdCodigoObra_ChooseFromListAfter;
               


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao posicionar campo de obra em tela: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
                return false;
            }
            return true;

        }

        private static void EdCodigoObra_ChooseFromListAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                Form oForm = Globals.Master.Connection.Interface.Forms.Item(pVal.FormUID);

                EditText EdCodigoObra = (EditText)oForm.Items.Item("EdObra").Specific;

                SBOChooseFromListEventArg aEvent = (SBOChooseFromListEventArg)pVal;
                if (aEvent.SelectedObjects == null)
                    return;

                string PrjCode = Convert.ToString(aEvent.SelectedObjects.GetValue("PrjCode", 0));

                EdCodigoObra.Value = PrjCode;


            }
            catch (Exception Ex)
            {
                Util.ExibeMensagensDialogoStatusBar($"Erro ao selecionar Obra: {Ex.Message}", BoMessageTime.bmt_Medium, true, Ex);
            }
        }


        public static bool Interface_FormItemEvent(ref ItemEvent pVal)
        {
            try
            {
                if (!pVal.BeforeAction)
                {
                    if (pVal.EventType == BoEventTypes.et_FORM_DRAW)
                    {
                        return PosicionaTela(pVal.FormUID);
                    }
                }

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception($"Erro nos eventos: {Ex.Message}");
            }
        }
    }
}