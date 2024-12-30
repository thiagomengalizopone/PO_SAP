
INSERT INTO [LINKZCLOUD].[zsistema_producao].[dbo].[obracandidato]
	(
		[obracandidatoid]
        ,[gestatus]
        ,[gedataacao]
        ,[gecontaidacao]
        ,[codigo]
        ,[obraid]
        ,[selecionado]
        ,[identificacao]
        ,[tipo]
        ,[nome]
        ,[detentora]
        ,[iddetentora]
        ,[cep]
        ,[endereco]
        ,[numero]
        ,[complemento]
        ,[bairro]
        ,[cidade]
        ,[estado]
        ,[latitude]
        ,[longitude]
        ,[altitude]
        ,[equipamento]
	)
    SELECT
		NEWID(),
		1,
		GETDATE(),
		NULL,
		CAND.U_Codigo,
		OBRA.U_IdPCI,
		0,
		CAND.U_Identif,
		CAND.U_Tipo,
		CAND.U_Nome,
		CAND.U_Detentora,
		CAND.U_IdDetentora,
		CAND.U_CEP,
		ISNULL(OBRA.U_TipoLog,'') + ' ' + ISNULL(OBRA.U_Rua,''),
		CAND.U_Numero,
		CAND.U_Complemento,
		CAND.U_Bairro,
		CAND.U_Cidade,
		CAND.U_Estado,
		CAND.U_Latitude,
		CAND.U_Longitude,
		CAND.U_Altitude,
		CAND.U_Equip

	FROM 
		"@ZPN_OPRJ_CAND" CAND
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = CAND.Code
	WHERE 
		CAND.U_Codigo NOT IN 
		(
			SELECT 
				OC.CODIGO 
			FROM 
				[LINKZCLOUD].[zsistema_producao].[dbo].[obracandidato] OC
			WHERE
				OC.Codigo = CAND.U_Codigo and 
				CAST(OC.[obraid] AS VARCHAR(50)) = OBRA.U_IdPCI
		)

		SELECT TOP 100 * FROM [LINKZCLOUD].[zsistema_producao].[dbo].[obracandidato]
		
		
		
		
		
