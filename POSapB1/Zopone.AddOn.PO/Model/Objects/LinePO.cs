﻿using System;

namespace Zopone.AddOn.PO.Model.Objects
{

    public class LinePO
    {
        public Int32 LineNum { get; set; }
        public Int32? VisOrder { get; set; }
        public string Agrupar { get; set; }
        public string U_PrjCode { get; set; }
        public string U_PrjName { get; set; }
        public string U_Candidato { get; set; }
        public string U_CardCode { get; set; }
        public string U_CardName { get; set; }
        public string U_Item { get; set; }
        public string U_ItemFat { get; set; }
        public string PCG { get; set; }
        public string Obra { get; set; }
        public string Regional { get; set; }

        public string U_DescItemFat { get; set; }
        public string U_ItemCode { get; set; }
        public string U_Parcela { get; set; }
        public Double U_Valor { get; set; }
        public string U_Tipo { get; set; }
        public Int32? AgrNo { get; set; }
        public string DescContrato { get; set; }
        public DateTime U_DataLanc { get; set; }
        public DateTime? U_DataFat { get; set; }
        public string U_NroNF { get; set; }
        public DateTime? U_DataSol { get; set; }
        public string U_Obs { get; set; }
        public Boolean U_Bloqueado { get; set; }
        public string U_Validado { get; set; }

        public string U_itemDescription { get; set; }
        public string U_manSiteInfo { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public bool Edited { get; set; } = false;

    }
}
