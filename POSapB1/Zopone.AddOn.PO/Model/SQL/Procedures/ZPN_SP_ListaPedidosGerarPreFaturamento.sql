﻿create procedure [dbo].[ZPN_SP_ListarPedidosGerarPreFaturamento]
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @DataInicialVenc datetime,
	 @DataFinalVEnc datetime,
	 @NumAtCard varchar(100),
	 @CardName varchar(250)
)
AS
BEGIN

SET @NumAtCard =  ISNULL(@NumAtCard,'') ;


declare @qtde int;

set @qtde = 50;

SELECT top 250
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
	ISNULL(ALOCAFAT."Code", '                                              ')	"AlocacaoFAT1",
	ISNULL(ALOCAFAT.U_Desc, '                                              ')	"DescAlocacaoFAT1",
	000000000000.00000		 													"PercFaturar1",
	000000000000.00000		 													"ValorFat1",

	'                                              '							"AlocacaoFAT2",
	'                                              '							"DescAlocacaoFAT2",
	000000000000.00000		 													"PercFaturar2",
	000000000000.00000		 													"ValorFat2",

	'                                              '							"AlocacaoFAT3",
	'                                              '							"DescAlocacaoFAT3",
	000000000000.00000		 													"PercFaturar3",
	000000000000.00000		 													"ValorFat3",

	'                                              '							"AlocacaoFAT4",
	'                                              '							"DescAlocacaoFAT4",
	000000000000.00000		 													"PercFaturar4",
	000000000000.00000		 													"ValorFat4",


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
	OCNT.AbsId,
	isnull(OBRA.U_Estado,'') + ' - ' + isnull(OBRA.U_CidadeDesc,'') "CidadeObra"
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
	ORDR.Canceled <> 'Y' AND 
	ISNULL(RDR1.U_Bloqueado,'N') <> 'Y'  
	AND ISNULL(ORDR.NumAtCard,'') <> ''  
	AND isnull(FAT."SaldoFaturado",0) < RDR1."LineTotal" 
	AND ORDR."DocDate" between @DataInicial and @DataFinal 
	AND ORDR.DocDueDate between @DataInicial and @DataFinal 

	AND 
	(
		(ORDR."CardName" like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
		OR 
		(ORDR.CardCode like '%' + @CardName + '%' or isnull(@CardName,'') = '')  
	)
	and
	(
        (ISNULL(@NumAtCard, '') <> '' 
            AND EXISTS (
                SELECT 1
                FROM STRING_SPLIT(@NumAtCard, ',') AS nums
                WHERE trim(ORDR.NumAtCard) like '%' + trim(nums.value) + '%'
            )
        )
        OR (ISNULL(@NumAtCard, '') = '')
    )
ORDER BY
	ORDR."DocDueDate" desc , ORDR."DocNum", RDR1."LineNum";


END ;




