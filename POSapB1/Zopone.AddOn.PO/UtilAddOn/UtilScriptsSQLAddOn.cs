using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.UtilAddOn
{
    public class UtilScriptsSQLAddOn
    {
        public static string SQL_Localizacao(string CodeLocalizacao)
        {
            return $@"
					SELECT 
						Code, 
						Location, 
						U_RegDesc 
					FROM
						OLCT
					WHERE
						('{CodeLocalizacao}' = '' or Code = '{CodeLocalizacao}') 
						AND 
							Code NOT IN 
							(
								SELECT 
									OPRC.U_Localiz
								FROM
									OPRC
								WHERE 
									OPRC.U_Localiz = OLCT.Code
							)
					ORDER BY
						Code ;
					";

        }


    }
}
