CREATE PROCEDURE SP_ZPN_REMOVENFSERVICO (@DocEntry INT)
AS
BEGIN
	BEGIN TRY

		DECLARE @NFSERVICOID VARCHAR(250);

		SELECT 
			@NFSERVICOID = ISNULL(MAX(U_IdPCI), '')
		FROM 
			ODRF 
		WHERE 
			DocEntry = @DocEntry;

		IF (@NFSERVICOID  <> '') BEGIN
			EXEC [LINKZCLOUD].[zsistema_producao].[dbo].ZPN_PCI_RemoveNfeservico @NFSERVICOID;							
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
