create view VW_ZPN_TotalImpostoRetidoPagamento
as

SELECT 
    'INV' AS TipoDocumento,
    T99."AbsEntry",
    sum(T99.WTAmnt) "WTAmnt"
    FROM 
        INV5 T99
    INNER JOIN OWHT T98 ON T99.WTCode = T98.WTCode
    INNER JOIN OWTT T97 ON T98.WTTypeId = T97.WTTypeId
WHERE
	T99."Category" = 'P'
GROUP BY
	T99."AbsEntry"
    