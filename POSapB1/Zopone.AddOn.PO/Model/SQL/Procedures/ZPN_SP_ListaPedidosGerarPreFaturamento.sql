create procedure ZPN_SP_ListaPedidosGerarPreFaturamento
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @NumAtCard varchar(100),
	 @CardName varchar(250)
)
AS
BEGIN

SET @NumAtCard =  ISNULL(@NumAtCard,'') ;


SELECT top 50
	'N' "Selecionar",
	ORDR."DocEntry" "Pedido",
	GetDate() "PrevFat",
	ORDR."NumAtCard" "PO",
	ORDR.CardName "Cliente",
	ORDR.CardCode "CodCliente",
	case 
		when isnull(CRD7.TaxId0,'') <> '' then CRD7.TaxId0
		else CRD7.TaxId4 
	end "CNPJ",
	RDR1."LineNum" "Linha",
	RDR1."U_Item" "Item",
	RDR1."U_Atividade" "Atividade",
	RDR1."U_itemDescription" "Descricao",
	RDR1."LineTotal" "Valor",
	ALOCA.U_Desc "Alocacao",
	ISNULL(ALOCAFAT."Code", '                                              ') "AlocacaoFAT",
	ISNULL(ALOCAFAT.U_Desc, '                                              ') "DescAlocacaoFAT",
	00000.00000									"PercFaturar",
	isnull(FAT."SaldoFaturado",0)"SaldoFaturado",
	(RDR1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "SaldoAberto",
	(RDR1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "TotalFaturar",
	RDR1.U_StatusFat as "Status",
	0000.00000											 as "TotalDocumento",
	OBRA.Code											"Obra",
	OOAT.Remarks										"Contrato",
	RDR1.ItemCode,
	RDR1.Dscription,
	case 
		when ISNULL(ocnt.ibgecode,'') = '' then '                    '
		else ISNULL(ocnt.ibgecode,'')
	end as "IbgeCode",
	OCNT.State								 	 AS "Estado",
	OCNT.Name							 		 as "Cidade",
	OCNT.AbsId
FROM
	RDR1 
	INNER JOIN ORDR								ON ORDR."DocEntry" = RDR1."DocEntry"
	INNER JOIN OCRD								ON OCRD."CardCode" = ORDR."CardCode"
	INNER JOIN CRD7								ON CRD7."CardCode" = OCRD."CardCode" and isnull(CRD7.Address,'') = '' 
	INNER JOIN "@ZPN_OPRJ"	OBRA				ON OBRA."Code"	   = RDR1.Project
	INNER JOIN OOAT								ON OOAT.AbsId	   = OBRA.U_CodContrato
	LEFT JOIN ZPN_VW_TotalFaturadoPedido FAT	ON FAT.U_BaseEntry = RDR1."DocEntry" AND FAT.U_BaseLine = RDR1.LineNum
	LEFT JOIN "@ZPN_ALOCA" ALOCA				ON ALOCA."Code" = RDR1.U_ItemFat 
	LEFT JOIN "@ZPN_ALOCA" ALOCAFAT				ON ALOCAFAT."Code" = ALOCA.U_EtapaFat
	LEFT JOIN OCNT								ON OCNT.State	   = OBRA.U_Estado AND OCNT.Name = OBRA.U_Cidade
WHERE 
	ISNULL(RDR1.U_Bloqueado,'N') <> 'Y'  
	AND ISNULL(ORDR.NumAtCard,'') <> ''  
	AND isnull(FAT."SaldoFaturado",0) < RDR1."LineTotal" 
	AND ORDR."DocDate" between @DataInicial and @DataFinal 
	AND 
	(
		(ORDR."CardName" like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
		OR 
		(ORDR.CardCode like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
	)
	AND (ISNULL(@NumAtCard,'') = '' or (ORDR.NumAtCard like '%' + @NumAtCard + '%' AND isnull(ORDR.NumAtCard,'') <> '') )
ORDER BY
	ORDR."DocDate", ORDR."DocNum", RDR1."LineNum";


END ;




