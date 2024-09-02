alter PROCEDURE ZPN_SP_PCI_ENVIACANCELAMENTOCONTASRECEBER
(
	@UltimaData date,
	@UltimaHora int
)
AS 
BEGIN

--DECLARE @UltimaData date;
--DECLARE @UltimaHora int;
BEGIN TRY



DECLARE @RowCount INT;
DECLARE @CurrentRow INT;
DECLARE @Id varchar(100);
DECLARE @DocDate DATE;


IF EXISTS (SELECT * FROM tempdb.sys.tables 
           WHERE NAME LIKE '##DadosCancelamnentoCRPCI%' AND TYPE = 'U')
	BEGIN
		drop table #DadosPagamentoPCI;
	END


	create TABLE #DadosCancelamnentoCRPCI (
		ID INT IDENTITY(1,1), -- Coluna de identidade para controlar o loop
		U_IdPci varchar(100),
		DocDate DATE
	);


INSERT INTO #DadosCancelamnentoCRPCI (U_IdPci, DocDate)
SELECT
    INV6.U_IdPci, OINV."DocDate"
FROM 
    OINV 
    INNER JOIN INV1 ON INV1."DocEntry" = OINV."DocEntry"
    INNER JOIN INV1 INV1_ORI ON INV1_ORI."DocEntry" = INV1."BaseEntry" AND
                               INV1_ORI."ObjType" = INV1."BaseType"  
    INNER JOIN INV6 ON INV6."DocEntry" = INV1_ORI."DocEntry"
    INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber] CR
    ON CR.[contareceberid] = INV6.U_IdPci AND isnull(CR.cancelamento,'2020-01-01') <> OINV."DocDate"
WHERE
    OINV.Canceled = 'C' and 
	OINV.CreateDate >= @UltimaData AND
	OINV.CreateTS >= @UltimaHora; 

	



-- Definir o número de registros a serem processados
SET @RowCount = (SELECT COUNT(*) FROM ZPN_IntegracaoDadosCancelamento);
SET @CurrentRow = 1; -- Começar na primeira linha

-- Loop WHILE para processar os dados
WHILE @CurrentRow <= @RowCount
BEGIN
    -- Buscar o próximo registro usando o IDENTITY da tabela
    SELECT 
        @Id = U_IdPci,
        @DocDate = DocDate
    FROM #DadosCancelamnentoCRPCI
    WHERE ID = @CurrentRow;

    -- Exemplo de processamento dos dados
    
	UPDATE 
		[LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber]
	SET 
		cancelamento = @DocDate
	WHERE 
		[contareceberid] = @Id;


    -- Incrementar para a próxima linha
    SET @CurrentRow = @CurrentRow + 1;

END;

drop table #DadosCancelamnentoCRPCI;

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

end;
