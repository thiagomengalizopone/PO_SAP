
CREATE PROCEDURE SP_ZPN_CRIATABELAALOCACAO
(
	@DocEntry int
)
as 
BEGIN

    BEGIN TRY


		INSERT INTO ZPN_ALOCACAOPARCELANF
				(
					DocEntry,
					ObjType,
					TipoDocumento,
					DraftKey,
					Parcela,
					Percentual,
					ValorParcela,
					CodigoAlocacao,
					DescricaoAlocacao,
					IdPCI,
					IdPCIDocumento
				)


		SELECT
			OINV.DocEntry,
			OINV.ObjType,
			'N',
			OINV.draftKey, 
			INV6.InstlmntID,
			INV6.InstPrcnt,
			INV6.InsTotal,
			INV6.U_ItemFat,
			INV6.U_DescItemFat,
			OINV.U_IdPCI,
			NEWID()
		FROM	
			OINV
			INNER JOIN INV6 ON INV6.DocEntry = OINV.DocEntry
			INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry
		WHERE
			ISNULL(INV6.U_ItemFat,'') <> '' AND
			INV6."DocEntry" = @DocEntry
		ORDER BY 
			INV6.InstlmntID;

   END TRY
   BEGIN CATCH
       -- Tratamento de erro
       DECLARE @ErrorMessage NVARCHAR(MAX), @ErrorSeverity INT, @ErrorState INT;
       SELECT 
           @ErrorMessage = ERROR_MESSAGE(), 
           @ErrorSeverity = ERROR_SEVERITY(), 
           @ErrorState = ERROR_STATE();
       -- Insere o erro no log
       INSERT INTO ZPN_LogImportacaoPCI (ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, HostName, ApplicationName, UserName)
       VALUES (ERROR_NUMBER(), @ErrorSeverity, @ErrorState, ERROR_PROCEDURE(), ERROR_LINE(), @ErrorMessage, HOST_NAME(), APP_NAME(), SYSTEM_USER);
       THROW;
   END CATCH
END;
