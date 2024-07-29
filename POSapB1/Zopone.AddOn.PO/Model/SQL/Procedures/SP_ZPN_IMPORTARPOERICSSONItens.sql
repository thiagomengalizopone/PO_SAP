
create PROCEDURE SP_ZPN_IMPORTARPOERICSSONItens
(	
	@po_id Numeric
)
as
BEGIN



	select  
		PO po_id,
		GETDATE() po_lis_DataConfirmacao,
		po.COdigo [itemDescription],
		PO [poNumber],
		0 [shipmentNum],
		0 [quantityCancelled],
		po.qtde AS [quantity],
		PO.Codigo [itemCode],
		PO.Piece [unitPrice],
		po.Descricao "manufactureSiteInfo",
		OPRJ_INST."Code" "IdObra",
		ISNULL(OPRJ_INST."U_BPLId",-1) "Filial",
		po.ITEM
	from 
		ZPN_POERICSSON PO
		LEFT JOIN ORDR ON ORDR."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT JOIN ODRF ON ODRF."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT join "@ZPN_OPRJ" OPRJ_INST on OPRJ_INST.Name = RIGHT(TRIM(PO.Municipio), 2) + '-' + PO.Site 
		LEFT join "@ZPN_OPRJ" OPRJ_CW   on OPRJ_CW.Name = RIGHT(TRIM(PO.Municipio), 2) + '-' + PO.Site + '/CW'
		LEFT join "@ZPN_OPRJ" OPRJ_5G   on OPRJ_5G.Name = RIGHT(TRIM(PO.Municipio), 2) + '-' + PO.Site + '-5G'
		LEFT join "@ZPN_OPRJ" OPRJ_COD2 on OPRJ_COD2.Name = RIGHT(TRIM(PO.Municipio), 2) + '-' + PO.Site + '-2'
	where 
		PO.PO = @po_id

	ORDER BY
		PO.PO;

		
END;