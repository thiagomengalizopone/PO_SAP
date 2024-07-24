
CREATE PROCEDURE SP_ZPN_IMPORTARPOERICSSON
as
BEGIN

	select distinct 
		PO po_id,
		PO poNumber,
		GETDATE() po_lis_DataConfirmacao,
		 CASE WHEN ISNULL(ODRF.NumAtCard, ORDR.NumAtCard) is null THEN 'Não Importado' ELSE  'Importado' end as "Status",
		 LOGPO.MensagemLog "Mensagem"

	from 
		ZPN_POERICSSON PO
		LEFT JOIN ORDR ON ORDR."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT JOIN ODRF ON ODRF."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT JOIN ZPN_VW_LISTALOGIMPORTACAOPO LOGPO ON LOGPO.po_id = PO.PO
	ORDER BY
		PO.PO;


END;