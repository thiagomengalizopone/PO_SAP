CREATE PROCEDURE [dbo].[ZPN_SP_PCI_ENVIACONTASRECEBER]
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
		

		DECLARE  
			@InstlmntID int,
			@contareceberid VARCHAR(150),
			@gestatus INT,
			@gedataacao datetime,
			@gecontaidacao VARCHAR(50),
			@empresaid VARCHAR(150),
			@obraid VARCHAR(150),
			@valor DECIMAL(18, 2),
			@valorliquido DECIMAL(18, 2),
			@valorpis DECIMAL(18, 2),
			@valorcofins DECIMAL(18, 2),
			@valorcsll DECIMAL(18, 2),
			@valorinss DECIMAL(18, 2),
			@valorirrf DECIMAL(18, 2),
			@valoriss DECIMAL(18, 2),
			@emissao DATETIME,
			@vencimento DATETIME,
			@programacao DATETIME,
			@recebimento DATETIME,
			@cancelamento DATETIME,
			@codigo VARCHAR(50),
			@fatura VARCHAR(50),
			@etapaid VARCHAR(150),
			@obracandidatoid VARCHAR(150)


		-- Captura a data e hora da execução atual
		SET @UltimaDataExec = GETDATE();
		SET @UltimaHoraExec = CAST(REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') AS INT);

		-- Recupera as últimas execuções
		SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = '13');
		SET @UltimaHora = (SELECT ISNULL(MAX(HoraExecutado), 0) FROM ZPN_INTEGRAPCI WHERE ObjType = '13');

		-- Declaração da tabela temporária
		DECLARE @contareceber TABLE
		(
			RowNum int,
			"DocEntry" int, 
			"InstlmntID" int,
			contareceberid VARCHAR(255),
			gestatus INT,
			gedataacao DATETIME,
			gecontaidacao VARCHAR(255),
			empresaid VARCHAR(255),
			obraid VARCHAR(255),
			valor DECIMAL(18, 2),
			valorliquido DECIMAL(18, 2),
			valorpis DECIMAL(18, 2),
			valorcofins DECIMAL(18, 2),
			valorcsll DECIMAL(18, 2),
			valorinss DECIMAL(18, 2),
			valorirrf DECIMAL(18, 2),
			valoriss DECIMAL(18, 2),
			emissao DATETIME,
			vencimento DATETIME,
			programacao DATETIME,
			recebimento DATETIME,
			cancelamento DATETIME,
			codigo VARCHAR(255),
			fatura VARCHAR(255),
			etapaid VARCHAR(255),
			obracandidatoid VARCHAR(255)
		);

		-- Preenche a tabela temporária com os dados da consulta
		INSERT INTO @contareceber
		SELECT 
			ROW_NUMBER() OVER (ORDER BY inv6."DocEntry", inv6."InstlmntID") AS RowNum,
			inv6."DocEntry",
			inv6."InstlmntID",
			ISNULL(INV6.U_IdPCI, '') AS U_IdPCI,
			1 AS gestatus,
			GETDATE() AS gedataacao,
			NULL AS gecontaidacao, 
			OBPL.U_IdPCI AS empresaid,
			ZPN_OPRJ.U_IdPCI AS obraid,
			OINV.GrosProfit * (inv6.InstPrcnt/100) AS valor,
			0 AS valorliquido,
			ISNULL(IMP.PIS, 0) * (inv6.InstPrcnt/100) AS valorpis,
			ISNULL(IMP.COFINS, 0)* (inv6.InstPrcnt/100) AS valorcofins,
			ISNULL(IMP.CSLL, 0)* (inv6.InstPrcnt/100)  AS valorcsll,
			ISNULL(IMP.INSS, 0) * (inv6.InstPrcnt/100) AS valorinss,    
			ISNULL(IMP.IRRF, 0)* (inv6.InstPrcnt/100)  AS valorirrf,
			ISNULL(IMP.ISS, 0) * (inv6.InstPrcnt/100) AS valoriss,
			OINV.DocDate AS emissao,
			INV6.DueDate AS vencimento, 
			INV6.U_DataProgramacao AS programacao,
			NULL AS recebimento,
			NULL AS cancelamento,
			CAST(OINV.DocEntry AS VARCHAR(10)) + CAST(INV6.InstlmntID AS VARCHAR(5)) AS codigo,
			CAST(OINV.Serial AS VARCHAR(10)) + '-' + CAST(INV6.InstlmntID AS VARCHAR(5)) AS fatura,
			ALOC.U_IdPCI AS etapaid,
			CAND.U_IdPCI AS obracandidatoid
		FROM 
			OINV
			INNER JOIN OBPL ON OBPL.BPLId = OINV.BPLId 
			INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry AND INV1.LineNum = (SELECT MIN(T0.LineNum) FROM INV1 T0 WHERE T0.DocEntry = OINV.DocEntry)
			LEFT JOIN "@ZPN_OPRJ" ZPN_OPRJ ON ZPN_OPRJ.Code = INV1.Project
			INNER JOIN INV6 ON INV6.DocEntry = OINV.DocEntry
			LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = INV6.U_ItemFat
			LEFT JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = INV1.U_Candidato
			LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.AbsEntry = OINV.DocEntry
		WHERE
			OINV.CANCELED <> 'Y' 
			AND (ISNULL(@DocEntry, 0) = 0 OR @DocEntry = OINV.DocEntry)
			AND ISNULL(ZPN_OPRJ.U_IdPCI, '') <> '' 
			AND (
				(OINV.CreateDate >= @UltimaData AND OINV.CreateTs >= @UltimaHora) OR 
				(OINV.UpdateDate >= @UltimaData AND OINV.UpdateTs >= @UltimaHora) 
			)
		ORDER BY inv6."DocEntry",	inv6."InstlmntID";

		-- Processa cada linha da CTE
		SET @RowCount = (SELECT COUNT(*) FROM @contareceber);
		
		WHILE @RowNum <= @RowCount
		BEGIN
			-- Atribui os valores de cada linha para as variáveis
			SELECT 
				@DocEntry =  "DocEntry", 
				@InstlmntID ="InstlmntID",
				@contareceberid = contareceberid,
				@gestatus = gestatus,
				@gedataacao = getdate(),
				@gecontaidacao = gecontaidacao,
				@empresaid = empresaid,
				@obraid = obraid,
				@valor = valor,
				@valorliquido = valorliquido,
				@valorpis = valorpis,
				@valorcofins = valorcofins,
				@valorcsll = valorcsll,
				@valorinss = valorinss,
				@valorirrf = valorirrf,
				@valoriss = valoriss,
				@emissao = emissao,
				@vencimento = vencimento,
				@programacao = programacao,
				@recebimento = recebimento,
				@cancelamento = cancelamento,
				@codigo = codigo,
				@fatura = fatura,
				@etapaid = etapaid,
				@obracandidatoid = obracandidatoid
			FROM @contareceber
			WHERE RowNum = @RowNum;

			if (isnull(@contareceberid,'') = '') begin
				set @contareceberid = newid();
			end;




			-- Chama a procedure para inserir ou atualizar
			EXEC [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaContasReceber 
				@contareceberid,
				@gestatus,
				@gecontaidacao,
				@empresaid,
				@obraid,
				@valor,
				@valorliquido,
				@valorpis,
				@valorcofins,
				@valorcsll,
				@valorinss,
				@valorirrf,
				@valoriss,
				@emissao,
				@vencimento,
				@programacao,
				@recebimento,
				@cancelamento,
				@codigo,
				@fatura,
				@etapaid,
				@obracandidatoid;

			update inv6 set U_IdPci = @contareceberid where isnull(u_idpci,'') = '' and DocEntry = @docEntry and InstlmntID = @InstlmntID;

			-- Incrementa o contador da linha
			SET @RowNum = @RowNum + 1;
		END;

		-- Chama procedure para cancelar contas a receber
		EXEC ZPN_SP_PCI_ENVIACANCELAMENTOCONTASRECEBER 0;

		-- Insere o log da execução
		IF (@DocEntry IS NULL OR @DocEntry = 0)
		BEGIN
			INSERT INTO ZPN_INTEGRAPCI (ObjType, DataExecutado, HoraExecutado)
			VALUES ('13', @UltimaDataExec, @UltimaHoraExec);
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
