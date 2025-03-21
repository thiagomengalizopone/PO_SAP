﻿create PROCEDURE [dbo].[ZPN_SP_PCI_ENVIACONTASRECEBERDESCONTO]
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

		declare @CountAloca int = 0;
		

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
			ROW_NUMBER() OVER (ORDER BY jdt1.transid, jdt1.Line_Id) AS RowNum,
			jdt1.transid,
			jdt1.Line_Id,
			jdt1.Ref3Line AS U_IdPCI,
			1 AS gestatus,
			GETDATE() AS gedataacao,
			NULL AS gecontaidacao, 
			OBPL.U_IdPCI AS empresaid,
			ZPN_OPRJ.U_IdPCI AS obraid,
			JDT1.Debit * parc.Percentual/100 AS valor,
			0 AS valorliquido,
			0 AS valorpis,
			0 AS valorcofins,
			0  AS valorcsll,
			0 AS valorinss,    
			0  AS valorirrf,
			0 AS valoriss,
			OINV.DocDate AS emissao,
			oinv.DocDueDate AS vencimento, 
			oinv.DocDueDate AS programacao,
			NULL AS recebimento,
			NULL AS cancelamento,
			CAST(jdt1.transid AS VARCHAR(10)) + CAST(jdt1.Line_Id AS VARCHAR(5)) AS codigo,

			jdt1.REf1 fatura,
			case 
				when isnull(ALOCA_REC.U_IdPCI,'') <> '' THEN ALOCA_REC.U_IdPCI
				else ALOC.U_IdPCI 
			end AS etapaid,
			CAND.U_IdPCI AS obracandidatoid
		FROM 
			OINV
			INNER JOIN OBPL ON OBPL.BPLId = OINV.BPLId 
			INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry AND INV1.LineNum = (SELECT MIN(T0.LineNum) FROM INV1 T0 WHERE T0.DocEntry = OINV.DocEntry)
			LEFT JOIN "@ZPN_OPRJ" ZPN_OPRJ ON ZPN_OPRJ.Code = 
						case when ISNULL(OINV.Project,'') <> '' THEN OINV.Project
						else INV1.Project
					end 
			INNER JOIN ZPN_ALOCACAOPARCELANF PARC ON PARC.IdPCIDocumento = OINV.U_IdPCI
			INNER JOIN JDT1 ON cast(OINV."DocEntry"  as varchar(20)) = cast(JDT1.Ref2 as varchar(15)) and OINV.Project = JDT1.Project AND JDT1.Debit > 0
			LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = PARC.CodigoAlocacao
			LEFT JOIN "@ZPN_ALOCA" ALOCA_REC ON  ALOCA_REC.Code = ALOC.U_EtapaRec
			LEFT JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code =  ZPN_OPRJ."Code" AND CAND.U_Identif =  INV1.U_Candidato
			LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP.AbsEntry = OINV.DocEntry and IMP.TipoDocumento = 'INV'
			LEFT JOIN sbo_taxOne.[dbo].doc ON DOC.DocType = oinv."ObjType" AND DOC.DocEntry = OINV."DocEntry"
			LEFT JOIN sbo_taxOne.[dbo].Entidade ET on ET.id = doc.EntityId
		WHERE
			JDT1.TransId  = @DocEntry;


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


			if (isnull(@obraid,'') = '') begin
				EXEC [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_RemoveContasReceber @contareceberid;
			
			end;

			
			if (isnull(@obraid,'') <> '') begin

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



					UPDATE JDT1 SET REF3Line = @contareceberid where TransId = @DocEntry and Line_Id = @InstlmntID;

			end;


			-- Incrementa o contador da linha
			SET @RowNum = @RowNum + 1;
		END;


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



