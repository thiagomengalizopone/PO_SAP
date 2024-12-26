CREATE PROCEDURE SP_ZPN_CriaObservacoesFaturamentoEsboco
(
    @DocEntry INT
)
AS 
BEGIN
    -- Declaração de variáveis
    DECLARE @CardName VARCHAR(250);
    DECLARE @ItemCode VARCHAR(250);
    DECLARE @MENSAGEM VARCHAR(MAX);
    DECLARE @MENSAGEMIMPOSTO VARCHAR(MAX);

    -- Seleciona dados necessários para o processamento
    SELECT TOP 1
        @ItemCode = DRF1.ItemCode,
        @CardName = ODRF.CardName,
        @MENSAGEM = ISNULL(ODRF.Header, '')
    FROM 
        DRF1 
        INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
    WHERE
        ODRF.DocEntry = @DocEntry;

    -- Atualiza o campo U_Project em DRF6
    UPDATE DRF6
    SET DRF6.U_Project = DRF1.Project
    FROM
        DRF6
        INNER JOIN DRF1 ON DRF1.DocEntry = DRF6.DocEntry
    WHERE
        DRF1.DocEntry = @DocEntry;

    -- Verifica se o nome do cliente contém 'CLARO'
    IF (UPPER(@CardName) LIKE '%CLARO%')
    BEGIN
        -- Se @MENSAGEM estiver vazia, cria a mensagem inicial
        IF (@MENSAGEM = '') 
        BEGIN
            SELECT 
                @MENSAGEM = 
                'PEDIDO DE COMPRA: ' + ISNULL(ODRF.NumAtCard, '') + CHAR(13) + CHAR(10) +
                'OBRA: ' + ISNULL(DRF1.Project, '') + CHAR(13) + CHAR(10) +
                'NUMERO DE PROTOCOLO: ' + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +
                'CÓD. DA PREST. DE SERVS. ' + ISNULL(DRF1.ItemCode, '') + CHAR(13) + CHAR(10) +
                'SERVS. DE PROJETOS DE ENGENHARIA ' + CHAR(13) + CHAR(10) +
                'CONTRATO: ' + CHAR(13) + CHAR(10) +
                'SITE: ' + ISNULL(OBRA.U_IdSite, '') + ' ' + ISNULL(OBRA.U_CidadeDesc, '') + ' - ' + ISNULL(OBRA.U_Estado, '') + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +
                'EVENTO: 100% - TERMINO DOS SERVIÇOS ' + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +

                -- Chama a função FN_ZPN_RetornaImpostosClaro
                DBO.FN_ZPN_RetornaImpostosClaro(@DocEntry) 

               
            FROM 
                DRF1 
                INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
                INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
            WHERE
                DRF1.DocEntry = @DocEntry;
        END
        ELSE
        BEGIN
            -- Caso contrário, adiciona a parte de impostos à mensagem existente
            SET @MENSAGEMIMPOSTO = dbo.FN_ZPN_RetornaImpostosClaro(@DocEntry);

            -- Substitui a parte de impostos na mensagem
            SET @MENSAGEM = 
                LEFT(@MENSAGEM, CHARINDEX(' IRRF', @MENSAGEM) - 1) -- Tudo antes de IRRF
                + @MENSAGEMIMPOSTO; -- A nova parte de impostos
        END
    END;

	ELSE IF (UPPER(@CardName) LIKE '%WINITY%')
    BEGIN

	IF (@MENSAGEM = '') 
        BEGIN
            SELECT 
                @MENSAGEM = 
                'OBRA: ' + ISNULL(DRF1.Project, '') + CHAR(13) + CHAR(10) +
				CHAR(13) + CHAR(10) +
				'ID SITE: ' + ISNULL(OBRA.U_IdSite, '') + ' ' + ISNULL(OBRA.U_CidadeDesc, '') + ' - ' + ISNULL(OBRA.U_Estado, '') + CHAR(13) + CHAR(10) +
				'CÓD. DA PREST. DE SERVS. ' + ISNULL(DRF1.ItemCode, '') + CHAR(13) + CHAR(10) +
				'SERVIÇO EXECUTADO: ' + CHAR(13) + CHAR(10) +
				 CHAR(13) + CHAR(10) +
				 CHAR(13) + CHAR(10) +
                'PEDIDO DE COMPRA: ' + ISNULL(ODRF.NumAtCard, '') + '/' +  CAST(YEAR(ODRF.DocDate) as varchar(4)) + CHAR(13) + CHAR(10) +
				CHAR(13) + CHAR(10) +
				CHAR(13) + CHAR(10) +
                +
				dbo.FN_ZPN_RetornaImpostosWinity(@DocEntry)
            FROM 
                DRF1 
                INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
                INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
            WHERE
                DRF1.DocEntry = @DocEntry;
        END
        ELSE
        BEGIN
            -- Caso contrário, adiciona a parte de impostos à mensagem existente
            SET @MENSAGEMIMPOSTO = dbo.FN_ZPN_RetornaImpostosWinity(@DocEntry);

            -- Substitui a parte de impostos na mensagem
            SET @MENSAGEM = 
                LEFT(@MENSAGEM, CHARINDEX(' BASE DE', @MENSAGEM) - 1) -- Tudo antes de IRRF
                + @MENSAGEMIMPOSTO; -- A nova parte de impostos
        END
    END;

	ELSE IF (UPPER(@CardName) LIKE '%HUAWEI%')
    BEGIN
	SET @MENSAGEM = '';
	IF (@MENSAGEM = '') 
        BEGIN
            SELECT 
                @MENSAGEM = 
				ISNULL(OBRA.U_CidadeDesc, '') + ' - ' + ISNULL(OBRA.U_Estado, '') + CHAR(13) + CHAR(10) +
				'PEDIDO DE COMPRA: ' + ISNULL(ODRF.NumAtCard, '') + + CHAR(13) + CHAR(10) +
				'OBRA: ' + ISNULL(DRF1.Project, '') + CHAR(13) + CHAR(10) +
				CASE 
					WHEN DRF1.ItemCode = '7.02' THEN 'LINHA: 1' 
					ELSE 'LINHA: 2' 
				END +  + CHAR(13) + CHAR(10) +
				'PARCELA 1/1 - 100%: ' + CHAR(13) + CHAR(10) +
				'CONTRATO: ' + CHAR(13) + CHAR(10) +
				'SERVS. ' + 
						CASE 
							WHEN DRF1.ItemCode = '7.02' THEN 'INSTA' 
							ELSE 'PPI.' 
						END + 
				'CÓD. DA PREST. DE SERVS. ' + ISNULL(DRF1.ItemCode, '') + CHAR(13) + CHAR(10) +

				CHAR(13) + CHAR(10) +
				'SITE: ' + ISNULL(OBRA.U_IdSite, '') +
				CHAR(13) + CHAR(10) +

                
                +
				dbo.FN_ZPN_RetornaImpostoHuawei(@DocEntry)
            FROM 
                DRF1 
                INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
                INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
            WHERE
                DRF1.DocEntry = @DocEntry;
        END
        ELSE
        BEGIN
            -- Caso contrário, adiciona a parte de impostos à mensagem existente
            SET @MENSAGEMIMPOSTO = dbo.FN_ZPN_RetornaImpostoHuawei(@DocEntry);

            -- Substitui a parte de impostos na mensagem
            SET @MENSAGEM = 
                LEFT(@MENSAGEM, CHARINDEX(' BASE DE', @MENSAGEM) - 1) -- Tudo antes de IRRF
                + @MENSAGEMIMPOSTO; -- A nova parte de impostos
        END
    END;
    ELSE
    BEGIN
        -- Se @MENSAGEM estiver vazia, cria a mensagem inicial
        IF (@MENSAGEM = '') 
        BEGIN
            SELECT 
                @MENSAGEM = 
                'PEDIDO DE COMPRA: ' + ISNULL(ODRF.NumAtCard, '') + CHAR(13) + CHAR(10) +
                'OBRA: ' + ISNULL(DRF1.Project, '') + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +
                'CÓD. DA PREST. DE SERVS. ' + ISNULL(DRF1.ItemCode, '') + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +
                'SITE: ' + ISNULL(OBRA.U_IdSite, '') + ' ' + ISNULL(OBRA.U_CidadeDesc, '') + ' - ' + ISNULL(OBRA.U_Estado, '') + CHAR(13) + CHAR(10) +
                CHAR(13) + CHAR(10) +              
                -- Chama a função FN_ZPN_RetornaImpostosClaro
                dbo.FN_ZPN_RetornaImpostosNokia(@DocEntry) 


            FROM 
                DRF1 
                INNER JOIN ODRF ON ODRF.DocEntry = DRF1.DocEntry
                INNER JOIN "@ZPN_OPRJ" OBRA ON OBRA.Code = DRF1.Project
            WHERE
                DRF1.DocEntry = @DocEntry;
        END
        ELSE
        BEGIN
            -- Caso contrário, adiciona a parte de impostos à mensagem existente
            SET @MENSAGEMIMPOSTO = dbo.FN_ZPN_RetornaImpostosNokia(@DocEntry);

            -- Substitui a parte de impostos na mensagem
            SET @MENSAGEM = 
                LEFT(@MENSAGEM, CHARINDEX(' IRRF', @MENSAGEM) - 1) -- Tudo antes de IRRF
                + @MENSAGEMIMPOSTO; -- A nova parte de impostos
        END
    END;

    -- Atualiza a mensagem final no campo Header da tabela ODRF
    UPDATE ODRF 
    SET Header = @MENSAGEM 
    WHERE DocEntry = @DocEntry;
END;
