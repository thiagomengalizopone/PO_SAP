

CREATE VIEW ZPN_VW_TotalFaturadoPedido
as

SELECT
	INV1.U_BaseEntry,
	INV1.U_BaseLine,
	isnull(SUM(INV1."LineTotal"),0) "LineTotal",
	0 as "TotalDevolucao",
	isnull(SUM(INV1."LineTotal"),0) "SaldoFaturado"
	
FROM
	INV1 
	INNER JOIN OINV ON OINV."DocEntry"  = INV1."DocEntry"
WHERE
	OINV."CANCELED"  = 'N'
GROUP BY
	INV1.U_BaseEntry,
	INV1.U_BaseLine;






	