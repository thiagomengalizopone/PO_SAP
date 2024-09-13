CREATE PROCEDURE ZPN_SP_PCI_ATUALIZACONTRATO
(
	@Descript varchar(250)
)
AS 
BEGIN


--ZPN_SP_PCI_ATUALIZACONTRATO ''




INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[CONTRATO]
           (
			[contratoid]
           ,[gestatus]
           ,[gedataacao]
           ,[gecontaidacao]
           ,[referencia]
           ,[descricao]
           ,[filialid]
           ,[clienteid]
           ,[iniciocontrato]
           ,[terminocontrato]
           ,[datacadastro]
           ,[codigo]
		  )
     
	 SELECT 
		NEWID(),
		1,
		GETDATE(),
		NULL,
		OOAT.Descript,
		OOAT.Remarks,
		OPRC.U_IdPCI,
		CRD8.U_IdPCI,
		OOAT.StartDate,
		OOAT.TermDate,
		GETDATE(),
		ISNULL(OOAT.U_IdZSistemas,0)
	 FROM 
		OOAT
		INNER JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
		INNER JOIN OBPL ON OBPL.BPLId  = OPRC.U_MM_Filial
		INNER JOIN OCRD ON OCRD.CardCode = ooat.BpCode
		INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
						   and CRD8.BPLId = OBPL.BPLId

	WHERE
		ISNULL(OOAT.U_IdPCI,'') = '' and 
		(ISNULL(@Descript,'') = '' OR @Descript = OOAT.Descript)
		AND ISNULL(OOAT.Descript,'') <> '' AND
		OOAT.Descript NOT IN 
			(
				SELECT 
					CONT.[referencia] COLLATE SQL_Latin1_General_CP1_CI_AS
				FROM 
					[LINKZCLOUD].[zsistema_aceite].[dbo].[CONTRATO] CONT
				WHERE 
					CONT.[referencia] COLLATE SQL_Latin1_General_CP1_CI_AS = OOAT.Descript  AND 
					CAST(CONT.[filialid] AS VARCHAR(50))   COLLATE SQL_Latin1_General_CP1_CI_AS = OPRC.U_IdPCI
			);


	UPDATE CONT
	SET
		CONT.gestatus = 1,
		CONT.gedataacao = GETDATE(),
		CONT.gecontaidacao = NULL,
		CONT.descricao = OOAT.Remarks,
		CONT.filialid = OPRC.U_IdPCI,
		CONT.clienteid = CRD8.U_IdPCI,
		CONT.iniciocontrato = OOAT.StartDate,
		CONT.terminocontrato = OOAT.TermDate,
		CONT.datacadastro = GETDATE(),
		CONT.codigo = ISNULL(OOAT.U_IdZSistemas, 0)
	FROM 
		OOAT
		INNER JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
		INNER JOIN OBPL ON OBPL.BPLId = OPRC.U_MM_Filial
		INNER JOIN OCRD ON OCRD.CardCode = OOAT.BpCode
		INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode 
			AND ISNULL(CRD8.DisabledBP, '') <> 'Y' 
			AND CRD8.BPLId = OBPL.BPLId
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[CONTRATO] CONT
			ON cast(CONT.[contratoid] as varchar(50)) COLLATE SQL_Latin1_General_CP1_CI_AS = OOAT.U_IdPCI
	WHERE
		(@Descript = OOAT.Descript)	AND 
		ISNULL(OOAT.Descript, '') <> '';



	UPDATE OOAT
		SET 
			U_IdPCI = CONT.[contratoid],
			U_IdZSistemas = CONT.[codigo]
	FROM 
		OOAT
		INNER JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[CONTRATO] CONT ON 
									CONT.[referencia] COLLATE SQL_Latin1_General_CP1_CI_AS = OOAT.Descript  AND 
									CAST(CONT.[filialid] AS VARCHAR(50))   COLLATE SQL_Latin1_General_CP1_CI_AS = OPRC.U_IdPCI
	WHERE
		ISNULL(OOAT.U_IdPCI,'') = '';


END;