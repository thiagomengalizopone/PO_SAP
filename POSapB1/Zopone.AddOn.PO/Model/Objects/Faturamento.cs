using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.Model.Objects
{
    public class Faturamento
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double Total { get; set; }

        public Faturamento(string codigo, string descricao, double total)
        {
            Codigo = codigo;
            Descricao = descricao;
            Total = total;
        }

        public double CalcularPercentual(double totalFaturamento)
        {
            if (totalFaturamento > 0)
                return Math.Round((Total / totalFaturamento) * 100, 2);
            return 0;
        }
    }

    public class AlocacaoFaturamento
    {
        public List<Faturamento> Faturamentos { get; set; } = new List<Faturamento>();

        public void Add(string codigo, string descricao, double total)
        {
            Faturamentos.Add(new Faturamento(codigo, descricao, total));
        }

        public double TotalFaturamento => Faturamentos.Sum(f => f.Total);

    }

}
