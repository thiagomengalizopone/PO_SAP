
CREATE PROCEDURE [dbo].[ZPN_PCI_InsereAtualizaNfeservico]
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
			UPDATE [dbo].[nfeservico]
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
            INSERT INTO [dbo].[nfeservico]
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
                (cast(@nfeservicoid as uniqueidentifier)
                ,1
                ,GETDATE()
                ,@sequencia
                ,cast(@obraid as  uniqueidentifier)
                ,@emissao
                ,@valor
                ,@situacao
                ,@obracandidatoid);
        END;


        delete from [nfeservicoparcela] where [nfeservicoid] = @nfeservicoid;
END;
GO


