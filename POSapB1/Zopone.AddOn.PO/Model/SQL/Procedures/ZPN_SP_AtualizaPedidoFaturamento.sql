CREATE procedure ZPN_SP_AtualizaPedidoFaturamento
(
	@DocEntry int,
	@LineNum int
)
AS
BEGIN


	UPDATE 
		RDR1 
	SET 
		U_StatusFat = 'F' 
	WHERE 
		DocEntry = @DocEntry and 
		LineNum = @LineNum;


END ;

