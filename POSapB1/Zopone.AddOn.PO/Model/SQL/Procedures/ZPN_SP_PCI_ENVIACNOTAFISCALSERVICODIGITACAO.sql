CREATE PROCEDURE [dbo].[ZPN_SP_PCI_ENVIACNOTAFISCALSERVICODIGITACAO]
(
    @DocEntry INT
)
AS
BEGIN
    BEGIN TRY


        -- Declaração das variáveis
        DECLARE @UltimaData DATE;
        DECLARE @UltimaHora INT;

        DECLARE @UltimaDataExec DATE;
        DECLARE @UltimaHoraExec INT;

        DECLARE @RowNum INT = 1;
        DECLARE @RowCount INT = 1;

		DECLARE @RowNumParcela INT = 1;
        DECLARE @RowCountParcela INT = 1;
        
		declare
			@nfeservicoid varchar(255),
            @sequencia INT,
            @obraid VARCHAR(255),
			@emissao datetime,
			@valor DECIMAL(18,6),
            @situacao int, 
            @obracandidatoid VARCHAR(255),
			@IdPCI varchar(250);


		declare 
			@RowNumParc INT,
			@InstlmntID int,
			@nfeservicopardelaid  varchar(250),
			@gestatus INT,
			@gedataacao DATETIME,
			@gecontaidacao  varchar(250),
			@sequenciaParc INT,
			@vencimento DATETIME,
			@valorParc DECIMAL(18,2),
			@fatura VARCHAR(200),
			@percentual FLOAT,
			@etapaid  varchar(250);

        -- Captura a data e hora da execução atual
        SET @UltimaDataExec = GETDATE();
        SET @UltimaHoraExec = CAST(REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') AS INT);

        -- Recupera as últimas execuções
        SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = 'DRF');
        SET @UltimaHora = (SELECT ISNULL(MAX(HoraExecutado), 0) FROM ZPN_INTEGRAPCI WHERE ObjType ='DRF');

        -- Declaração da tabela temporária
        DECLARE @nfeservico TABLE
        (
            RowNum INT,
            nfeservicoid VARCHAR(250),
            sequencia INT,
            obraid VARCHAR(255),
			emissao datetime,
			valor decimal(18,6),
            situacao int, 
            obracandidatoid VARCHAR(255),
			IdPCI varchar(250)
        );

		
		DECLARE @nfeservicoparcela TABLE
		(
			RowNumParc INT,
			InstlmntID int,
			nfeservicopardelaid varchar(250),
			gestatus INT,
			gedataacao DATETIME,
			gecontaidacao  varchar(250),
			nfeservicoid  varchar(250),
			sequenciaParc INT,
			vencimento DATETIME,
			valorParc DECIMAL(18,2),
			fatura VARCHAR(200),
			percentual FLOAT,
			etapaid  varchar(250)
		);

		
        -- Preenche a tabela temporária com os dados da consulta
        INSERT INTO @nfeservico
        SELECT 
            ROW_NUMBER() OVER (ORDER BY ODRF.DocEntry, ZPN_OPRJ.U_IdPCI) AS RowNum,
            ODRF.DocEntry,
            ODRF.DocEntry,
            ZPN_OPRJ.U_IdPCI AS obraid,
			ODRF.DocDate,
			DRF1.LineTotal,
            1,
            CAND.U_IdPCI AS obracandidatoid,
			isnull(ODRF.U_IdPCI,'') 
        FROM 
            ODRF
            INNER JOIN OBPL ON OBPL.BPLId = ODRF.BPLId 
			INNER JOIN DRF1 ON DRF1."DocEntry" = ODRF."DocEntry"
            LEFT JOIN "@ZPN_OPRJ" ZPN_OPRJ ON ZPN_OPRJ.Code = DRF1.Project
            LEFT JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = DRF1.U_Candidato
        WHERE
			ODRF."ObjType" = 13 and 
            ODRF.CANCELED <> 'Y' 
            AND (ISNULL(@DocEntry, 0) = 0 OR @DocEntry = ODRF.DocEntry)
            AND (
				ISNULL(@DocEntry, 0) <> 0 or
					
                ((ODRF.CreateDate >= @UltimaData AND ODRF.CreateTs >= @UltimaHora) OR 
                (ODRF.UpdateDate >= @UltimaData AND ODRF.UpdateTs >= @UltimaHora)
				)
            )
        ORDER BY  ODRF.DocEntry, ZPN_OPRJ.U_IdPCI;

        -- Processa cada linha da tabela temporária
        SET @RowCount = (SELECT COUNT(*) FROM @nfeservico);
        
        WHILE @RowNum <= @RowCount
        BEGIN
            -- Atribui os valores de cada linha para as variáveis
            SELECT 
                @RowNum = RowNum,
				@nfeservicoid = nfeservicoid ,
				@sequencia = sequencia ,
				@obraid = obraid ,
				@valor = valor,
				@emissao = emissao ,
				@situacao = situacao, 
				@obracandidatoid = obracandidatoid ,
				@IdPCI  = IdPCI 
			FROM 
				@nfeservico
            WHERE RowNum = @RowNum;

			delete from @nfeservicoparcela;

			-- Verifica se contareceberid está vazio e gera um novo se necessário
            IF (ISNULL(@IdPCI, '') = '') 
            BEGIN
                SET @IdPCI = NEWID();
            END;
			
			INSERT INTO @nfeservicoparcela
			SELECT 
	            ROW_NUMBER() OVER (ORDER BY ODRF.DocEntry, DRF6.DueDate) AS RowNum,
				DRF6.InstlmntID,
				ISNULL(DRF6.U_IdPCI,''),
				1,
				GETDATE(),
				NULL,
				@IdPci,
				ODRF.DocEntry,
				DRF6.DueDate,
				DRF6.InsTotal,
				ODRF.NumAtCard,
				DRF6.InstPrcnt,
				isnull(ALOCA.U_IdPCI,'')

			FROM 
				DRF6
				INNER JOIN ODRF ON ODRF.DocEntry = DRF6.DocEntry
				INNER JOIN DRF1 ON DRF1.DocEntry = ODRF.DocEntry
				INNER JOIN "@ZPN_ALOCA" ALOCA ON 
					case when isnull(DRF6.U_ItemFat,'') <> '' then DRF6.U_ItemFat
					else DRF1.U_ItemFat
				end 
					= ALOCA.Code
			WHERE 
				ODRF.DocEntry = @nfeservicoid
			ORDER BY ODRF.DocEntry, DRF6.DueDate;  

			EXEC [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaNfeservico 
				@IdPCI ,
				@sequencia,
				@obraid,
				@emissao,
				@valor,
				@situacao, 
				@obracandidatoid;

			SET @RowCountParcela = (SELECT COUNT(*) FROM @nfeservicoparcela);
			SET @RowNumParcela = 1;



			--[ZPN_SP_PCI_ENVIACNOTAFISCALSERVICODIGITACAO] 17


			WHILE @RowNumParcela <= @RowCountParcela 
			BEGIN
				SELECT 
					@InstlmntID = InstlmntID,
					@nfeservicopardelaid = nfeservicopardelaid,
					@gestatus = gestatus,
					@gedataacao = gedataacao,
					@gecontaidacao = gecontaidacao,
					@sequenciaParc = sequenciaParc,
					@vencimento = vencimento,
					@valorParc = valorParc,
					@fatura = fatura,
					@percentual = percentual,
					@etapaid = etapaid
				FROM 
					@nfeservicoparcela
				WHERE RowNumParc = @RowNumParcela;

				IF (ISNULL(@nfeservicopardelaid, '') = '') 
				BEGIN
					SET @nfeservicopardelaid = NEWID();
				END;
				
				EXEC [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaNfeservicoParcela 
															 @nfeservicopardelaid,
															@gestatus,
															@gedataacao,
															@gecontaidacao,
															@IdPCI,
															@sequencia,
															@vencimento,
															@valor,
															@fatura,
															@percentual ,
															@etapaid;



				UPDATE DRF6 SET U_IdPCI = @nfeservicopardelaid WHERE ISNULL(U_IdPCI,'') = '' AND DocEntry = @nfeservicoid AND InstlmntID = @InstlmntID; 

				set @RowNumParcela = @RowNumParcela+1;

			END;



			UPDATE ODRF SET U_IdPCI = @IdPCI WHERE ISNULL(U_IdPCI,'') = '' AND DocEntry = @nfeservicoid;

            SET @RowNum = @RowNum + 1;
        END;
		
		

         
        IF (ISNULL(@DocEntry, 0) = 0)
       BEGIN
            INSERT INTO ZPN_INTEGRAPCI (ObjType, DataExecutado, HoraExecutado)
            VALUES ('DRF', @UltimaDataExec, @UltimaHoraExec);
        END
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
END
