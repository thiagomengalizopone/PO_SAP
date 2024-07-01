create PROCEDURE SP_ZPN_PESQUISAETAPA
(
	@CampoPesquisa varchar(100)
)
AS 
BEGIN

	SELECT
		ALC."Code" "Código", 
		ALC."U_Desc" "Descrição",
		ALC."U_ItemCode" "Cód. Item",
		ALC."U_ItemName" "Descrição Item"
	FROM 
		"@ZPN_ALOCA" ALC
	where
		ALC."Code" LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  OR
		ALC."U_Desc"  LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  OR
		ALC."U_ItemCode"  LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  OR
		ALC."U_ItemName"  LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  
	order by 
		ALC."U_Desc" ;

END;