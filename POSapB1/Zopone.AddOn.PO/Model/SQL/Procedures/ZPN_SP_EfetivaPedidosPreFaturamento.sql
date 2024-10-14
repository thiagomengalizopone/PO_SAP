CREATE PROCEDURE ZPN_SP_EfetivaPedidosPreFaturamento
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
	DRF1."LineNum" "Linha",
	DRF1."U_Item" "Item",
	DRF1."U_Atividade" "Atividade",
	DRF1."U_itemDescription" "Descricao",
	DRF1."LineTotal" "Valor",
	DRF1.DocEntry "Esboco",
	0 "NF",
	DRF1."ItemCode",
	DRF1."Dscription", 
	isnull(FAT."SaldoFaturado",0)"SaldoFaturado",
	(DRF1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "SaldoAberto",
	(DRF1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "TotalFaturar",
	DRF1.U_StatusFat as "Status",
	0 as "TotalDocumento"
FROM
	RDR1 
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
	LEFT JOIN ZPN_VW_TotalFaturadoPedido FAT ON FAT.U_BaseEntry = RDR1."DocEntry" AND FAT.U_BaseLine = RDR1.LineNum
	INNER JOIN DRF1 ON DRF1.U_BaseEntry = RDR1."DocEntry" and DRF1."U_BaseLine" = RDR1."LineNum"
	INNER JOIN ODRF ON ODRF."DocEntry" = DRF1."DocEntry"
WHERE 
	ODRF."DocStatus" = 'O' AND 
	ISNULL(DRF1."U_StatusFat",'A') = 'F'
	AND (isnull(ODRF."NumAtCard",'') = '' or isnull(@NumAtCard,'') = '')
	AND DRF1."DocDate" between @DataInicial and @DataFinal
ORDER BY
	DRF1."DocDate", ODRF."DocNum", DRF1."LineNum";


END ;


