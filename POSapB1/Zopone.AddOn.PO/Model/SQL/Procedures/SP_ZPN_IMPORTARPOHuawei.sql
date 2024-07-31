CREATE PROCEDURE SP_ZPN_IMPORTARPOHuawei
(
	@DataInicial datetime,
	@DataFinal datetime,
	@Reimportar varchar(2)
)
as
BEGIN

-- SP_ZPN_IMPORTARPOHuawei '2024-07-01', '2024-07-18', 'N'
	SELECT DISTINCT
		PO.[po_id],
		 POList.[poNumber],
		 CAST(POList.po_lis_DataConfirmacao AS DATE) po_lis_DataConfirmacao,
		 CASE WHEN ISNULL(ODRF.NumAtCard, ORDR.NumAtCard) is null THEN 'Não Importado' ELSE  'Importado' end as "Status",
		 LOGPO.MensagemLog "Mensagem"
	FROM 
		 [192.168.8.241,15050].Zopone.dbo.POList POList
		INNER JOIN [192.168.8.241,15050].[Zopone].dbo.PO PO ON PO.po_id = POList.po_id
		LEFT JOIN ORDR ON ORDR."NumAtCard" = POList.[poNumber] COLLATE Latin1_General_CI_AS 
		LEFT JOIN ODRF ON ODRF."NumAtCard" = POList.[poNumber] COLLATE Latin1_General_CI_AS 
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = PO.[po_id]
	where 
		PO.po_status = 1 AND
		CAST(POList.po_lis_DataConfirmacao  AS DATE) BETWEEN @DataInicial AND @DataFinal AND 
		(
			@Reimportar = 'Y' OR ORDR.NumAtCard is null
	
		)
	ORDER BY 
		POList.[poNumber],
		POList.po_lis_DataConfirmacao 
DESC 

END;