CREATE PROCEDURE SP_ZPN_PESQUISAETAPA
(
	@CampoPesquisa varchar(100),
	@Obra varchar(100)
)
AS 
BEGIN


	SELECT distinct
		ALCI.U_CodAloc "Código", 
		ALCI.U_Descaloc "Descrição"
	FROM 
		"@ZPN_OPRJ" OBRA 
		INNER JOIN "@ZPN_ALOCONI" ALCI ON ALCI.Code		= OBRA.U_CodContrato AND ALCI.U_PC = 'Y'
		INNER JOIN "@ZPN_ALOCA" ON "@ZPN_ALOCA"."Code"	= ALCI.U_CodAloc
	where
		"@ZPN_ALOCA".U_ItensPed = 'Y' and 
		OBRA."Code" = @Obra AND
		(ALCI.U_CodAloc LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%'  OR
		ALCI.U_Descaloc  LIKE '%' ++ ISNULL(@CampoPesquisa,'') ++  '%' )
	order by 
		ALCI.U_Descaloc ;
		
END;