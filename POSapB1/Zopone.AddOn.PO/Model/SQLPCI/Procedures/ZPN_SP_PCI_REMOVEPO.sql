CREATE PROCEDURE [dbo].ZPN_SP_PCI_REMOVEPO (@DocEntry int)
AS
BEGIN

    BEGIN TRY
	
		DECLARE @poid varchar(150);

	    SELECT 
			@poid = isnull(MAX(ORDR.U_IdPCI),'')
        FROM 
			ORDR
        WHERE 
            ORDR.DocEntry = @DocEntry;


        exec [LINKZCLOUD].[zsistema_aceite].[dbo].[ZPN_PCI_RemovePO] @poid;
          
    END TRY
    BEGIN CATCH
        -- Captura do erro
        DECLARE @ErrorNumber INT = ERROR_NUMBER(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE(),
                @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE(),
                @ErrorLine INT = ERROR_LINE(),
                @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();

        -- Inserir o log de erro na tabela ErrorLog
        INSERT INTO ZPN_LogImportacaoPCI
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            HostName,
            ApplicationName,
            UserName
        )
        VALUES
        (@ErrorNumber,
         @ErrorSeverity,
         @ErrorState,
         ISNULL(@ErrorProcedure, 'ZPN_SP_PCI_INSEREATUALIZAPO'),
         @ErrorLine,
         @ErrorMessage,
         HOST_NAME(),
         APP_NAME(),
         SYSTEM_USER
        );

    -- Opcional: Re-lançar o erro se necessário
    -- THROW; 

    END CATCH;


end;

