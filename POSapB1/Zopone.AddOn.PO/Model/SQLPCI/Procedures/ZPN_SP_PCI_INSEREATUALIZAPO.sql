create PROCEDURE ZPN_SP_PCI_INSEREATUALIZAPO (@DocEntry int)
AS
BEGIN

    BEGIN TRY

	
	



        DECLARE @UltimaData date;
        DECLARE @UltimaHora int;

        DECLARE @UltimaDataExec date;
        DECLARE @UltimaHoraExec int;

        set @UltimaDataExec = getdate();
        set @UltimaHoraExec = cast(replace(CONVERT(varchar(8), getdate(), 108), ':', '') as int);


        SELECT @UltimaData = ISNULL(MAX(DataExecutado), '2024-01-01'),
               @UltimaHora = ISNULL(MAX(HoraExecutado), 0)
        FROM ZPN_INTEGRAPCI
        WHERE ObjType = '17';


        IF OBJECT_ID('tempdb..#TempPO') IS NOT NULL
        BEGIN
            DROP TABLE #TempPO;
        END

        IF OBJECT_ID('tempdb..#POItensTemp') IS NOT NULL
        BEGIN
            DROP TABLE #POItensTemp;
        END


        CREATE TABLE #TempPO
		(
			poid varchar(150),
			gestatus INT,
			gedataacao DATETIME,
			empresaid varchar(150),
			descricao NVARCHAR(MAX),
			pedido NVARCHAR(MAX),
			valor DECIMAL(18, 2),
			data DATETIME,
			contratocliente NVARCHAR(MAX),
			codigo int
		);

        CREATE TABLE #POItensTemp
        (
            poitemid varchar(150),
            gestatus INT,
            gedataacao DATETIME,
            gecontaidacao VARCHAR(50),
            item NVARCHAR(MAX),
            poid varchar(150),
            obraid varchar(150),
            percentualparcela DECIMAL(18, 2),
            datalancamento DATETIME,
            datafaturamento DATETIME,
            numeronotafiscal NVARCHAR(MAX),
            valor DECIMAL(18, 2),
            tipo CHAR(1),
            clienteid varchar(150),
            etapaid varchar(150),
            observacao NVARCHAR(MAX),
            codigo int,
			LineNum int,
            descricao NVARCHAR(MAX),
            obracandidatoid varchar(150)
        );


        WITH MinLineNums
        AS (SELECT T0.DocEntry,
                   MIN(T0.LineNum) AS MinLineNum
            FROM RDR1 T0
            WHERE ISNULL(T0.AgrNo, 0) <> 0
            GROUP BY T0.DocEntry
           )

		    
        INSERT INTO #TempPO
		(
			poid,
            gestatus,
            gedataacao,
            empresaid,
            descricao,
            pedido,
            valor,
            data,
            contratocliente,
            codigo
		)
        SELECT 
			   isnull(ORDR.U_IdPCI,''),
               1,
               GETDATE(),
               OBPL.U_IdPCI,
               ORDR.Comments,
               ORDR.NumAtCard,
               ORDR.DocTotal,
               ORDR.DocDate,
               OOAT.Descript,
               ORDR.DocEntry
        FROM ORDR
            INNER JOIN OBPL
                ON OBPL.BPLId = ORDR.BPLId
            INNER JOIN MinLineNums
                ON MinLineNums.DocEntry = ORDR.DocEntry
            INNER JOIN RDR1
                ON RDR1.DocEntry = ORDR.DocEntry
                   AND RDR1.LineNum = MinLineNums.MinLineNum
            INNER JOIN OOAT
                ON OOAT.AbsID = RDR1.AgrNo
        WHERE 
              (
                      (
                          ORDR.CreateDate >= @UltimaData
                          AND ORDR.CreateTs >= @UltimaHora
                          AND ISNULL(@DocEntry, 0) = 0
                      )
                      OR (
                             ORDR.DocEntry = @DocEntry
                             OR ISNULL(@DocEntry, 0) = 0
                         )
                  )
              AND OBPL.U_IdPCI IS NOT NULL;


        -- Variáveis para armazenar os dados
        DECLARE @poid varchar(150);
        DECLARE @gestatus INT;
        DECLARE @gedataacao DATETIME;
        DECLARE @gecontaidacao VARCHAR(50);
        DECLARE @empresaid varchar(150);
        DECLARE @descricao NVARCHAR(MAX);
        DECLARE @pedido NVARCHAR(MAX);
        DECLARE @valor DECIMAL(18, 2);
        DECLARE @data DATETIME;
        DECLARE @contratocliente NVARCHAR(MAX);
        DECLARE @codigo int;

            DECLARE @poitemid varchar(150);



            DECLARE @item NVARCHAR(MAX);

            DECLARE @obraid varchar(150);
            DECLARE @percentualparcela DECIMAL(18, 2);
            DECLARE @datalancamento DATETIME;
            DECLARE @datafaturamento DATETIME;
            DECLARE @numeronotafiscal NVARCHAR(MAX);

            DECLARE @tipo CHAR(1);
            DECLARE @clienteid varchar(150);
            DECLARE @etapaid varchar(150);
            DECLARE @observacao NVARCHAR(MAX);
		    declare @LineNum int;

            DECLARE @obracandidatoid varchar(150);

        -- Contar o número de registros
        DECLARE @CountPO INT;

        DECLARE @Count INT;
        
        
        SELECT @CountPO = COUNT(*)
        FROM #TempPO;

        -- Inicializar o índice
        DECLARE @Index INT;
        DECLARE @IndexPO INT;
        SET @IndexPO = 1;

        -- Loop para processar os registros
        WHILE @IndexPO <= @CountPO
        BEGIN
            -- Buscar os dados da tabela temporária para o índice atual
            SELECT @poid = poid,
                   @gestatus = gestatus,
                   @gedataacao = gedataacao,
                   @empresaid = empresaid,
                   @descricao = descricao,
                   @pedido = pedido,
                   @valor = valor,
                   @data = data,
                   @contratocliente = contratocliente,
                   @codigo = codigo
            FROM
            (
                SELECT poid,
                       gestatus,
                       gedataacao,
                       empresaid,
                       descricao,
                       pedido,
                       valor,
                       data,
                       contratocliente,
                       codigo,
                       ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
                FROM #TempPO
            ) AS Temp
            WHERE RowNum = @IndexPO;

			if (@poid = '') begin
				set @poid = newid();
			end;

            exec [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaPO @poid,
                                                                               @gestatus,
                                                                               @gedataacao,
                                                                               @empresaid,
                                                                               @descricao,
                                                                               @pedido,
                                                                               @valor,
                                                                               @data,
                                                                               @contratocliente,
                                                                               @codigo;

          



        INSERT INTO #POItensTemp
        SELECT isnull(RDR1.U_IdPCI,''),
               1,
               GETDATE(),
               NULL,
               RDR1.U_Item,
               ORDR.U_IdPCI,
               OBRA.U_IdPCI,
               0,
               ORDR.DocDate,
               RDR1.U_DataFat,
               NULL,
               RDR1.LineTotal,
               CASE
                   WHEN RDR1.U_Tipo = 'Item' THEN
                       'I'
                   ELSE
                       'S'
               END,
               CRD8.U_IdPCI,
               ALOC.U_IdPCI,
               RDR1.U_itemDescription,
               RDR1.DocEntry,
			   RDR1.LineNum,
               NULL,
               CAND.U_IdPCI
        FROM RDR1
            INNER JOIN ORDR
                ON ORDR.DocEntry = RDR1.DocEntry
            INNER JOIN CRD8
                ON CRD8.CardCode = ORDR.CardCode
                   AND CRD8.BPLId = ORDR.BPLId
            INNER JOIN "@ZPN_OPRJ" OBRA
                ON OBRA.Code = RDR1.Project
            LEFT JOIN "@ZPN_ALOCA" ALOC
                ON ALOC.Code = RDR1.U_ItemFat
            LEFT JOIN "@ZPN_OPRJ_CAND" CAND
                ON CAND.Code = RDR1.U_Candidato
        WHERE 
              (
                      (
                          ORDR.CreateDate >= @UltimaData
                          AND ORDR.CreateTs >= @UltimaHora
                          AND ISNULL(@DocEntry, 0) = 0
                      )
                      OR (
                             ORDR.DocEntry = @DocEntry
                             OR ISNULL(@DocEntry, 0) = 0
                         )
                  );

            -- Variáveis para armazenar os dados
            SET @poitemid = ''



            SET @item = ''

            SET @obraid = ''
            SET @percentualparcela =0;
            SET @datalancamento = NULL;
            SET @datafaturamento = NULL;
            SET @numeronotafiscal = '';

            SET @tipo= '';
            SET @clienteid = '';
            SET @etapaid = '';
            SET @observacao = '';
		    SET @LineNum =0;

            SET @obracandidatoid = '';

            -- Contar o número de registros

            SELECT @Count = COUNT(*)
            FROM #POItensTemp;

            -- Inicializar o índice

            SET @Index = 1;

            -- Loop para processar os registros
            WHILE @Index <= @Count
            BEGIN
                -- Buscar os dados da tabela temporária para o índice atual
                SELECT @poitemid = poitemid,
                       @gestatus = gestatus,
                       @gedataacao = gedataacao,
                       @gecontaidacao = gecontaidacao,
                       @item = item,
                       @obraid = obraid,
                       @percentualparcela = percentualparcela,
                       @datalancamento = datalancamento,
                       @datafaturamento = datafaturamento,
                       @numeronotafiscal = numeronotafiscal,
                       @valor = valor,
                       @tipo = tipo,
                       @clienteid = clienteid,
                       @etapaid = etapaid,
                       @observacao = observacao,
                       @codigo = codigo,
				       @LineNum = LineNum,
                       @descricao = descricao,
                       @obracandidatoid = obracandidatoid
                FROM
                (
                    SELECT poitemid,
                           gestatus,
                           gedataacao,
                           gecontaidacao,
                           item,
                           obraid,
                           percentualparcela,
                           datalancamento,
                           datafaturamento,
                           numeronotafiscal,
                           valor,
                           tipo,
                           clienteid,
                           etapaid,
                           observacao,
                           codigo,
					       LineNum,
                           descricao,
                           obracandidatoid,
                           ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
                    FROM #POItensTemp
                ) AS Temp
                WHERE RowNum = @Index;

			    if (@poitemid = '')
			    begin
				    set @poitemid = NEWID();
			    end;
			   

                -- Inserir os dados na tabela final
                exec [LINKZCLOUD].[zsistema_aceite].[dbo].ZPN_PCI_InsereAtualizaPOItem @poitemid,
                                                                                       @gestatus,
                                                                                       @gedataacao,
                                                                                       @item,
                                                                                       @poid,
                                                                                       @obraid,
                                                                                       @percentualparcela,
                                                                                       @datalancamento,
                                                                                       @datafaturamento,
                                                                                       @numeronotafiscal,
                                                                                       @valor,
                                                                                       @tipo,
                                                                                       @clienteid,
                                                                                       @etapaid,
                                                                                       @observacao,
                                                                                       @codigo,
                                                                                       @descricao,
                                                                                       @obracandidatoid;

			    UPDATE RDR1 SET U_IdPCI = @poitemid where DocEntry = @codigo and LineNum = @LineNum;


                -- Incrementar o índice
                SET @Index = @Index + 1;
            END

          update ordr set U_IdPCI = @poid where DocEntry = @codigo;


            -- Incrementar o índice
            SET @IndexPO = @IndexPO + 1;
        END

                -- Excluir a tabela temporária
        DROP TABLE #TempPO;

        -- Excluir a tabela temporária
        DROP TABLE #POItensTemp;



        IF (isnull(@DocEntry, 0) = 0)
        BEGIN
            INSERT INTO ZPN_INTEGRAPCI
            (
                ObjType,
                DataExecutado,
                HoraExecutado
            )
            VALUES
            ('17', @UltimaDataExec, @UltimaHoraExec);

        END;

    END TRY
    BEGIN CATCH
        -- Captura do erro
        DECLARE @ErrorNumber INT = ERROR_NUMBER(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE(),
                @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE(),
                @ErrorLine INT = ERROR_LINE(),
                @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();

        -- Inserir o log de erro na tabela ErrorLog
        INSERT INTO ZPN_LogImportacaoPCI
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            HostName,
            ApplicationName,
            UserName
        )
        VALUES
        (@ErrorNumber,
         @ErrorSeverity,
         @ErrorState,
         ISNULL(@ErrorProcedure, 'ZPN_SP_PCI_INSEREATUALIZAPO'),
         @ErrorLine,
         @ErrorMessage,
         HOST_NAME(),
         APP_NAME(),
         SYSTEM_USER
        );

    -- Opcional: Re-lançar o erro se necessário
    -- THROW; 

    END CATCH;









end;



