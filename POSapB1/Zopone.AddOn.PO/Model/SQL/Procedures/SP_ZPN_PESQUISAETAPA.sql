
CREATE PROCEDURE SP_ZPN_PESQUISAETAPA
AS 
BEGIN

	SELECT
		ALC."Code" "Código", 
		ALC."U_Desc" "Descrição"
	FROM 
		"@ZPN_ALOCA" ALC
	order by 
		ALC."U_Desc" ;

END;