create PROCEDURE [dbo].[ZPN_PCI_InsereAtualizaContasReceber]
(
    @contareceberid VARCHAR(150),
    @gestatus INT,
    @gecontaidacao VARCHAR(50),
    @empresaid VARCHAR(150),
    @obraid VARCHAR(150),
    @valor DECIMAL(18, 2),
    @valorliquido DECIMAL(18, 2),
    @valorpis DECIMAL(18, 2),
    @valorcofins DECIMAL(18, 2),
    @valorcsll DECIMAL(18, 2),
    @valorinss DECIMAL(18, 2),
    @valorirrf DECIMAL(18, 2),
    @valoriss DECIMAL(18, 2),
    @emissao DATETIME,
    @vencimento DATETIME,
    @programacao DATETIME,
    @recebimento DATETIME,
    @cancelamento DATETIME,
    @codigo VARCHAR(50),
    @fatura VARCHAR(50),
    @etapaid VARCHAR(150),
    @obracandidatoid VARCHAR(150)
 )
AS
BEGIN
    
    declare @count int;

    set @count = (select count(1) from [contareceber] where [contareceberid] = @contareceberid);

    if (@count = 0) begin 
    
        INSERT INTO [contareceber]
        (
            [contareceberid],
            [gestatus],
            [gedataacao],
            [gecontaidacao],
            [empresaid],
            [obraid],
            [valor],
            [valorliquido],
            [valorpis],
            [valorcofins],
            [valorcsll],
            [valorinss],
            [valorirrf],
            [valoriss],
            [emissao],
            [vencimento],
            [programacao],
            [recebimento],
            [cancelamento],
            [codigo],
            [fatura],
            [etapaid],
            [obracandidatoid]
        )
        VALUES
        (
            @contareceberid,            -- contareceberid
            @gestatus,                  -- gestatus
            GETDATE(),                  -- gedataacao (data e hora atual)
            @gecontaidacao,             -- gecontaidacao
            @empresaid,                 -- empresaid
            @obraid,                    -- obraid
            @valor,                     -- valor
            @valorliquido,              -- valorliquido
            @valorpis,                  -- valorpis
            @valorcofins,               -- valorcofins
            @valorcsll,                 -- valorcsll
            @valorinss,                 -- valorinss
            @valorirrf,                 -- valorirrf
            @valoriss,                  -- valoriss
            @emissao,                   -- emissao
            @vencimento,                -- vencimento
            @programacao,               -- programacao
            @recebimento,               -- recebimento
            @cancelamento,              -- cancelamento
            @codigo,                    -- codigo
            @fatura,                    -- fatura
            @etapaid,                   -- etapaid
            @obracandidatoid            -- obracandidatoid
        );
    end;
    else
    begin
        UPDATE [contareceber]
        SET
            [gestatus] = @gestatus,
            [gedataacao] = GETDATE(),              -- Atualiza a data de ação para a data atual
            [gecontaidacao] = @gecontaidacao,
            [empresaid] = @empresaid,
            [obraid] = @obraid,
            [valor] = @valor,
            [valorliquido] = @valorliquido,
            [valorpis] = @valorpis,
            [valorcofins] = @valorcofins,
            [valorcsll] = @valorcsll,
            [valorinss] = @valorinss,
            [valorirrf] = @valorirrf,
            [valoriss] = @valoriss,
            [emissao] = @emissao,
            [vencimento] = @vencimento,
            [programacao] = @programacao,
            [codigo] = @codigo,
            [fatura] = @fatura,
            [etapaid] = @etapaid,
            [obracandidatoid] = @obracandidatoid
        WHERE
            [contareceberid] = @contareceberid;

    end;



END;
