CREATE PROCEDURE ZPN_SP_ATUALIZAOBRACANDIDATOSAP 
(
    @referencia varchar(20),
    @codigo int,
    @identificacao varchar(3),
    @tipo varchar(10),
    @nome varchar(30),
    @detentora varchar(20),
    @iddetentora varchar(20),
    @endereco varchar(50),
    @numero varchar(10),
    @complemento varchar(20),
    @bairro varchar(50),
    @cidade	int,
    @cep varchar(9),
    @latitude varchar(50),
    @longitude varchar(50),
    @altitude varchar(50),
    @equipamento varchar(8),
    @status int,
	@CidadeNome varchar(150),
	@EstadoNome varchar(2),
	@obracandidatoid varchar(250)
)
AS 
BEGIN
	declare @count int;

	IF @status = 1 
	BEGIN
		SELECT 
			@count = count(1) 
		FROM 
			[@ZPN_OPRJ_CAND]  T0 
		where 
			"Code" = @referencia 
			and U_Identif = @identificacao;

		if (@count = 0) 
		begin
			INSERT INTO [@ZPN_OPRJ_CAND] 
			(
				[Code], 
				LineId,
				[Object],
				[LogInst],
				[U_Identif], 
				[U_Tipo], 
				[U_Nome], 
				[U_Detentora], 
				[U_IdDetentora], 
				[U_Estado], 
				[U_CidadeDesc], 
				[U_Rua], 
				[U_Numero], 
				[U_Complemento], 
				[U_CEP], 
				[U_Bairro], 
				[U_Latitude], 
				[U_Longitude], 
				[U_Altitude], 
				[U_Codigo], 
				[U_Equip]
			)
		VALUES 
			(
				@referencia, 
				1 + ISNULL((SELECT MAX("LineId") FROM [@ZPN_OPRJ_CAND] WHERE "Code" = @referencia), 0), 
				'ZPN_OPRJ',
				null,
				@identificacao, 
				@tipo, 
				@nome, 
				@detentora, 
				@iddetentora, 
				@EstadoNome, 
				@CidadeNome, 
				@endereco, 
				@numero, 
				@complemento,
				@cep, 
				@cidade, 
				@latitude, 
				@longitude, 
				@altitude, 
				@codigo, 
				@equipamento
			);

		end;
		else
		begin
			UPDATE [@ZPN_OPRJ_CAND]
			   SET [Code] = @referencia
				  ,[U_Identif] = @identificacao
				  ,[U_Tipo] = @tipo
				  ,[U_Nome] = @nome
				  ,[U_Detentora] = @detentora
				  ,[U_IdDetentora] = @iddetentora
				  ,[U_Estado] = @EstadoNome
				  ,[U_CidadeDesc] = @EstadoNome
				  ,[U_Rua] = @endereco
				  ,[U_Numero] = @numero
				  ,[U_Complemento] = @complemento
				  ,[U_CEP] = @cep
				  ,[U_Bairro] = @cidade
				  ,[U_Latitude] = @latitude
				  ,[U_Longitude] = @longitude
				  ,[U_Altitude] = @altitude
				  ,[U_Codigo] = @codigo
				  ,[U_Equip] = @equipamento
				  ,U_IdPCI = @obracandidatoid
			 WHERE 
				[Code] = @referencia and [U_Identif] = @identificacao;
		end;


	END
	ELSE 
	BEGIN
		delete from [@ZPN_OPRJ_CAND] where [Code] = @referencia and [U_Identif] = @identificacao;
	END;




END;