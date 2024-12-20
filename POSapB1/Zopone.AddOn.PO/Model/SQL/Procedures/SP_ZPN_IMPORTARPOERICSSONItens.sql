﻿create PROCEDURE SP_ZPN_IMPORTARPOERICSSONItens
(	
	@po_id Numeric
)
as
BEGIN



	select  
		PO po_id,
		GETDATE() po_lis_DataConfirmacao,
		PO [poNumber],
		0 [shipmentNum],
		0 [quantityCancelled],
		1 AS [quantity],
		PO.Codigo [itemCode],
		po.qtde * PO.Piece [unitPrice],
		po.Descricao "manufactureSiteInfo",
		OPRJ_INST."Code" "IdObra",
		ISNULL(OPRJ_INST."U_BPLId",-1) "Filial",
		po.ITEM,
		RIGHT(TRIM(PO.Municipio), 2) + '-' + PO.Site "SITE",
		po.ITEM + ' '  + po.COdigo + ' ' + po.Descricao + ' ' + cast(po.qtde as varchar(20)) [itemDescription],
		isnull(OPRJ_INST.U_CodContrato,0)U_CodContrato
		,OPRJ_INST.[U_PCG] "PCG"
		,OPRJ_INST.[U_Regional] "Regional"
		,OPRC."PrcCode" "Obra"
		,isnull(OPRJ_INST."U_CardCode",'') "CardCode"
		,isnull(OOAT.Descript,'') descricaoContrato

	from 
		ZPN_POERICSSON PO
		LEFT JOIN ORDR ON ORDR."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT JOIN ODRF ON ODRF."NumAtCard" = cast(PO.PO as varchar(50))
		LEFT join "@ZPN_OPRJ" OPRJ_INST on OPRJ_INST.Code = dbo.ZPN_FN_RetornaObraPOEricsson(PO.Site, PO.Obra )
		LEFT JOIN OPRC ON OPRC."U_Obra" = OPRJ_INST."Code"  AND OPRC."DimCode" = 2
		LEFT JOIN OOAT ON OOAT."AbsId" = OPRJ_INST.U_CodContrato

	where 
		PO.PO = @po_id

	ORDER BY
		PO.PO;

		
END;