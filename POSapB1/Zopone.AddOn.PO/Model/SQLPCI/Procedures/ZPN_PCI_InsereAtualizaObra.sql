
CREATE PROCEDURE [dbo].[ZPN_PCI_InsereAtualizaObra]
    @obraid VARCHAR(255),
    @gestatus INT,
    @gedataacao DATETIME,
    @gecontaidacao VARCHAR(255),
    @obraclassificacaoid VARCHAR(255),
    @referencia VARCHAR(255),
    @longitude  VARCHAR(255),
    @bairro VARCHAR(100),
    @realizadotermino DATETIME,
    @realizadoinicio DATETIME,
    @altitude VARCHAR(255),
    @cep VARCHAR(10),
    @complemento VARCHAR(255),
    @contratoid UNIQUEIDENTIFIER,
    @datacadastro DATETIME,
    @endereco VARCHAR(255),
    @filialid UNIQUEIDENTIFIER,
    @latitude VARCHAR(255),
    @localizacao VARCHAR(255),
    @numero VARCHAR(50),
    @previsaoinicio DATETIME,
    @previsaotermino DATETIME,
    @visualizarpci INT,
    @detentora VARCHAR(100),
    @equipamento VARCHAR(100),
    @historicoavaliacoes VARCHAR(MAX),
    @iddetentora VARCHAR(50),
    @idsite VARCHAR(50),
    @tipo VARCHAR(50),
    @situacao VARCHAR(50),
    @cidade VARCHAR(100),
    @estado VARCHAR(50),
    @dataatualizacao DATETIME,
    @situacaopci VARCHAR(50),
    @observacaomontagemform VARCHAR(MAX),
    @gedarquivoid VARCHAR(250),
    @gedpastaid VARCHAR(250)
AS
BEGIN
    DECLARE @count INT;

    SET @count = (SELECT COUNT(1) FROM [dbo].[obra] WHERE obraid = @obraid);

    IF (@count = 0) 
    BEGIN
        -- Inserir dados na tabela final
        INSERT INTO [dbo].[obra]
        (
            [obraid],
            [gestatus],
            [gedataacao],
            [gecontaidacao],
            [obraclassificacaoid],
            [referencia],
            [longitude],
            [bairro],
            [realizadotermino],
            [realizadoinicio],
            [altitude],
            [cep],
            [complemento],
            [contratoid],
            [datacadastro],
            [endereco],
            [filialid],
            [latitude],
            [localizacao],
            [numero],
            [previsaoinicio],
            [previsaotermino],
            [visualizarpci],
            [detentora],
            [equipamento],
            [historicoavaliacoes],
            [iddetentora],
            [idsite],
            [tipo],
            [situacao],
            [cidade],
            [estado],
            [dataatualizacao],
            [situacaopci],
            [observacaomontagemform],
            [gedarquivoid],
            [gedpastaid]
        ) VALUES
        (
            @obraid,
            @gestatus,
            @gedataacao,
            @gecontaidacao,
            @obraclassificacaoid,
            @referencia,
            @longitude,
            @bairro,
            @realizadotermino,
            @realizadoinicio,
            @altitude,
            @cep,
            @complemento,
            @contratoid,
            @datacadastro,
            @endereco,
            @filialid,
            @latitude,
            @localizacao,
            @numero,
            @previsaoinicio,
            @previsaotermino,
            @visualizarpci,
            @detentora,
            @equipamento,
            @historicoavaliacoes,
            @iddetentora,
            @idsite,
            @tipo,
            @situacao,
            @cidade,
            @estado,
            @dataatualizacao,
            @situacaopci,
            @observacaomontagemform,
            @gedarquivoid,
            @gedpastaid
        );
    END
    ELSE
    BEGIN
        UPDATE [dbo].[obra]  -- Corrigido para a tabela correta
        SET
            [gedataacao] = GETDATE(),
            [gecontaidacao] = NULL,
            [obraclassificacaoid] = @obraclassificacaoid,
            [referencia] = @referencia,
            [longitude] = @longitude,
            [bairro] = @bairro,  -- Corrigido para usar @bairro
            [realizadotermino] = @realizadotermino,  -- Corrigido para usar @realizadotermino
            [realizadoinicio] = @realizadoinicio,  -- Corrigido para usar @realizadoinicio
            [altitude] = @altitude,  -- Corrigido para usar @altitude
            [cep] = @cep,  -- Corrigido para usar @cep
            [complemento] = @complemento,  -- Corrigido para usar @complemento
            [contratoid] = @contratoid,
            [endereco] = @endereco,  -- Corrigido para usar @endereco
            [filialid] = @filialid,  -- Corrigido para usar @filialid
            [latitude] = @latitude,  -- Corrigido para usar @latitude
            [localizacao] = @localizacao,  -- Corrigido para usar @localizacao
            [numero] = @numero,  -- Corrigido para usar @numero
            [previsaoinicio] = @previsaoinicio,  -- Corrigido para usar @previsaoinicio
            [previsaotermino] = @previsaotermino,  -- Corrigido para usar @previsaotermino
            [visualizarpci] = @visualizarpci,
            [detentora] = @detentora,
            [equipamento] = @equipamento,
            [historicoavaliacoes] = @historicoavaliacoes,
            [iddetentora] = @iddetentora,
            [idsite] = @idsite,
            [tipo] = @tipo,
            [situacao] = @situacao,
            [cidade] = @cidade,
            [estado] = @estado,
            [dataatualizacao] = @dataatualizacao,
            [situacaopci] = @situacaopci,  -- Corrigido para usar @situacaopci
            [observacaomontagemform] = @observacaomontagemform,
            [gedarquivoid] = @gedarquivoid,
            [gedpastaid] = @gedpastaid
        WHERE
            [obraid] = @obraid;
    END
END
