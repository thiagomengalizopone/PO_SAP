create PROCEDURE ZPN_SP_PCI_ENVIACONTASRECEBER
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

	INSERT INTO [LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber]
	(
     			[contareceberid]
			   ,[gestatus]
			   ,[gedataacao]
			   ,[gecontaidacao]
			   ,[empresaid]
			   ,[obraid]
			   ,[valor]
			   ,[valorliquido]
			   ,[valorpis]
			   ,[valorcofins]
			   ,[valorcsll]
			   ,[valorinss]
			   ,[valorirrf]
			   ,[valoriss]
			   ,[emissao]
			   ,[vencimento]
			   ,[programacao]
			   ,[recebimento]
			   ,[cancelamento]
			   ,[codigo]
			   ,[fatura]
			   ,[etapaid]
			   ,[obracandidatoid]
	)
	SELECT 
		newid(),
		1,
		getdate(),
		null, 
		OBPL.U_IdPCI,
		ZPN_OPRJ.U_IdPCI,
		OINV.DocTotal, --valor
		0 [valorliquido],
		isnull(IMP.PIS,0) [valorpis],
		isnull(IMP.COFINS,0) [valorcofins],
		isnull(IMP.CSLL,0) [valorcsll],
		isnull(IMP.INSS,0) [valorinss],	
		isnull(IMP.IRRF,0) [valorirrf],
		isnull(IMP.ISS,0) [valoriss],
		OINV."DocDate",
		INV6."DueDate", 
		null,
		null,
		null,
		cast(oinv."DocEntry" as varchar(10)) + cast(INV6.InstlmntID as varchar(5)),
		cast(oinv.Serial as varchar(10)) + '-' + cast(INV6.InstlmntID as varchar(5)),
		ALOC.U_IdPCI,
		CAND.U_IdPCI

	FROM 
		OINV
		INNER JOIN OBPL							ON OBPL.BPLId		= OINV.BPLId 
		INNER JOIN INV1							ON INV1.DocEntry	= OINV.DocEntry AND INV1.LineNum = (SELECT MIN(T0.LineNum) FROM INV1 T0 where T0.DocEntry = OINV.DocEntry)
		LEFT JOIN  "@ZPN_OPRJ" ZPN_OPRJ			ON ZPN_OPRJ."Code"	= INV1.Project
		INNER JOIN INV6							ON INV6."DocEntry"	= OINV."DocEntry"
		LEFT  JOIN "@ZPN_ALOCA" ALOC			ON ALOC.Code		= INV1.U_ItemFat
		LEFT  JOIN "@ZPN_OPRJ_CAND" CAND		ON CAND.Code		= INV1.U_Candidato
		LEFT JOIN ZPN_VW_DOCUMENTOSIMPOSTO IMP	ON IMP."AbsEntry"	= OINV."DocEntry"
	WHERE
		OINV.CANCELED <> 'Y' and
		(isnull(@DocEntry,0) = 0 or @DocEntry = OINV.DocEntry) and
		ISNULL(ZPN_OPRJ.U_IdPCI,'') <> '' AND 
		isnull(INV6.U_IdPCI,'') = '' and 
		(
			(OINV.CreateDate >= @UltimaData AND OINV.CreateTs >= @UltimaHora) OR 
			(OINV.UpdateDate >= @UltimaData AND OINV.UpdateTs >= @UltimaHora) 
		);


	UPDATE
		INV6 
	set 
		U_IdPCI = CR.[contareceberid]
	FROM
		INV6 PARC
		INNER JOIN OINV														ON PARC."DocEntry"	= OINV."DocEntry"
		INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[contareceber] CR   ON CR.[codigo]      = cast(oinv."DocEntry" as varchar(10)) + cast(PARC.InstlmntID as varchar(5)) AND
																			   CR.[fatura]      = cast(oinv.Serial as varchar(10)) + '-' + cast(PARC.InstlmntID as varchar(5))
	WHERE
		(isnull(@DocEntry,0) = 0 or @DocEntry = PARC.DocEntry) and
		ISNULL(PARC.U_IdPCI,'') = ''
		AND (
			(OINV.CreateDate >= @UltimaData AND OINV.CreateTs >= @UltimaHora) OR 
			(OINV.UpdateDate >= @UltimaData AND OINV.UpdateTs >= @UltimaHora) 
		);

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


	
