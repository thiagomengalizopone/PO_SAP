create PROCEDURE [dbo].[SP_ZPN_VERIFICACADASTROPCI]
(
	@DocEntry INT,
	@TipoDoc int
)
AS 
BEGIN


	-- Declaração do cursor diretamente com o SELECT
	DECLARE CursorResultado CURSOR FOR
	SELECT 
		CRD8."CardCode" AS CodigoCliente,   
		ISNULL(CASE WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 ELSE CRD7.TaxId4 end, '') AS DocumentoPrincipal,
		ISNULL(CRD8."U_IdPci",'') AS IdPciCliente,
		crd8.BPLId,
		OBRA."Code" AS CodigoObra,
		ISNULL(OBRA.U_IdPci, '') AS IdPciObra,
		ISNULL(ALOC."Code", '') AS CodigoAlocacao,
		ISNULL(ALOC.U_IdPci, '') AS IdPciAlocacao,    
		ISNULL(OOAT.AbsID, '') AS CodigoContrato,
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato,
		null AS CodigoAlocacaoParc,
		null AS IdPciAlocacaoParc
	FROM 
		RDR1
		INNER JOIN ORDR ON ORDR.DocEntry = RDR1.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = ORDR.CardCode
			AND CRD8.BPLId = ORDR.BPLId
		INNER JOIN CRD7 ON CRD7.CardCode = CRD8.CardCode 
			AND ISNULL(CRD7.Address, '') = ''
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = RDR1.Project
		INNER JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = RDR1.U_ItemFat
		INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
	WHERE 
		@TipoDoc = 17 and 
		(ISNULL(OOAT.U_IdPCI, '') = '' or  ISNULL(ALOC.U_IdPci, '') = '' or ISNULL(OBRA.U_IdPci, '')  = '' or     ISNULL(CRD8."U_IdPci",'')  = '' ) and 
		ORDR.DocEntry = @DocEntry
		
	UNION ALL 
	SELECT 
		CRD8."CardCode" AS CodigoCliente,   
		ISNULL(CASE WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 ELSE CRD7.TaxId4 end, '') AS DocumentoPrincipal,
		ISNULL(CRD8."U_IdPci",'') AS IdPciCliente,
		crd8.BPLId,
		OBRA."Code" AS CodigoObra,
		ISNULL(OBRA.U_IdPci, '') AS IdPciObra,
		ISNULL(ALOC."Code", '') AS CodigoAlocacao,
		ISNULL(ALOC.U_IdPci, '') AS IdPciAlocacao,    
		ISNULL(OOAT.AbsID, '') AS CodigoContrato,
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato,
		ISNULL(ALOC_PARC."Code", '') AS CodigoAlocacaoParc,
		ISNULL(ALOC_PARC.U_IdPci, '') AS IdPciAlocacaoParc
	FROM 
		DRF1
		INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
		INNER JOIN DRF6 ON drf1.DocEntry = drf6.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = ODRF.CardCode
			AND CRD8.BPLId = ODRF.BPLId
		INNER JOIN CRD7 ON CRD7.CardCode = CRD8.CardCode 
			AND ISNULL(CRD7.Address, '') = ''
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
		LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = DRF1.U_ItemFat
		INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
		LEFT JOIN "@ZPN_ALOCA" ALOC_PARC ON ALOC.Code = DRF6.U_ItemFat
	WHERE 
		@TipoDoc = 112 and 
		--(ISNULL(OOAT.U_IdPCI, '') = '' or  ISNULL(ALOC.U_IdPci, '') = '' or ISNULL(OBRA.U_IdPci, '')  = '' or     ISNULL(CRD8."U_IdPci",'')  = '' ) and 
		ODRF.DocEntry = @DocEntry

	union all 
	SELECT 
		CRD8."CardCode" AS CodigoCliente,   
		ISNULL(CASE WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 ELSE CRD7.TaxId4 end, '') AS DocumentoPrincipal,
		ISNULL(CRD8."U_IdPci",'') AS IdPciCliente,
		crd8.BPLId,
		OBRA."Code" AS CodigoObra,
		ISNULL(OBRA.U_IdPci, '') AS IdPciObra,
		ISNULL(ALOC."Code", '') AS CodigoAlocacaoItem,
		ISNULL(ALOC.U_IdPci, '') AS IdPciAlocacaoItem,    
		ISNULL(OOAT.AbsID, '') AS CodigoContrato,
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato,
		ISNULL(ALOC_PARC."Code", '') AS CodigoAlocacaoParc,
		ISNULL(ALOC_PARC.U_IdPci, '') AS IdPciAlocacaoParc

	FROM 
		INV1
		INNER JOIN OINV ON OINV.DocEntry = INV1.DocEntry
		INNER JOIN inv6 ON OINV.DocEntry = inv6.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = OINV.CardCode
			AND CRD8.BPLId = OINV.BPLId
		INNER JOIN CRD7 ON CRD7.CardCode = CRD8.CardCode 
			AND ISNULL(CRD7.Address, '') = ''
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = INV1.Project
		LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = INV1.U_ItemFat
		LEFT JOIN "@ZPN_ALOCA" ALOC_PARC ON ALOC.Code = INV6.U_ItemFat
		INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
	WHERE 
		@TipoDoc = 13 and 
		--(ISNULL(OOAT.U_IdPCI, '') = '' or  ISNULL(ALOC.U_IdPci, '') = '' or ISNULL(OBRA.U_IdPci, '')  = '' or     ISNULL(CRD8."U_IdPci",'')  = '' ) and 
		OINV.DocEntry = @DocEntry;

	-- Declaração das variáveis que receberão os dados de cada linha
	DECLARE 
		@CodigoCliente VARCHAR(250),
		@DocumentoPrincipal VARCHAR(250),
		@IdPciCliente VARCHAR(250),
		@CodigoObra VARCHAR(250),
		@IdPciObra VARCHAR(250),
		@CodigoAlocacaoItem VARCHAR(250),
		@IdPciAlocacaoItem VARCHAR(250),
		@CodigoContrato VARCHAR(250),
		@IdPciContrato VARCHAR(250),
		@BplId int,
		@CodigoAlocacaoParcela VARCHAR(250),
		@IdPciAlocacaoParcela VARCHAR(250);

	-- Abrir o cursor
	OPEN CursorResultado;

	-- Loop para percorrer os dados do cursor
	FETCH NEXT FROM CursorResultado INTO 
		@CodigoCliente, 
		@DocumentoPrincipal,
		@IdPciCliente,
		@BplId,
		@CodigoObra,
		@IdPciObra,
		@CodigoAlocacaoItem,
		@IdPciAlocacaoItem,
		@CodigoContrato,
		@IdPciContrato,
		@CodigoAlocacaoParcela,
		@IdPciAlocacaoParcela;

	-- Percorrer o cursor linha por linha
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@IdPciCliente = '') begin

			 select @IdPciCliente = ISNULL(MAX(cast([clienteid] as varchar(250))),'') from [LINKZCLOUD].[zsistema_producao].[dbo].[cliente] where [documentoprincipal] = @DocumentoPrincipal;

			 IF (@IdPciCliente = '') 
			 BEGIN
				SET @IdPciCliente = NEWID();
			 END;
		 
			 UPDATE CRD8 SET U_IdPCI = @IdPciCliente WHERE CardCode = @CodigoCliente and BplId = @BplId;

			 exec ZPN_SP_PCI_ATUALIZACLIENTE @CodigoCliente;
		end;




		--if (@IdPciContrato = '') BEGIN 
			exec ZPN_SP_PCI_ATUALIZACONTRATO @CodigoContrato;
		--end;




		--if (@IdPciObra = '') begin
			EXEC [ZPN_SP_PCI_ATUALIZAOBRA] @CodigoObra, NULL;
		--end;
	 	
		--IF (@IdPciAlocacaoItem = '')
		--BEGIN
			--print 'passou1';
			--EXEC [ZPN_SP_PCI_ATUALIZAETAPA] @CodigoAlocacaoItem;
		--END;

		--IF (@IdPciAlocacaoParcela = '')
		--BEGIN
		--print 'passou2';
			--EXEC [ZPN_SP_PCI_ATUALIZAETAPA] @CodigoAlocacaoParcela;
		--END;


		FETCH NEXT FROM CursorResultado INTO 
			@CodigoCliente, 
			@DocumentoPrincipal,
			@IdPciCliente,
			@BplId,
			@CodigoObra,
			@IdPciObra,
			@CodigoAlocacaoItem,
			@IdPciAlocacaoItem,
			@CodigoContrato,
			@IdPciContrato,
			@CodigoAlocacaoParcela,
			@IdPciAlocacaoParcela;
	END;

	-- Fechar e desalocar o cursor
	CLOSE CursorResultado;
	DEALLOCATE CursorResultado;


END;
GO


