create procedure ZPN_SP_SelecionaItemAtividade
(
	@Atividade varchar(100)
)
AS
BEGIN


	SELECT 
		ItemCode, 
		ItemName
	FROM 
		SBO_TaxOne..CodigoServicoNumero 
	WHERE 
		EntidadeId = 99 and 
		NumeroCodigoServico = @Atividade;

	

END;




