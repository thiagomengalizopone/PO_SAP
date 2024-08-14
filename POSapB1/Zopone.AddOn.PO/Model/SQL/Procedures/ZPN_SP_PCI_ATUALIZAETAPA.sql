CREATE PROCEDURE ZPN_SP_PCI_ATUALIZAETAPA
(
	@CodeEtapa varchar(30)
)
AS
BEGIN


	INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[etapa]
			   ([etapaid]
			   ,[gestatus]
			   ,[gedataacao]
			   ,[gecontaidacao]
			   ,[empresaid]
			   ,[nome]
			   ,[codigo]
			   ,[percentualfaturamento]
			   ,[itemfaturamento]
			   ,[itempedido]
			   ,[itemrecebimento])
     
	 
		 SELECT 
			NEWID(),
			1,
			GETDATE(),
			NULL,
			OBPL.U_IdPci COLLATE SQL_Latin1_General_CP1_CI_AS, 
			ALOCA.U_Desc COLLATE SQL_Latin1_General_CP1_CI_AS,
			ALOCA.Code,
			ISNULL(ALOCA.U_Perc, 0),
			CASE WHEN ISNULL(ALOCA.U_ItensFat, '') = 'Y' THEN 1 ELSE 0 END,
			CASE WHEN ISNULL(ALOCA.U_ItensPed, '') = 'Y' THEN 1 ELSE 0 END,
			CASE WHEN ISNULL(ALOCA.U_ItensRec, '') = 'Y' THEN 1 ELSE 0 END
		FROM
			"@ZPN_ALOCA" ALOCA
			INNER JOIN OBPL ON OBPL.BPLId = ALOCA.U_BplId
		WHERE
			ALOCA.Name NOT IN 
			(
				SELECT 
					[nome] COLLATE SQL_Latin1_General_CP1_CI_AS
				FROM 
					[LINKZCLOUD].[zsistema_aceite].[dbo].[etapa] ETP 
				WHERE 
					ETP.Nome COLLATE SQL_Latin1_General_CP1_CI_AS = ALOCA.U_Desc COLLATE SQL_Latin1_General_CP1_CI_AS
					AND CAST(ETP.[empresaid] AS VARCHAR(50)) COLLATE SQL_Latin1_General_CP1_CI_AS = OBPL.U_IdPci COLLATE SQL_Latin1_General_CP1_CI_AS
			) 
			AND
			(ISNULL(@CodeEtapa, '') = '' OR @CodeEtapa = ALOCA."Code") 
			AND OBPL.U_EnviaPCI = 'Y' 
			AND ISNULL(OBPL.U_IdPci, '') <> ''
			AND ISNULL(ALOCA.U_IdPci, '') = '';
	
		
		UPDATE "@ZPN_ALOCA"
			SET U_IdPCI = etapa.etapaid
		FROM
			"@ZPN_ALOCA" ALOCA
			INNER JOIN OBPL ON OBPL.BPLId = ALOCA.U_BplId
			INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[etapa] ETAPA ON 
				ETAPA.nome collate SQL_Latin1_General_CP1_CI_AS = ALOCA.U_Desc and 
				cast(etapa.empresaid  as varchar(50)) = OBPL.U_IdPCI
		WHERE 
			(ISNULL(@CodeEtapa, '') = '' OR @CodeEtapa = ALOCA."Code")  and
			isnull(ALOCA.U_IdPCI,'') <> cast(etapa.etapaid as varchar(50));


	UPDATE [LINKZCLOUD].[zsistema_aceite].[dbo].[etapa]
	SET
		ETP.gestatus = 1,
		ETP.gedataacao = GETDATE(),
		ETP.gecontaidacao = NULL,
		ETP.nome = ALOCA.U_Desc COLLATE SQL_Latin1_General_CP1_CI_AS,
		ETP.codigo = ALOCA.Code,
		ETP.percentualfaturamento = ISNULL(ALOCA.U_Perc, 0),
		ETP.itemfaturamento = CASE WHEN ISNULL(ALOCA.U_ItensFat, '') = 'Y' THEN 1 ELSE 0 END,
		ETP.itempedido = CASE WHEN ISNULL(ALOCA.U_ItensPed, '') = 'Y' THEN 1 ELSE 0 END,
		ETP.itemrecebimento = CASE WHEN ISNULL(ALOCA.U_ItensRec, '') = 'Y' THEN 1 ELSE 0 END
	FROM
		[LINKZCLOUD].[zsistema_aceite].[dbo].[etapa] ETP
		INNER JOIN "@ZPN_ALOCA" ALOCA ON cast(ETP.[etapaid] as varchar(50)) COLLATE SQL_Latin1_General_CP1_CI_AS = ALOCA.U_IdPCI
		INNER JOIN OBPL ON OBPL.BPLId = ALOCA.U_BplId
	WHERE
		(ISNULL(@CodeEtapa, '') = '' OR @CodeEtapa = ALOCA."Code")  ;






END;			
			