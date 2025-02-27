
CREATE PROCEDURE [dbo].[ZPN_SP_PCI_ATUALIZAOBRA]
(
    @CodeObra VARCHAR(30),
	@CreateDate date
)
AS
BEGIN

BEGIN TRY

    DECLARE 
        @obraid VARCHAR(255),
        @gestatus INT,
        @gedataacao DATETIME,
        @gecontaidacao NVARCHAR(255),
        @obraclassificacaoid NVARCHAR(255),
        @referencia NVARCHAR(255),
        @longitude NVARCHAR(255),
        @bairro NVARCHAR(100),
        @realizadotermino DATETIME,
        @realizadoinicio DATETIME,
        @altitude NVARCHAR(255),
        @cep VARCHAR(10),
        @complemento NVARCHAR(255),
        @contratoid Nvarchar(250),
        @datacadastro DATETIME,
        @endereco NVARCHAR(255),
        @filialid Nvarchar(250),
        @latitude NVARCHAR(255),
        @localizacao NVARCHAR(255),
        @numero NVARCHAR(50),
        @previsaoinicio DATETIME,
        @previsaotermino DATETIME,
        @visualizarpci INT,
        @detentora NVARCHAR(100),
        @equipamento NVARCHAR(100),
        @historicoavaliacoes NVARCHAR(MAX),
        @iddetentora NVARCHAR(50),
        @idsite NVARCHAR(50),
        @tipo NVARCHAR(50),
        @situacao NVARCHAR(50),
        @cidade NVARCHAR(100),
        @estado NVARCHAR(50),
        @dataatualizacao DATETIME,
        @situacaopci NVARCHAR(50),
        @observacaomontagemform NVARCHAR(MAX),
        @gedarquivoid NVARCHAR(250),
        @gedpastaid NVARCHAR(250),
		@codigo int;

	DECLARE @erro_obra  NVARCHAR(max) = '';

    -- Cria a tabela temporária
    DECLARE @TempObra TABLE 
    (
		rownumber int,
        obraid VARCHAR(255),
        gestatus INT,
        gedataacao DATETIME,
        gecontaidacao NVARCHAR(255),
        obraclassificacaoid NVARCHAR(255),
        referencia NVARCHAR(255),
        longitude NVARCHAR(255),
        bairro NVARCHAR(100),
        realizadotermino DATETIME,
        realizadoinicio DATETIME,
        altitude NVARCHAR(255),
        cep NVARCHAR(10),
        complemento NVARCHAR(255),
        contratoid NVARCHAR(255),
        datacadastro DATETIME,
        endereco NVARCHAR(255),
        filialid NVARCHAR(255),
        latitude NVARCHAR(255),
        localizacao NVARCHAR(255),
        numero NVARCHAR(50),
        previsaoinicio DATETIME,
        previsaotermino DATETIME,
        visualizarpci INT,
        detentora NVARCHAR(100),
        equipamento NVARCHAR(100),
        historicoavaliacoes TEXT,
        iddetentora NVARCHAR(255),
        idsite NVARCHAR(255),
        tipo NVARCHAR(50),
        situacao NVARCHAR(50),
        cidade NVARCHAR(100),
        estado NVARCHAR(50),
        dataatualizacao DATETIME,
        situacaopci NVARCHAR(255),
        observacaomontagemform TEXT,
        gedarquivoid NVARCHAR(255),
        gedpastaid NVARCHAR(255),
		codigo int
    );

    -- Inserir dados na tabela temporária
    INSERT INTO @TempObra
    SELECT
		ROW_NUMBER() over (order by OBRA.DocEntry),
        ISNULL(OBRA.U_IdPCI,''),                 -- obraid
        1,                                       -- gestatus
        GETDATE(),                               -- gedataacao
        NULL,                                    -- gecontaidacao
        CLASS.U_IdPCI,                           -- obraclassificacaoid
        OBRA.Code,                               -- referencia
        OBRA.U_Longitude,                        -- longitude
        OBRA.U_Bairro,                           -- bairro
        OBRA.U_RelTerm,                          -- realizadotermino
        OBRA.U_RelIni,                           -- realizadoinicio
        OBRA.U_Altitude,                         -- altitude
        OBRA.U_CEP,                              -- cep
        OBRA.U_Complemento,                      -- complemento
        OOAT.U_IdPCI,                            -- contratoid
        GETDATE(),                               -- datacadastro
        ISNULL(OBRA.U_TipoLog, '') + ' ' + ISNULL(OBRA.U_Rua, ''),  -- endereco
        OPRC.U_IdPCI,                            -- filialid
        OBRA.U_Latitude,                         -- latitude
        OBRA.Name,                           -- localizacao
        OBRA.U_Numero,                           -- numero
        OBRA.U_PrevIni,                          -- previsaoinicio
        OBRA.U_PrevTerm,                         -- previsaotermino
        CASE WHEN ISNULL(OBRA.U_VisPCI, '') = 'Y' THEN 1 ELSE 0 END, -- visualizarpci
        OBRA.U_Detent,                           -- detentora
        OBRA.U_Equip,                            -- equipamento
        OBRA.U_Obs,                              -- historicoavaliacoes
        OBRA.U_IdDetent,                         -- iddetentora
        OBRA.U_IdSite,                           -- idsite
        OBRA.U_Tipo,                             -- tipo
        ISNULL(OBRA.U_Situacao,''),              -- situacao
        obra.U_CidadeDesc collate SQL_Latin1_General_CP1_CI_AS,                             -- cidade
        OBRA.U_Estado,                              -- estado
        OBRA.UpdateDate,                         -- dataatualizacao
        0,                                       -- situacaopci
        NULL,                                    -- observacaomontagemform
        NULL,                                    -- gedarquivoid
        NULL,                                     -- gedpastaid,
		OBRA.U_CodMigrado
    FROM     
        "@ZPN_OPRJ" OBRA
        INNER JOIN OOAT ON OOAT.AbsID = OBRA.U_CodContrato
        LEFT JOIN "@ZPN_CLASSOBF" CLASS ON CLASS.Code = OBRA.U_ClassOb AND CLASS.U_BPLId = OBRA.U_BPLId
        LEFT JOIN OPRC ON OPRC.PrcCode = OBRA.U_Regional 
        LEFT JOIN OCNT ON OCNT.[Code] = OBRA.U_Cidade
    WHERE
        (OBRA.Code = @CodeObra) or
		(
			ISNULL(@CodeObra, '') = '' AND 
			OBRA.CreateDate = @CreateDate
		)
	order by OBRA.DocEntry;      

    -- Loop para inserir os dados da tabela temporária na tabela final
    DECLARE @Counter INT = 1;
    DECLARE @TotalRows INT = (SELECT COUNT(*) FROM @TempObra);
    declare @Row_Number int;

    WHILE @Counter <= @TotalRows
    BEGIN
        SELECT TOP 1
            @obraid = obraid,
            @gestatus = gestatus,
            @gedataacao = gedataacao,
            @gecontaidacao = gecontaidacao,
            @obraclassificacaoid = obraclassificacaoid,
            @referencia = referencia,
            @longitude = longitude,
            @bairro = bairro,
            @realizadotermino = realizadotermino,
            @realizadoinicio = realizadoinicio,
            @altitude = altitude,
            @cep = cep,
            @complemento = complemento,
            @contratoid = contratoid,
            @datacadastro = datacadastro,
            @endereco = endereco,
            @filialid = filialid,
            @latitude = latitude,
            @localizacao = localizacao,
            @numero = numero,
            @previsaoinicio = previsaoinicio,
            @previsaotermino = previsaotermino,
            @visualizarpci = visualizarpci,
            @detentora = detentora,
            @equipamento = equipamento,
            @historicoavaliacoes = historicoavaliacoes,
            @iddetentora = iddetentora,
            @idsite = idsite,
            @tipo = tipo,
            @situacao = situacao,
            @cidade = cidade,
            @estado = estado,
            @dataatualizacao = dataatualizacao,
            @situacaopci = situacaopci,
            @observacaomontagemform = observacaomontagemform,
            @gedarquivoid = gedarquivoid,
            @gedpastaid = gedpastaid,
			@codigo = codigo
        FROM @TempObra
        WHERE rownumber = @Counter;

		if (ISNULL(@obraid,'') = '') begin

       		select @obraid = ISNULL(max(cast(obraid as varchar(250))),'') from [LINKZCLOUD].[zsistema_producao].[dbo].obra where [referencia] = @referencia;

            if (@obraid = '') begin
			    set @obraid = newid();
            end;
		end;
			-- Chama a procedure para inserir/atualizar a obra
			EXEC [LINKZCLOUD].[zsistema_producao].[dbo].[ZPN_PCI_InsereAtualizaObra]
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
				@cidade ,
				@estado,
				@dataatualizacao,
				@situacaopci,
				@observacaomontagemform,
				@gedarquivoid,
				@gedpastaid, 
				@codigo;

			UPDATE "@ZPN_OPRJ" SET U_IdPCI = @obraid WHERE "Code" = @referencia;

        SET @Counter = @Counter + 1;
    END;    

	select @erro_obra "erro";

    	  END TRY
BEGIN CATCH
   -- Captura do erro
    DECLARE 
        @ErrorNumber INT              = ERROR_NUMBER(),
        @ErrorSeverity INT            = ERROR_SEVERITY(),
        @ErrorState INT               = ERROR_STATE(),
        @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE(),
        @ErrorLine INT                = ERROR_LINE(),
        @ErrorMessage NVARCHAR(4000)  = ERROR_MESSAGE();

    -- Inserir o log de erro na tabela ErrorLog
    INSERT INTO ZPN_LogImportacaoPCI (ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, HostName, ApplicationName, UserName)
    VALUES (@ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage, HOST_NAME(), APP_NAME(), SYSTEM_USER);
    
    select @erro_obra = @erro_obra + ' Erro ao enviar Obra ' + @referencia + ' '  + ERROR_MESSAGE() + '\n';
    
    -- Opcional: Re-lançar o erro se necessário
    -- THROW; 

END CATCH;

END;