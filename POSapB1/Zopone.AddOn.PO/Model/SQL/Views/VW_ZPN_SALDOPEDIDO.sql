CREATE VIEW VW_ZPN_SALDOPEDIDO
AS 

SELECT 
			T0."DocEntry",
			CASE 
			WHEN T0.DocStatus = 'O' 
			THEN T0.DocTotal - isnull((select sum(T10.[Valor NF]) from MM_PedidosDeCompraComNFEntrada T10 where T10.OC = T0.DocEntry), 0) 
			WHEN T0.DocStatus = 'C' 
				AND (SELECT 
					ISNULL(SUM(T100.Quantity),0) 
					FROM PDN1 T100
					INNER JOIN OPDN T101 ON T100.DocEntry = T101.DocEntry
					WHERE T100.BaseType = 22 
					AND T101.DocEntry NOT IN (SELECT ISNULL(RefDocEntr,0) FROM PCH21 T10
										INNER JOIN OPCH T11 ON T10.DocEntry = T11.DocEntry
										WHERE T10.RefObjType = 20 
										AND T11.CANCELED = 'N')
					AND T101.DocStatus = 'O' 
					AND T101.CANCELED = 'N'
					AND ISNULL(T101.U_SituacaoDocumento,'') <> '02'
					AND T100.BaseEntry = T0.DocEntry) <> 0
			THEN T0.DocTotal - isnull((select ISNULL(sum(T10.[Valor NF]),0) from MM_PedidosDeCompraComNFEntrada T10 where T10.OC = T0.DocEntry), 0)
			WHEN T0.DocStatus = 'C' AND T0.DocManClsd = 'Y' THEN 0
			ELSE 0
			END
			as "Saldo"
		FROM 
				OPOR T0
