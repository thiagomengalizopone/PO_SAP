

CREATE PROCEDURE ZPN_PCI_InsereAtualizaCliente
    @clienteid varchar(250),      
    @gestatus INT,                    
    @gedataacao DATETIME,             
    @empresaid  varchar(250),                   
    @razaosocial VARCHAR(255),        
    @nomefantasia VARCHAR(255),       
    @documentoprincipal VARCHAR(50),  
    @documentoadicional VARCHAR(50),  
    @inicioatividade DATETIME,        
    @codigo INT                       
AS
BEGIN
    -- Verificar se o cliente já existe (se o clienteid já está presente)
    IF EXISTS (SELECT 1 FROM [cliente] WHERE [clienteid] = @clienteid)
    BEGIN
        -- Se o cliente existir, realizar o UPDATE
        UPDATE [cliente]
        SET
            [gestatus] = @gestatus,
            [gedataacao] = @gedataacao,
            [empresaid] = @empresaid,
            [razaosocial] = @razaosocial,
            [nomefantasia] = @nomefantasia,
            [documentoprincipal] = @documentoprincipal,
            [documentoadicional] = @documentoadicional,
            [inicioatividade] = @inicioatividade,
            [codigo] = @codigo
        WHERE [clienteid] = @clienteid;
    END
    ELSE
    BEGIN
        -- Se o cliente não existir, realizar o INSERT
        INSERT INTO 
            [cliente]
            (
                [clienteid],
                [gestatus],
                [gedataacao],
                [empresaid],
                [razaosocial],
                [nomefantasia],
                [documentoprincipal],
                [documentoadicional],
                [inicioatividade],
                [codigo]
            )
        VALUES
            (
                @clienteid,           
                @gestatus,            
                @gedataacao,          
                @empresaid,           
                @razaosocial,         
                @nomefantasia,        
                @documentoprincipal,  
                @documentoadicional,  
                @inicioatividade,     
                @codigo               
            );
    END
END;
