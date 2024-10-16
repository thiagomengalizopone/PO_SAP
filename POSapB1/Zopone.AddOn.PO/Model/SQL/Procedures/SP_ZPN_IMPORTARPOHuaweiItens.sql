﻿create PROCEDURE SP_ZPN_IMPORTARPOHuaweiItens
(	
	@po_id INT
)
as
BEGIN

-- SP_ZPN_IMPORTARPOHuawei '2024-07-01', '2024-07-18', 'N'
	SELECT 
		 PO.[po_id]
		,POList.po_lis_DataConfirmacao
		,POList.[poNumber]
		,POList.[shipmentNum]
		,POList.[quantityCancelled]
		, 1 [quantity]
		,POList.[itemCode]
		,(POList.[quantity] * POList.[unitPrice]) [unitPrice]
		,polist.manufactureSiteInfo
		,"@ZPN_OPRJ"."Code" "IdObra"
		,ISNULL("@ZPN_OPRJ"."U_BPLId",-1) "Filial"
		,polist.poLineNum "ITEM"
		,cast(polist.poLineNum as varchar(10)) + ' ' + POList.[itemDescription] + ' '+ cast(POList.[quantity] as varchar(15))[itemDescription]
		,isnull("@ZPN_OPRJ".U_CodContrato,0) U_CodContrato
		,"@ZPN_OPRJ".[U_PCG] "PCG"
		,"@ZPN_OPRJ".[U_Regional] "Regional"
		,OPRC."PrcCode" "Obra"
		,isnull(OOAT.BpCode,'') "CardCode"

	FROM 
		 [192.168.8.241,15050].Zopone.dbo.POList POList
		INNER JOIN [192.168.8.241,15050].[Zopone].dbo.PO PO ON PO.po_id = POList.po_id
		LEFT join "@ZPN_OPRJ" on dbo.ZPN_FN_RetornaObraPOHuawei(polist.manufactureSiteInfo) = "@ZPN_OPRJ"."Code"
		LEFT JOIN OPRC ON OPRC."U_Obra" = "@ZPN_OPRJ"."Code" AND OPRC."DimCode" = 2
		LEFT JOIN OOAT ON OOAT."AbsId" = "@ZPN_OPRJ".U_CodContrato


	where 
		PO.po_id = @po_id
	ORDER BY 
		POList.[poNumber],
		polist.poLineNum 
DESC 

END;