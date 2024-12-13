
CREATE PROCEDURE ZPN_SP_PCI_ENVIACANCELAMENTOCONTASRECEBER
(
	@DocEntry INT
)
AS 
BEGIN

--DECLARE @UltimaData date;
--DECLARE @UltimaHora int;
BEGIN TRY



DECLARE @RowCount INT;
DECLARE @CurrentRow INT;
DECLARE @U_IdPci varchar(100);
DECLARE @U_IdPciNF varchar(100);
DECLARE @DocDate DATE;


        
		DECLARE @UltimaData date;
		DECLARE @UltimaHora int;
		DECLARE @UltimaDataExec DATE;
        DECLARE @UltimaHoraExec INT;



        -- Captura a data e hora da execução atual
        SET @UltimaDataExec = GETDATE();
        SET @UltimaHoraExec = CAST(REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') AS INT);

        -- Recupera as últimas execuções
        SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = 'INV-C');
        SET @UltimaHora = (SELECT ISNULL(MAX(HoraExecutado), 0) FROM ZPN_INTEGRAPCI WHERE ObjType ='INV-C');


	declare @DadosCancelamnentoCRPCI TABLE  (
		ID INT IDENTITY(1,1), 
		U_IdPci varchar(250),
		DocDate DATE,
		U_IdPciNF varchar(250)

	);



INSERT INTO @DadosCancelamnentoCRPCI (U_IdPci, DocDate, U_IdPciNF)
SELECT
    INV6.U_IdPci, OINV."DocDate", OINV_ORI.U_IdPCI
FROM 
    OINV 
    INNER JOIN INV1 ON INV1."DocEntry" = OINV."DocEntry"
    INNER JOIN INV1 INV1_ORI ON INV1_ORI."DocEntry" = INV1."BaseEntry" AND
                               INV1_ORI."ObjType" = INV1."BaseType"  
    INNER JOIN INV6 ON INV6."DocEntry" = INV1_ORI."DocEntry"
	INNER JOIN  OINV OINV_ORI ON OINV_ORI.DocEntry  = INV6.DocEntry
WHERE
    OINV.Canceled = 'C' and 

	(INV6.DocEntry = @DocEntry
	OR 
		(
			ISNULL( @DocEntry,0) = 0 AND 
			OINV.CreateDate >= @UltimaData AND
			OINV.CreateTS >= @UltimaHora
		)
	);



-- Definir o número de registros a serem processados
SET @RowCount = (SELECT COUNT(*) FROM @DadosCancelamnentoCRPCI);
SET @CurrentRow = 1; -- Começar na primeira linha

-- Loop WHILE para processar os dados
WHILE @CurrentRow <= @RowCount
BEGIN
    -- Buscar o próximo registro usando o IDENTITY da tabela
    SELECT 
        @U_IdPci = U_IdPci,
        @DocDate = DocDate,
		@U_IdPciNF = U_IdPciNF
    FROM @DadosCancelamnentoCRPCI
    WHERE ID = @CurrentRow;

    
    select  @U_IdPci "nf", @U_IdPciNF "cr", @DocDate;


	EXEC  [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_CancelaNFContasReceber  @U_IdPciNF, @U_IdPci, @DocDate;


    
    SET @CurrentRow = @CurrentRow + 1;

END;

   IF (ISNULL(@DocEntry, 0) = 0)
  BEGIN
       INSERT INTO ZPN_INTEGRAPCI (ObjType, DataExecutado, HoraExecutado)
       VALUES ('INV-C', @UltimaDataExec, @UltimaHoraExec);
   END


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
