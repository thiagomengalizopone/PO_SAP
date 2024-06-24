using System;
using static sap.dev.core.EnumList;

namespace Zopone.AddOn.PO.Model
{
    public static class SqlTables
    {
        public static class TableLog
        {
            public const string Nome = "Zopone.AddOn.PO.Model.SQL.Tables.TB_LOGMSG.sql";
            public const string Descricao = "Tabela de log de Mensagens do AddOn";
            public const TipoScript Tipo = TipoScript.Table;
            public const Int32 Versao = 2023090801;
        }
    }
}
