create procedure ZPN_SP_ListaPedidosGerarPreFaturamento
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @NumAtCard varchar(100)
)
AS
BEGIN


--ZPN_SP_ListaPedidosGerarPreFaturamento '2024-08-28', '2024-08-28', ''

--DECLARE @StatusFaturamento varchar(1);
--DECLARE @NumAtCard varchar(100);
--DECLARE @DataInicial datetime;
--DECLARE @DataFinal datetime;

--set @DataInicial = '2024-01-01';
--set @DataFinal = '2025-01-01';

--set @StatusFaturamento = 'A';
SELECT 
	'N' "Selecionar",
	ORDR."DocEntry" "Pedido",
	ORDR."NumAtCard" "PO",
	RDR1."LineNum" "Linha",
	RDR1."U_Item" "Item",
	RDR1."U_Atividade" "Atividade",
	RDR1."U_itemDescription" "Descricao",
	RDR1."LineTotal" "Valor",
	ALOCA.U_Desc "Alocacao",
	ISNULL(ALOCAFAT."Code", '           ') "AlocacaoFAT",
	ISNULL(ALOCAFAT.U_Desc, '                                              ') "DescAlocacaoFAT",
	RDR1."ItemCode",
	RDR1."Dscription", 
	isnull(FAT."SaldoFaturado",0)"SaldoFaturado",
	(RDR1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "SaldoAberto",
	(RDR1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "TotalFaturar",
	RDR1.U_StatusFat as "Status",
	0 as "TotalDocumento",
	RDR1.OcrCode "Obra"
FROM
	RDR1 
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
	LEFT JOIN ZPN_VW_TotalFaturadoPedido FAT ON FAT.U_BaseEntry = RDR1."DocEntry" AND FAT.U_BaseLine = RDR1.LineNum
	LEFT JOIN "@ZPN_ALOCA" ALOCA    ON ALOCA."Code" = RDR1.U_ItemFat 
	LEFT JOIN "@ZPN_ALOCA" ALOCAFAT ON ALOCAFAT."Code" = ALOCA.U_EtapaFat
WHERE 
	ISNULL(RDR1.U_Bloqueado,'N') <> 'Y' 
	AND isnull(FAT."SaldoFaturado",0) < RDR1."LineTotal" and
	ISNULL(RDR1."U_StatusFat",'A') = 'F'
	AND (isnull(ORDR."NumAtCard",'') = '' or isnull(@NumAtCard,'') = '')
	AND ORDR."DocDate" between @DataInicial and @DataFinal
ORDER BY
	ORDR."DocDate", ORDR."DocNum", RDR1."LineNum";


END ;


