CREATE PROCEDURE [ZPN_SP_PCI_ATUALIZAOBRAPCG]
(
    @CodeObra VARCHAR(30),
	@CreateDate date
)
AS
BEGIN

	UPDATE "@ZPN_OPRJ" 
		SET U_PCG = OPRC.PrcCode
	FROM
		"@ZPN_OPRJ" obra
		INNER JOIN OPRC ON OPRC.U_Obra = OBRA."Code" and OPRC."DimCode" = 2
	WHERE
		isnull(obra.U_PCG,'')  <> OPRC.PrcCode and
        ((OBRA.Code = @CodeObra) or
		(
			ISNULL(@CodeObra, '') = '' AND 
			OBRA.CreateDate = @CreateDate
		));
END;