alter PROCEDURE [dbo].[SP_ZPN_VERIFICAIMPORTARPOHuawei]
(
	@DataInicial datetime,
	@DataFinal datetime,
	@TipoData varchar(10),
	@Empresa varchar(100),
	@IgnorarValidado varchar(100)
)
as
BEGIN


-- SP_ZPN_VERIFICAIMPORTARPOHuawei '2024-07-19', '2024-07-19'
	SELECT DISTINCT
		CAST(ORDR.U_IdPO AS NUMERIC)  [po_id],
		ORDR.DocEntry as "DocEntryPO",
		ORDR.NumAtCard AS poNumber,
		ORDR."CardName" as "Empresa",
		ORDR.DocDate,
		'Pedido' "Status",
		LOGPO.MensagemLog "Mensagem",
		CASE WHEN ISNULL(DOCPO.U_IdPO,0) > 0 THEN 'LINHAS SEM OBRA' ELSE '' END 'DOCTOOBRA'
	FROM 
		ORDR
		INNER JOIN ZPN_VW_STATUSVALIDACAOPEDIDOSPO STAT ON STAT.DocEntry = ORDR.DocEntry
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = ORDR.U_IdPO
		LEFT JOIN ZPN_VW_DOCUMENTOSSEMOBRA    DOCPO ON DOCPO.U_IdPO = ORDR.U_IdPO AND
													   DOCPO."DocEntry" = ORDR."DocEntry" and DOCPO."ObjType" = ORDR."ObjType"  AND
													   DOCPO."Documento" = 'P'
	where 
		(isnull(@IgnorarValidado,'') = 'N' OR (@IgnorarValidado = 'Y' AND isnull(STAT.Validado,'') = 'N' ))  AND
		(isnull(@Empresa,'') = '' or ORDR.CardName like '%' + @Empresa + '%') AND
		ISNULL(ORDR.U_IdPO,0) > 0 AND 
		(
			(ORDR.DocDate between @DataInicial AND @DataFinal and @TipoData = 'P')
			OR 
			(ORDR.CreateDate between @DataInicial AND @DataFinal and @TipoData = 'I')
		)


	UNION

	SELECT DISTINCT
		LOGPO.po_id  [po_id],
		0 as "DocEntryPO",
		cast(LOGPO.po_id as varchar(30)) AS poNumber,
		isnull(LOGPO."Empresa",'') "Empresa",
		LOGPO.DataLog DocDate,
		'Erro de importação' "Status",
		LOGPO.MensagemLog "Mensagem",
		'' 'DOCTOOBRA'
	FROM 
		ZPN_LOGIMPORTACAOPO LOGPO
		LEFT JOIN ODRF  ON LOGPO.po_id = ODRF.U_IdPO 
		LEFT JOIN ORDR	ON LOGPO.po_id = ORDR.U_IdPO
	where 
		(isnull(LOGPO."Empresa",'') = @Empresa or isnull(@empresa,'') = '') and
		ORDR.DocEntry IS NULL AND
		ORDR.DocEntry IS NULL AND
		cast(LOGPO.DataLog  as date) between @DataInicial AND @DataFinal 


	ORDER BY 
		DocDate,
		NumAtCard


END;