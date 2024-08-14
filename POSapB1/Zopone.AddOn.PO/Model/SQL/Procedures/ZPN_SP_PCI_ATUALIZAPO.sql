CREATE PROCEDURE ZPN_SP_PCI_ATUALIZAPO
(
	@DocEntry int,
	@DataPO datetime
)
AS
BEGIN

	INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[po]
           ([poid]
           ,[gestatus]
           ,[gedataacao]
           ,[gecontaidacao]
           ,[empresaid]
           ,[descricao]
           ,[pedido]
           ,[valor]
           ,[data]
           ,[contratocliente]
           ,[codigo])
     
	 SELECT
		newid(),
		1,
		getdate(),
		null,
		OBPL.U_IdPCI,
		ORDR.Comments,
		ORDR.NumAtCard,
		ORDR.DocTotal,
		ORDR.DocDate,
		OOAT.Descript,
		ORDR.DocEntry
	 FROM	
		ORDR
		INNER JOIN OBPL ON OBPL.BPLId = ORDR.BPLId 
		INNER JOIN RDR1 ON RDR1.DocEntry = ORDR.DocEntry AND RDR1.LineNum = (SELECT MIN(T0.LineNum) FROM RDR1 T0 where T0.DocEntry = ORDR.DocEntry AND isnull(T0.AgrNo,0) <> 0)
		INNER JOIN OOAT ON OOAT.AbsID = RDR1.AgrNo
	WHERE 
		(ORDR.DocEntry = @DocEntry OR ISNULL(@DocEntry,0) = 0) AND
		(ORDR.CreateDate = @DataPO AND ISNULL(@DocEntry,0) = 0) AND
		OBPL.U_IdPCI is not null and
		ORDR.DocEntry NOT IN
		(
			SELECT	
				cast([codigo] as numeric)
			FROM	
				[LINKZCLOUD].[zsistema_aceite].[dbo].PO
			WHERE 
				cast(PO.[codigo] as numeric) = cast(ORDR.DocEntry as numeric) AND 
				PO.[empresaid] = OBPL.U_IdPCI
		);

	update
		ordr
	set
		U_IdPCI = po.[poid]
	FROM
		ORDR 
		INNER JOIN OBPL ON OBPL.BPLId = ORDR.BPLId 
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].PO ON 
										PO.Codigo		= ORDR.DocEntry AND 
										PO.[empresaid]	= OBPL.U_IdPCI
	where 
		(ORDR.DocEntry = @DocEntry OR ISNULL(@DocEntry,0) = 0) AND
		(ORDR.CreateDate = @DataPO AND ISNULL(@DocEntry,0) = 0) AND
		isnull(ORDR.U_IdPCI,'') = '' ;
		
	INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].POitem 
			   ([poitemid]
			   ,[gestatus]
			   ,[gedataacao]
			   ,[gecontaidacao]
			   ,[item]
			   ,[poid]
			   ,[obraid]
			   ,[percentualparcela]
			   ,[datalancamento]
			   ,[datafaturamento]
			   ,[numeronotafiscal]
			   ,[valor]
			   ,[tipo]
			   ,[clienteid]
			   ,[etapaid]
			   ,[observacao]
			   ,[codigo]
			   ,[descricao]
			   ,[obracandidatoid])      

	SELECT 
		NEWID(),
		1,
		GETDATE(),
		NULL,
		RDR1.U_Item,
		ORDR.U_IdPCI,
		OBRA.U_IdPCI,
		0,
		ordr.DocDate,
		rdr1.U_DataFat,
		null,
		rdr1.LineTotal,
		CASE 
			WHEN rdr1.U_Tipo = 'Item' THEN 'I'
			ELSE 'S'
		END,
		CRD8.U_IdPCI,
		ALOC.U_IdPCI,
		RDR1.FreeTxt,
		RDR1.DocEntry,
		NULL,
		CAND.U_IdPCI
	FROM
		RDR1
		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = ORDR.CardCode AND CRD8.BPLId = ORDR.BPLId
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = RDR1.Project
		LEFT  JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = RDR1.U_ItemFat
		LEFT  JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = RDR1.U_Candidato
	WHERE
		(ORDR.DocEntry = @DocEntry OR ISNULL(@DocEntry,0) = 0) AND
		(ORDR.CreateDate = @DataPO AND ISNULL(@DocEntry,0) = 0) AND
		ISNULL(ORDR.U_IdPCI,'') <> '' AND ISNULL(rdr1.U_IdPCI,'') = '';
	


	UPDATE 
		RDR1 
	SET 
		U_IdPCI = POitem.poitemid
	FROM	
		RDR1
		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].POitem POitem ON 
			cast(POitem.POID as varchar(50)) collate SQL_Latin1_General_CP1_CI_AS = ORDR.U_IdPCI 
			AND POitem.Item collate SQL_Latin1_General_CP1_CI_AS  = RDR1.U_Item
	WHERE
		(ORDR.DocEntry = @DocEntry OR ISNULL(@DocEntry,0) = 0) AND
		(ORDR.CreateDate = @DataPO AND ISNULL(@DocEntry,0) = 0) AND
		ISNULL(RDR1.U_IdPCI,'') = '';
	


	UPDATE PO
		SET 
			PO.gestatus = 1,
			PO.gedataacao = GETDATE(),
			PO.gecontaidacao = NULL,
			PO.empresaid = OBPL.U_IdPCI,
			PO.descricao = ORDR.Comments,
			PO.pedido = ORDR.NumAtCard,
			PO.valor = ORDR.DocTotal,
			PO.data = ORDR.DocDate,
			PO.contratocliente = OOAT.Descript
		FROM 
			[LINKZCLOUD].[zsistema_aceite].[dbo].[po] PO
			INNER JOIN ORDR ON ORDR.U_IdPCI = PO.poid
			INNER JOIN OBPL ON OBPL.BPLId = ORDR.BPLId
			INNER JOIN RDR1 ON RDR1.DocEntry = ORDR.DocEntry 
				AND RDR1.LineNum = (SELECT MIN(T0.LineNum) 
									FROM RDR1 T0 
									WHERE T0.DocEntry = ORDR.DocEntry 
									  AND ISNULL(T0.AgrNo, 0) <> 0)
			INNER JOIN OOAT ON OOAT.AbsID = RDR1.AgrNo
			WHERE 
				(ORDR.DocEntry = @DocEntry);



	UPDATE POitem
		SET 
			POitem.gestatus = 1,
			POitem.gedataacao = GETDATE(),
			POitem.gecontaidacao = NULL,
			POitem.item = RDR1.U_Item,
			POitem.poid = ORDR.U_IdPCI,
			POitem.obraid = OBRA.U_IdPCI,
			POitem.percentualparcela = 0,
			POitem.datalancamento = ORDR.DocDate,
			POitem.datafaturamento = RDR1.U_DataFat,
			POitem.numeronotafiscal = NULL,
			POitem.valor = RDR1.LineTotal,
			POitem.tipo = CASE 
							WHEN RDR1.U_Tipo = 'Item' THEN 'I'
							ELSE 'S'
						 END,
			POitem.clienteid = CRD8.U_IdPCI,
			POitem.etapaid = ALOC.U_IdPCI,
			POitem.observacao = RDR1.FreeTxt,
			POitem.descricao = NULL,
			POitem.obracandidatoid = CAND.U_IdPCI
		FROM 
			[LINKZCLOUD].[zsistema_aceite].[dbo].POitem POitem
		INNER JOIN RDR1 ON CAST(POitem.codigo AS NUMERIC) = CAST(RDR1.DocEntry AS NUMERIC)
						AND RDR1.U_IdPCI = POitem.poitemid
		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = ORDR.CardCode AND CRD8.BPLId = ORDR.BPLId
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = RDR1.Project
		LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = RDR1.U_ItemFat
		LEFT JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = RDR1.U_Candidato
		WHERE 

			(ORDR.DocEntry = @DocEntry);

















end;

	
	
