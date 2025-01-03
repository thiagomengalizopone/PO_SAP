﻿CREATE PROCEDURE ZPN_PCI_InsereAtualizaNfeservico
(
    @nfeservicoid varchar(250),
    @sequencia INT,
    @obraid varchar(250),
    @emissao DATETIME,
    @valor DECIMAL(18, 2),
    @situacao INT,
    @obracandidatoid varchar(250)
)
AS
BEGIN
		declare @count int;

		set @count = (select count(1) from [nfeservico] where nfeservicoid = @nfeservicoid);

        if (@count > 0) begin 
			UPDATE [nfeservico]
			SET
				gestatus = 1,
				gedataacao = GETDATE(),
				sequencia = @sequencia,
				obraid = @obraid,
				emissao = @emissao,
				valor = @valor,
				situacao = @situacao,
				obracandidatoid = @obracandidatoid
			WHERE nfeservicoid = @nfeservicoid;
		end;
		else
		begin
            INSERT INTO [nfeservico]
                ([nfeservicoid]
                ,[gestatus]
                ,[gedataacao]
                ,[sequencia]
                ,[obraid]
                ,[emissao]
                ,[valor]
                ,[situacao]
                ,[obracandidatoid])
            VALUES
                (@nfeservicoid
                ,1
                ,GETDATE()
                ,@sequencia
                ,@obraid
                ,@emissao
                ,@valor
                ,@situacao
                ,@obracandidatoid);
        END;
END;