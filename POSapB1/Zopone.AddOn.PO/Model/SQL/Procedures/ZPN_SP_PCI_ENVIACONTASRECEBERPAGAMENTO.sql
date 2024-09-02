create PROCEDURE ZPN_SP_PCI_ENVIACONTASRECEBERPAGAMENTO
(
	@DocEntryNF INT,
	@DocentryPagto INT
)
AS 
BEGIN


BEGIN TRY

	--DECLARE @DocEntryNF INT;
	--DECLARE @DocentryPagto INT;


	DECLARE @RowCount INT;
	DECLARE @CurrentRow INT;
	DECLARE @Id varchar(100);
	DECLARE @DocDate DATE;

	DECLARE @UltimaData date;
	DECLARE @UltimaHora int;

	DECLARE @UltimaDataExec date;
	DECLARE @UltimaHoraExec int;

	set @UltimaDataExec = getdate();
	set @UltimaHoraExec = cast(replace(CONVERT(varchar(8), getdate(), 108), ':', '') as int);

	SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = '24');

	SET @UltimaHora = (SELECT isnull(max(HoraExecutado),0) FROM ZPN_INTEGRAPCI WHERE ObjType = '24');
	

	IF EXISTS (SELECT * FROM tempdb.sys.tables 
           WHERE NAME LIKE '#DadosPagamentoPCI%' AND TYPE = 'U')
	BEGIN
		drop table #DadosPagamentoPCI;
	END

	create TABLE #DadosPagamentoPCI (
		ID INT IDENTITY(1,1), -- Coluna de identidade para controlar o loop
		U_IdPci varchar(100),
		DocDate DATE
	);

	Insert Into #DadosPagamentoPCI
	SELECT
		INV6.U_IdPci,  isnull(ORCT.trsfrdate, orct."DocDate") as "DataPagto"
	FROM 
		OINV 
		INNER JOIN INV6 ON INV6."DocEntry"	= OINV."DocEntry"
		INNER JOIN RCT2 ON RCT2."DocEntry"	= INV6."DocEntry" and 
						   RCT2."InvType"	= INV6."ObjType" AND
						   RCT2.InstId		= INV6."InstlmntID"
		INNER JOIN ORCT ON RCT2."DocNum"	= ORCT."DocEntry"
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber] CR
				   ON CR.[contareceberid] = INV6.U_IdPci AND isnull(ORCT.trsfrdate, orct."DocDate") > isnull(cr.recebimento,'2000-01-01')

	WHERE
		(isnull(@DocentryPagto,0) = 0 or ORCT."DocEntry" = @DocentryPagto) and 
		(isnull(@DocEntryNF,0) = 0 or OINV."DocEntry" = @DocEntryNF) and 
		ORCT."Canceled" <> 'Y' AND
		ORCT.CreateDate >= @UltimaData AND
		ORCT.CreateTS >= @UltimaHora; 



	-- Definir o número de registros a serem processados
	SET @RowCount = (SELECT COUNT(*) FROM #DadosPagamentoPCI);
	SET @CurrentRow = 1; -- Começar na primeira linha

	-- Loop WHILE para processar os dados
	WHILE @CurrentRow <= @RowCount
	BEGIN
		-- Buscar o próximo registro usando o IDENTITY da tabela
		SELECT 
			@Id = U_IdPci,
			@DocDate = DocDate
		FROM #DadosPagamentoPCI
		WHERE ID = @CurrentRow;

    
		UPDATE 
			[LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber]
		SET 
			recebimento = @DocDate
		WHERE 
			[contareceberid] = @Id;


		-- Incrementar para a próxima linha
		SET @CurrentRow = @CurrentRow + 1;

	END;

	drop table #DadosPagamentoPCI;

	
	IF (isnull(@DocEntryNF,0) = 0 and isnull(@DocentryPagto,0) = 0 ) 
	BEGIN
		INSERT INTO 
			ZPN_INTEGRAPCI
				(
					ObjType,
					DataExecutado,
					HoraExecutado
				)
			VALUES
				(
					'24',
					@UltimaDataExec,
					@UltimaHoraExec
				);		

	END;


END TRY
BEGIN CATCH
   -- Captura do erro
    DECLARE 
        @ErrorNumber INT = ERROR_NUMBER(),
        @ErrorSeverity INT = ERROR_SEVERITY(),
        @ErrorState INT = ERROR_STATE(),
        @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE(),
        @ErrorLine INT = ERROR_LINE(),
        @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();

    -- Inserir o log de erro na tabela ErrorLog
    INSERT INTO ZPN_LogImportacaoPCI (ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, HostName, ApplicationName, UserName)
    VALUES (@ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage, HOST_NAME(), APP_NAME(), SYSTEM_USER);
    
    -- Opcional: Re-lançar o erro se necessário
    -- THROW; 

END CATCH;

END;