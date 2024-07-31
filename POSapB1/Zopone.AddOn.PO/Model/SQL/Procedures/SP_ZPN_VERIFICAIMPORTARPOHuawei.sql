
CREATE PROCEDURE [dbo].[SP_ZPN_VERIFICAIMPORTARPOHuawei]
(
	@DataInicial datetime,
	@DataFinal datetime
)
as
BEGIN

-- SP_ZPN_VERIFICAIMPORTARPOHuawei '2024-07-19', '2024-07-19'
	SELECT DISTINCT
		CAST(ORDR.U_IdPO AS NUMERIC)  [po_id],
		ORDR.DocEntry as "DocEntryPO",
		ORDR.NumAtCard AS poNumber,
		ORDR.DocDate,
		'Pedido' "Status",
		LOGPO.MensagemLog "Mensagem",
		CASE WHEN ISNULL(DOCPO.U_IdPO,0) > 0 THEN 'LINHAS SEM OBRA' ELSE '' END 'DOCTOOBRA'
	FROM 
		ORDR
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = ORDR.U_IdPO
		LEFT JOIN ZPN_VW_DOCUMENTOSSEMOBRA    DOCPO ON DOCPO.U_IdPO = ORDR.U_IdPO AND
													   DOCPO."DocEntry" = ORDR."DocEntry" and DOCPO."ObjType" = ORDR."ObjType"  AND
													   DOCPO."Documento" = 'P'
	where 
		ISNULL(ORDR.U_IdPO,0) > 0 AND 
		ORDR.DocDate between @DataInicial AND @DataFinal 

	UNION ALL 

	SELECT DISTINCT
		CAST(ODRF.U_IdPO AS NUMERIC)  [po_id],
		ODRF.DocEntry as "DocEntryPO",
		ODRF.NumAtCard AS poNumber,
		ODRF.DocDate,
		'Esboço' "Status",
		LOGPO.MensagemLog "Mensagem",
		CASE WHEN ISNULL(DOCPO.U_IdPO,0) > 0 THEN 'LINHAS SEM OBRA' ELSE '' END 'DOCTOOBRA'
	FROM 
		ODRF
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = ODRF.U_IdPO 
		LEFT JOIN ORDR							    ON LOGPO.po_id = ORDR.U_IdPO
		LEFT JOIN ZPN_VW_DOCUMENTOSSEMOBRA    DOCPO ON DOCPO.U_IdPO = ODRF.U_IdPO AND
													   DOCPO."DocEntry" = ODRF."DocEntry" and DOCPO."ObjType" = ODRF."ObjType"  AND
													   DOCPO."Documento" = 'E'
	where 
		ISNULL(ODRF.U_IdPO,0) > 0 AND 
		ORDR.DocEntry IS NULL AND
		ODRF.DocDate between @DataInicial AND @DataFinal 

	UNION

	SELECT DISTINCT
		LOGPO.po_id  [po_id],
		0 as "DocEntryPO",
		cast(LOGPO.po_id as varchar(30)) AS poNumber,
		LOGPO.DataLog DocDate,
		'Erro de importação' "Status",
		LOGPO.MensagemLog "Mensagem",
		'' 'DOCTOOBRA'
	FROM 
		ZPN_LOGIMPORTACAOPO LOGPO
		LEFT JOIN ODRF  ON LOGPO.po_id = ODRF.U_IdPO 
		LEFT JOIN ORDR	ON LOGPO.po_id = ORDR.U_IdPO
	where 
		ORDR.DocEntry IS NULL AND
		ORDR.DocEntry IS NULL AND
		cast(LOGPO.DataLog  as date) between @DataInicial AND @DataFinal 


	ORDER BY 
		DocDate,
		NumAtCard


END;