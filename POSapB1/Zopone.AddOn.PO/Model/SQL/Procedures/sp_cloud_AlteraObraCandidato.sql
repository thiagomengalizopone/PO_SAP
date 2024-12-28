
CREATE PROCEDURE [dbo].[sp_cloud_AlteraObraCandidato] (
	@emp_id int,
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
    @altitude float,
    @equipamento varchar(8),
    @status int
)
AS
    declare @banco varchar(200) = 'TOPENG'
    declare @params nvarchar(max)
    declare @sql nvarchar(max)
    declare @count int
    declare @EMP_001 int = (SELECT EMP_001 FROM Empresas WHERE emp_id = @emp_id)
    declare @OBR_001 int
    
    IF @emp_id = 8 
    BEGIN
        SET @banco = 'TOPCHILE'
    END
    
    SET @sql = N'SELECT @OBR_001 = OBR_001 FROM LINK.' + @banco + '.dbo.OBRAS
                WHERE EMP_001 = @EMP_001
                AND OBR_002 = @referencia'
    SET @params = N'
        @OBR_001 int OUT, 
        @EMP_001 int, 
        @referencia varchar(20)'
    exec sp_executesql @sql, @params, @OBR_001 out, @EMP_001, @referencia
    
    
    IF @status = 1 -- Inclusão/Alteração
    BEGIN
        SET @sql = N'SELECT @count = COUNT(1) FROM LINK.' + @banco + '.dbo.CANDIDATOS
                    WHERE EMP_001 = @EMP_001
                    AND OBR_001 = @OBR_001
                    AND CAN_001 = @codigo'
        SET @params = N'
            @count int out, 
            @EMP_001 int, 
            @OBR_001 int,
            @codigo int'
        exec sp_executesql @sql, @params, @count out, @EMP_001, @OBR_001, @codigo

        IF @count > 0 -- Alteração
        BEGIN
            SET @sql = N'
                UPDATE LINK.' + @banco + '.dbo.CANDIDATOS
                SET CAN_002 = @identificacao,
                    CAN_003 = @tipo,
                    CAN_004 = @nome,
                    CAN_005 = @detentora,
                    CAN_006 = @iddetentora,
                    CAN_007 = @endereco,
                    CAN_008 = @numero,
                    CAN_009 = @complemento,
                    CAN_010 = @bairro,
                    CID_001 = @cidade,
                    CAN_011 = @cep,
                    CAN_012 = @latitude,
                    CAN_013 = @longitude,
                    CAN_014 = @altitude,
                    CAN_016 = @equipamento
                WHERE EMP_001 = @EMP_001
                AND OBR_001 = @OBR_001
                AND CAN_001 = @codigo'

            SET @params=N'
                @identificacao varchar(3),
                @tipo varchar(10),
                @nome varchar(30),
                @detentora varchar(20),
                @iddetentora varchar(20),
                @endereco varchar(50),
                @numero  varchar(10),
                @complemento varchar(20),
                @bairro varchar(50),
                @cidade int,
                @cep varchar(9),
                @latitude varchar(50),
                @longitude varchar(50),
                @altitude float,
                @equipamento varchar(8),
                @EMP_001 int,
                @OBR_001 int,
                @codigo int'
            
            exec sp_executesql @sql, @params, @identificacao,
                                              @tipo, 
                                              @nome, 
                                              @detentora, 
                                              @iddetentora, 
                                              @endereco, 
                                              @numero, 
                                              @complemento, 
                                              @bairro, 
                                              @cidade, 
                                              @cep, 
                                              @latitude, 
                                              @longitude, 
                                              @altitude, 
                                              @equipamento, 
                                              @EMP_001,
                                              @OBR_001,
                                              @codigo
            
        END
        ELSE -- Inclusão
        BEGIN
            SET @sql = N'UPDATE LINK.' + @banco + '.dbo.SEQCANDIDATOS SET SEQ_001 = SEQ_001 + 1 WHERE EMP_001 = @EMP_001'
            SET @params = N'
                @EMP_001 int'
            exec sp_executesql @sql, @params, @EMP_001

            SET @sql = N'SELECT @codigo = SEQ_001 FROM LINK.' + @banco + '.dbo.SEQCANDIDATOS
                        WHERE EMP_001 = @EMP_001'
            SET @params = N'
                @codigo int OUT, 
                @EMP_001 int'
            exec sp_executesql @sql, @params, @codigo out, @EMP_001

            SET @sql = N'
                INSERT INTO LINK.' + @banco + '.dbo.CANDIDATOS
                    (EMP_001, OBR_001, CAN_001, CAN_002, CAN_003, CAN_004, CAN_005, CAN_006, CAN_007, CAN_008, CAN_009, CAN_010, CID_001, CAN_011, CAN_012, CAN_013, CAN_014, CAN_016)
                VALUES (
                    @EMP_001,
                    @OBR_001,
                    @codigo,
                    @identificacao,
                    @tipo,
                    @nome,
                    @detentora,
                    @iddetentora,
                    @endereco,
                    @numero,
                    @complemento,
                    @bairro,
                    @cidade,
                    @cep,
                    @latitude,
                    @longitude,
                    @altitude,
                    @equipamento
                )'
            
            SET @params=N'
                @EMP_001 int,
                @OBR_001 int,
                @codigo int,
                @identificacao varchar(3),
                @tipo varchar(10),
                @nome varchar(30),
                @detentora varchar(20),
                @iddetentora varchar(20),
                @endereco varchar(50),
                @numero  varchar(10),
                @complemento varchar(20),
                @bairro varchar(50),
                @cidade int,
                @cep varchar(9),
                @latitude varchar(50),
                @longitude varchar(50),
                @altitude float,
                @equipamento varchar(8)'
            
            exec sp_executesql @sql, @params, @EMP_001,
                                              @OBR_001,
                                              @codigo,
                                              @identificacao,
                                              @tipo, 
                                              @nome, 
                                              @detentora, 
                                              @iddetentora, 
                                              @endereco, 
                                              @numero, 
                                              @complemento, 
                                              @bairro, 
                                              @cidade, 
                                              @cep, 
                                              @latitude, 
                                              @longitude, 
                                              @altitude, 
                                              @equipamento
                                              
        END
    END
    ELSE --Exclusão
    BEGIN
        SET @sql = N'
                DELETE FROM LINK.' + @banco + '.dbo.CANDIDATOS
                WHERE EMP_001 = @EMP_001
                AND OBR_001 = @OBR_001
                AND CAN_001 = @codigo'

        SET @params=N'
                @EMP_001 int,
                @OBR_001 int,
                @codigo int'
        
        exec sp_executesql @sql, @params, @EMP_001,
                                          @OBR_001,
                                          @codigo
        
    END

    RETURN @codigo
