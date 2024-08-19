CREATE procedure ZPN_SP_ListaPedidosFaturamento
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @StatusFaturamento varchar(1),
	 @NumAtCard varchar(100)
)
AS
BEGIN
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
	ODRF."DocEntry" "Esboco",
	0 "NF"


FROM
	RDR1 
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
	LEFT JOIN DRF1 ON DRF1."U_BaseEntry" = ORDR."DocEntry" AND DRF1."U_BaseLine" = RDR1."LineNum"
	LEFT JOIN ODRF ON ODRF."DocEntry" = DRF1."DocEntry"
WHERE 
	ISNULL(RDR1."U_StatusFat",'A') = @StatusFaturamento 
	AND (isnull(ORDR."NumAtCard",'') = '' or isnull(@NumAtCard,'') = '')
	AND ORDR."DocDate" between @DataInicial and @DataFinal
ORDER BY
	ORDR."DocDate", ORDR."DocNum", RDR1."LineNum";


END ;

