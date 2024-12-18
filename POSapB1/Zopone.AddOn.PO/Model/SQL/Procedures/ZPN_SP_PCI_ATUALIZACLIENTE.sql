﻿create PROCEDURE SP_ZPN_VERIFICACADASTROPCI
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
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato
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
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato
	FROM 
		DRF1
		INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = ODRF.CardCode
			AND CRD8.BPLId = ODRF.BPLId
		INNER JOIN CRD7 ON CRD7.CardCode = CRD8.CardCode 
			AND ISNULL(CRD7.Address, '') = ''
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
		INNER JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = DRF1.U_ItemFat
		INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
	WHERE 
		@TipoDoc = 112 and 
		(ISNULL(OOAT.U_IdPCI, '') = '' or  ISNULL(ALOC.U_IdPci, '') = '' or ISNULL(OBRA.U_IdPci, '')  = '' or     ISNULL(CRD8."U_IdPci",'')  = '' ) and 
		ODRF.DocEntry = @DocEntry

	union all 
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
		ISNULL(OOAT.U_IdPCI, '') AS IdPciContrato
	FROM 
		INV1
		INNER JOIN OINV ON OINV.DocEntry = INV1.DocEntry
		INNER JOIN CRD8 ON CRD8.CardCode = OINV.CardCode
			AND CRD8.BPLId = OINV.BPLId
		INNER JOIN CRD7 ON CRD7.CardCode = CRD8.CardCode 
			AND ISNULL(CRD7.Address, '') = ''
		INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = INV1.Project
		INNER JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = INV1.U_ItemFat
		INNER JOIN OOAT ON OOAT.AbsId = OBRA.U_CodContrato
	WHERE 
		@TipoDoc = 13 and 
		(ISNULL(OOAT.U_IdPCI, '') = '' or  ISNULL(ALOC.U_IdPci, '') = '' or ISNULL(OBRA.U_IdPci, '')  = '' or     ISNULL(CRD8."U_IdPci",'')  = '' ) and 
		OINV.DocEntry = @DocEntry;

	-- Declaração das variáveis que receberão os dados de cada linha
	DECLARE 
		@CodigoCliente VARCHAR(50),
		@DocumentoPrincipal VARCHAR(50),
		@IdPciCliente VARCHAR(50),
		@CodigoObra VARCHAR(50),
		@IdPciObra VARCHAR(50),
		@CodigoAlocacao VARCHAR(50),
		@IdPciAlocacao VARCHAR(50),
		@CodigoContrato VARCHAR(50),
		@IdPciContrato VARCHAR(50),
		@BplId int;

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
		@CodigoAlocacao,
		@IdPciAlocacao,
		@CodigoContrato,
		@IdPciContrato;

	-- Percorrer o cursor linha por linha
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@IdPciCliente = '') begin
			 exec ZPN_SP_PCI_ATUALIZACLIENTE @CodigoCliente;
		end;

		if (@IdPciContrato = '') BEGIN 
			exec ZPN_SP_PCI_ATUALIZACONTRATO @CodigoContrato;
		end;

		if (@IdPciObra = '') begin
			EXEC [ZPN_SP_PCI_ATUALIZAOBRA] @CodigoObra, NULL;
		end;
	 



		-- Busque a próxima linha
		FETCH NEXT FROM CursorResultado INTO 
			@CodigoCliente, 
			@DocumentoPrincipal,
			@IdPciCliente,
			@BplId,
			@CodigoObra,
			@IdPciObra,
			@CodigoAlocacao,
			@IdPciAlocacao,
			@CodigoContrato,
			@IdPciContrato;
	END;

	-- Fechar e desalocar o cursor
	CLOSE CursorResultado;
	DEALLOCATE CursorResultado;


END;