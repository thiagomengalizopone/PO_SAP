create PROCEDURE SP_ZPN_PESQUISAETAPA
(
	@CampoPesquisa varchar(100)
)
AS 
BEGIN

	SELECT
		ALC."Code" "Código", 
		ALC."U_Desc" "Descrição"
	FROM 
		"@ZPN_ALOCA" ALC
	where
		ALC."Code" LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  OR
		ALC."U_Desc"  LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%' 
	order by 
		ALC."U_Desc" ;

END;