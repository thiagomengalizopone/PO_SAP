﻿CREATE PROCEDURE ZPN_SP_EfetivaPedidosPreFaturamento
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
	ODRF.DocDate "DataT",
	ODRF."CardCode",
	ODRF."CardName",
	DRF1."LineNum" "Linha",
	DRF1."U_Item" "Item",
	DRF1."U_Atividade" "Atividade",
	DRF1."U_itemDescription" "Descricao",
	DRF1."LineTotal" "Valor",
	DRF1.DocEntry "Esboco",
	0 "NF",
	DRF1."ItemCode",
	DRF1."Dscription", 
	
	ODRF.DocTotal "SaldoFaturado",
	0 "SaldoAberto",
	0 "TotalFaturar",
	DRF1.U_StatusFat as "Status",
	0 as "TotalDocumento"
FROM
	ODRF 
	INNER JOIN DRF1 ON ODRF."DocEntry" = DRF1."DocEntry"
	INNER JOIN DRF21 ON DRF21.DocEntry = ODRF.DocEntry  
	LEFT JOIN  ORDR ON ORDR.DocEntry = DRF21.RefDocEntr AND DRF21.RefObjType = ORDR.ObjType
WHERE
	ODRF."DocStatus" = 'O' AND 
	(
		(isnull(ODRF."NumAtCard",'') = @NumAtCard and isnull(@NumAtCard,'') <> '')
		or 
		(isnull(@NumAtCard,'') = '')
	) 
	AND DRF1."DocDate" between @DataInicial and @DataFinal
ORDER BY
	DRF1."DocDate", ODRF."DocNum", DRF1."LineNum";


END ;


