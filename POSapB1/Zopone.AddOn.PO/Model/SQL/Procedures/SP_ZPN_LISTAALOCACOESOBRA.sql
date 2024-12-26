CREATE PROCEDURE SP_ZPN_LISTAALOCACOESOBRA
(
	@CodigoObra varchar(100)
)
as
BEGIN
	
	SELECT distinct
		ALCI.U_CodAloc "Codigo"
	FROM 
		"@ZPN_OPRJ" OBRA 
		INNER JOIN "@ZPN_ALOCONI" ALCI ON ALCI.Code		= OBRA.U_CodContrato AND ALCI.U_PC = 'Y'
		INNER JOIN "@ZPN_ALOCA" ON "@ZPN_ALOCA"."Code"	= ALCI.U_CodAloc
	where
		"@ZPN_ALOCA".U_ItensFat = 'Y' and 
		OBRA."Code" = @CodigoObra;

END;
