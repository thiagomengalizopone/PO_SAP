CREATE PROCEDURE ZPN_SP_ATUALIZAOBRASAP
(
		@endereco varchar(50),
		@numero varchar(10),
		@complemento varchar(50),
		@bairro varchar(40),
		@cidade int,
		@cep varchar(9),
		@altitude varchar(50),
		@latitude varchar(50),
		@longitude varchar(50),
		@status int,
		@tipo varchar(8),
		@equipamento varchar(8),
		@referencia varchar(20),
		@CidadeNome varchar(150),
		@EstadoNome varchar(2))
AS 
BEGIN


	UPDATE
		"@ZPN_OPRJ"
	SET 
		U_Rua = @endereco,
		U_Numero = @Numero,
		U_Complemento = U_Complemento,
		U_Bairro = @bairro,
		U_CEP = @cep,
		U_Altitude = @altitude,
		U_Latitude = @latitude, 
		U_Longitude = @longitude,
		U_Situacao = @status,
		U_Tipo = @Tipo,
		U_Equip = @equipamento,
		U_CidadeDesc = @CidadeNome,
		U_Estado = @EstadoNome
	WHERE
	"Code" = @referencia;

end;