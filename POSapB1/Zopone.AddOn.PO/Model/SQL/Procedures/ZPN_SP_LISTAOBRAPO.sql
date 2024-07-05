CREATE PROCEDURE ZPN_SP_LISTAOBRAPO
(
	@CodObra varchar(50)
)
AS

SELECT DISTINCT
	ORDR."DocEntry" "Id PO",
	ORDR."DocNum" "Código PO",
	ORDR."DocDate" "Data PO",
	ORDR."NumAtCard" "Número Pedido"
FROM 
	RDR1
	INNER JOIN ORDR ON ORDR."DocEntry" = RDR1."DocEntry"
WHERE
	RDR1."Project" = @CodObra;