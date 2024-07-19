

CREATE PROCEDURE SP_ZPN_VERIFICAIMPORTARPOHuawei
(
	@DataInicial datetime,
	@DataFinal datetime
)
as
BEGIN

-- SP_ZPN_VERIFICAIMPORTARPOHuawei '2024-07-19', '2024-07-19'
	SELECT DISTINCT
		ORDR.U_IdPO  [po_id],
		ORDR.DocEntry as "DocEntryPO",
		ORDR.NumAtCard AS poNumber,
		ORDR.DocDate,
		'Pedido' "Status",
		 LOGPO.MensagemLog "Mensagem"
	FROM 
		ORDR
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = ORDR.U_IdPO
	where 
		ISNULL(ORDR.U_IdPO,0) > 0 AND 
		ORDR.DocDate between @DataInicial AND @DataFinal 


	UNION ALL 

	SELECT DISTINCT
		ODRF.U_IdPO  [po_id],
		ODRF.DocEntry as "DocEntryPO",
		ODRF.NumAtCard AS poNumber,
		ODRF.DocDate,
		'Esboço' "Status",
		 LOGPO.MensagemLog "Mensagem"
	FROM 
		ODRF
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = ODRF.U_IdPO
	where 
		ISNULL(ODRF.U_IdPO,0) > 0 AND 
		ODRF.DocDate between @DataInicial AND @DataFinal 
	ORDER BY 
		DocDate,
		NumAtCard


END;