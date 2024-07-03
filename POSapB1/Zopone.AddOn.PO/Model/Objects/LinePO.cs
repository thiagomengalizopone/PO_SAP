using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.Model.Objects
{
    
    public class LinePO
    {
        public Int32 LineNum { get; set; }
        public string Project   { get; set; }
        public string U_Candidato { get; set; }
        public string U_CardCode { get; set; }
        public string U_Item { get; set; }
        public string U_ItemFat { get; set; }
        public string U_DescItemFat { get; set; }
        public string ItemCode { get; set; }
        public string U_Parcela { get; set; }
        public Double LineTotal { get; set; }
        public string U_Tipo { get; set; }
        public DateTime U_DataLanc { get; set; }
        public DateTime U_DataFat { get; set; }
        public string U_NroNF { get; set; }
        public DateTime U_DataSol { get; set; }
        public string FreeTxt { get; set; }

    }
}
