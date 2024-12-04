create PROCEDURE [dbo].[ZPN_SP_PCI_ENVIACONTASRECEBER]
(
	@DocEntry int
)
AS 
	BEGIN

BEGIN TRY


	DECLARE @UltimaData date;
	DECLARE @UltimaHora int;

	DECLARE @UltimaDataExec date;
	DECLARE @UltimaHoraExec int;

	set @UltimaDataExec = getdate();
	set @UltimaHoraExec = cast(replace(CONVERT(varchar(8), getdate(), 108), ':', '') as int);

	SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = '13');

	SET @UltimaHora = (SELECT isnull(max(HoraExecutado),0) FROM ZPN_INTEGRAPCI WHERE ObjType = '13');

	DECLARE @contareceber TABLE
(
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
    ISNULL(INV6.U_IdPCI, '') AS U_IdPCI,
    1 AS gestatus,
    GETDATE() AS gedataacao,
    NULL AS gecontaidacao, 
    OBPL.U_IdPCI AS empresaid,
    ZPN_OPRJ.U_IdPCI AS obraid,
    OINV.DocTotal AS valor,
    0 AS valorliquido,
    ISNULL(IMP.PIS, 0) AS valorpis,
    ISNULL(IMP.COFINS, 0) AS valorcofins,
    ISNULL(IMP.CSLL, 0) AS valorcsll,
    ISNULL(IMP.INSS, 0) AS valorinss,    
    ISNULL(IMP.IRRF, 0) AS valorirrf,
    ISNULL(IMP.ISS, 0) AS valoriss,
    OINV."DocDate" AS emissao,
    INV6."DueDate" AS vencimento, 
    NULL AS programacao,
    NULL AS recebimento,
    NULL AS cancelamento,
    CAST(OINV."DocEntry" AS VARCHAR(10)) + CAST(INV6.InstlmntID AS VARCHAR(5)) AS codigo,
    CAST(OINV.Serial AS VARCHAR(10)) + '-' + CAST(INV6.InstlmntID AS VARCHAR(5)) AS fatura,
    ALOC.U_IdPCI AS etapaid,
    CAND.U_IdPCI AS obracandidatoid
FROM 
    OINV
    INNER JOIN OBPL ON OBPL.BPLId = OINV.BPLId 
    INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry AND INV1.LineNum = (SELECT MIN(T0.LineNum) FROM INV1 T0 WHERE T0.DocEntry = OINV.DocEntry)
    LEFT JOIN "@ZPN_OPRJ" ZPN_OPRJ ON ZPN_OPRJ."Code" = INV1.Project
    INNER JOIN INV6 ON INV6."DocEntry" = OINV."DocEntry"
    LEFT JOIN "@ZPN_ALOCA" ALOC ON ALOC.Code = INV1.U_ItemFat
    LEFT JOIN "@ZPN_OPRJ_CAND" CAND ON CAND.Code = INV1.U_Candidato
    LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP ON IMP."AbsEntry" = OINV."DocEntry"
WHERE
    OINV.CANCELED <> 'Y' 
    AND (ISNULL(@DocEntry, 0) = 0 OR @DocEntry = OINV.DocEntry)
    AND ISNULL(ZPN_OPRJ.U_IdPCI, '') <> '' 
    AND ISNULL(INV6.U_IdPCI, '') = '' 
    AND (
        (OINV.CreateDate >= @UltimaData AND OINV.CreateTs >= @UltimaHora) OR 
        (OINV.UpdateDate >= @UltimaData AND OINV.UpdateTs >= @UltimaHora) 
    );

    -- Declaração das variáveis para armazenar os dados de cada linha
    DECLARE 
        @contareceberid VARCHAR(255),
        @gestatus INT,
        @gedataacao DATETIME,
        @gecontaidacao VARCHAR(255),
        @empresaid VARCHAR(255),
        @obraid VARCHAR(255),
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
        @codigo VARCHAR(255),
        @fatura VARCHAR(255),
        @etapaid VARCHAR(255),
        @obracandidatoid VARCHAR(255);

    -- Início do laço WHILE para iterar sobre os dados da tabela @contareceber
    DECLARE @RowCount INT = (SELECT COUNT(*) FROM @contareceber);
    DECLARE @CurrentRow INT = 1;

    WHILE @CurrentRow <= @RowCount
    BEGIN
        -- Atribui os valores de cada linha da tabela temporária para as variáveis
        SELECT 
            @contareceberid = contareceberid,
            @gestatus = gestatus,
            @gedataacao = gedataacao,
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
        WHERE ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) = @CurrentRow;

        -- Aqui você pode fazer o que quiser com as variáveis, por exemplo, inseri-las em outra tabela
        EXEC [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaContasReceber 
             @contareceberid,
            @gestatus,
            @gedataacao,
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
    

        -- Incrementa o contador da linha
        SET @CurrentRow = @CurrentRow + 1;
END;





	EXEC ZPN_SP_PCI_ENVIACANCELAMENTOCONTASRECEBER @UltimaData, @UltimaHora;

	IF (isnull(@DocEntry,0) = 0) 
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
					'13',
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


	