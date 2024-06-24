using System;
using static sap.dev.core.EnumList;

namespace Zopone.AddOn.PO.Model
{
    public static class SqlProcedures
    {
        public static class SP_ZPN_CriaCodigoConta
        {
            public const string Nome = "Zopone.AddOn.PO.Model.SQL.Procedures.SP_ZPN_CriaCodigoConta.sql";
            public const string Descricao = "Cria código de próxima conta";
            public const TipoScript Tipo = TipoScript.Proc;
            public const Int32 Versao = 2023090801;
        }

        public static class SP_GRAVARLOG
        {
            public const string Nome = "Zopone.AddOn.PO.Model.SQL.Procedures.SP_GRAVARLOG.sql";
            public const string Descricao = "Gravar Log do sistema";
            public const TipoScript Tipo = TipoScript.Proc;
            public const Int32 Versao = 2023090801;
        }
    }
}
