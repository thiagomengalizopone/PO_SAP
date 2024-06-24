using sap.dev.core;
using sap.dev.core.DTO;
using sap.dev.core.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static sap.dev.core.EnumList;

namespace Zopone.AddOn.PO.Model
{
    public class ScriptSQL
    {
        static Encoding encoding;

        public static DTOScript[] RetornaSQLScripts()
        {
            int Count = 0;

            Int32 versaoAddOn = Globals.Master.CurrentVersion;

            encoding = Encoding.GetEncoding("utf-8", EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

            Type[] SqlProceduresSql = RetornaScriptAlterado.Retorna(typeof(SqlProcedures).GetNestedTypes(), versaoAddOn);
            Type[] SqlFunctionsSql = RetornaScriptAlterado.Retorna(typeof(SqlFunctions).GetNestedTypes(), versaoAddOn);
            Type[] SqlTablesSql = RetornaScriptAlterado.Retorna(typeof(SqlTables).GetNestedTypes(), versaoAddOn);
            Type[] SqlViewsSql = RetornaScriptAlterado.Retorna(typeof(SqlViews).GetNestedTypes(), versaoAddOn);

            DTOScript[] SqlScriptsAddOn = new DTOScript[SqlProceduresSql.Length + SqlFunctionsSql.Length + SqlTablesSql.Length + SqlViewsSql.Length];

            #region SQL - Table                        
            for (int iPos = 0; iPos < SqlTablesSql.Length; iPos++)
            {
                SqlScriptsAddOn[Count] = new DTOScript();
                SqlScriptsAddOn[Count].Tipo = TipoScript.Table;
                SqlScriptsAddOn[Count].FileName = SqlTablesSql[iPos].GetField("Nome")?.GetValue(null).ToString();
                SqlScriptsAddOn[Count].Bytes = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(SqlScriptsAddOn[Count].FileName), encoding);
                Count++;
            }
            #endregion

            #region SQL - Function                        
            for (int iPos = 0; iPos < SqlFunctionsSql.Length; iPos++)
            {
                SqlScriptsAddOn[Count] = new DTOScript();
                SqlScriptsAddOn[Count].Tipo = TipoScript.Function;
                SqlScriptsAddOn[Count].FileName = SqlFunctionsSql[iPos].GetField("Nome")?.GetValue(null).ToString();
                SqlScriptsAddOn[Count].Bytes = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(SqlScriptsAddOn[Count].FileName), encoding);
                Count++;
            }
            #endregion

            #region SQL  - View                        
            for (int iPos = 0; iPos < SqlViewsSql.Length; iPos++)
            {
                SqlScriptsAddOn[Count] = new DTOScript
                {
                    Tipo = TipoScript.View,
                    FileName = SqlViewsSql[iPos].GetField("Nome")?.GetValue(null).ToString(),
                    Bytes = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(SqlScriptsAddOn[Count].FileName), encoding)
                };
                Count++;
            }
            #endregion

            #region SQL - Procedure                        
            for (int iPos = 0; iPos < SqlProceduresSql.Length; iPos++)
            {
                SqlScriptsAddOn[Count] = new DTOScript();
                SqlScriptsAddOn[Count].Tipo = TipoScript.Proc;
                SqlScriptsAddOn[Count].FileName = SqlProceduresSql[iPos].GetField("Nome")?.GetValue(null).ToString();
                SqlScriptsAddOn[Count].Bytes = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(SqlScriptsAddOn[Count].FileName), encoding);
                Count++;
            }
            #endregion

            return SqlScriptsAddOn;
        }
    }
}
