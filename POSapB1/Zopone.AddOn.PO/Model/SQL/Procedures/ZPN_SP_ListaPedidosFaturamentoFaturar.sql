create PROCEDURE ZPN_SP_ListaPedidosFaturamentoFaturar
(
	 @DataInicial datetime,
	 @DataFinal datetime,
	 @NumAtCard varchar(100),
	 @Cliente varchar(250)
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
	ORDR.DocDate "Data",
	ORDR.CardCode "CodCliente",
	ORDR.CardName "DescCliente",
	RDR1."LineNum" "Linha",
	RDR1."U_Item" "Item",
	RDR1."U_Atividade" "Atividade",
	RDR1."U_itemDescription" "Descricao",
	RDR1."LineTotal" "Valor",
	0 "Esboco",
	0 "NF",
	RDR1."ItemCode",
	RDR1."Dscription", 
	isnull(FAT."SaldoFaturado",0)"SaldoFaturado",
	(RDR1."LineTotal" - isnull(FAT."SaldoFaturado",0)) "SaldoAberto",
	0.00000000 "TotalFaturar",
	RDR1.U_StatusFat as "Status"

FROM
	RDR1 
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
	LEFT JOIN ZPN_VW_TotalFaturadoPedido FAT ON FAT.U_BaseEntry = RDR1."DocEntry" AND FAT.U_BaseLine = RDR1.LineNum

WHERE
	isnull(ORDR."NumAtCard",'') <> '' and 
	ISNULL(RDR1.U_Bloqueado,'N') <> 'Y'
	AND ISNULL(RDR1."U_StatusFat",'A') = 'A'
	AND (isnull(ORDR."NumAtCard",'') = @NumAtCard or isnull(@NumAtCard,'') = '')
	AND ORDR."DocDate" between @DataInicial and @DataFinal
	AND 
	(
		ISNULL(@Cliente,'') = '' OR 

		(
			ISNULL(@Cliente,'') <> '' AND 
			(
				ORDR.CardCode LIKE '%'+ ISNULL(@Cliente,'') + '%'
				OR 
				ORDR.CardName LIKE '%'+ ISNULL(@Cliente,'') + '%'
			)
		)
	)
ORDER BY
	ORDR."DocDate", ORDR."DocNum", RDR1."LineNum";


END ;

