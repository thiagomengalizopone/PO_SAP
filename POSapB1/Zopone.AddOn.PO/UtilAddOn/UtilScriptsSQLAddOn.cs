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
						OLCT.Code, 
						OLCT.Location, 
						OLCT.U_RegDesc 
					FROM
						OLCT
					WHERE
						('{CodeLocalizacao}' = '' or Code = '{CodeLocalizacao}') 
						AND
							OLCT.Code NOT IN 
							(
								SELECT 
									OPRC.U_Localiz
								FROM
									OPRC
								WHERE 
									OPRC.U_Localiz = OLCT.Code
							)
					ORDER BY
						OLCT.Code ;
					";



        }


    }
}
