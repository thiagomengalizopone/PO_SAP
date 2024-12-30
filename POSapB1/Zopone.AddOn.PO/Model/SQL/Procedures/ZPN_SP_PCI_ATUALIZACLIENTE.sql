CREATE PROCEDURE [ZPN_SP_PCI_ATUALIZACLIENTE]
(
	@CardCode varchar(50)
)
AS 
BEGIN

BEGIN TRY

--DECLARE @CardCode varchar(50);

		DECLARE @UltimaData date;
		DECLARE @UltimaHora int;
		DECLARE @UltimaDataExec DATE;
        DECLARE @UltimaHoraExec INT;

		declare @RowNumber int, @RowCount int;

		declare     @RowNum int,
					@clienteid varchar(250),    
					@CardCodePCI varchar(100),
					@BPLId int,
					@gestatus INT,                    
					@gedataacao DATETIME,             
					@empresaid  varchar(250),                   
					@razaosocial VARCHAR(255),        
					@nomefantasia VARCHAR(255),       
					@documentoprincipal VARCHAR(50),  
					@documentoadicional VARCHAR(50),  
					@inicioatividade DATETIME,        
					@codigo INT  ;


        -- Captura a data e hora da execução atual
        SET @UltimaDataExec = GETDATE();
        SET @UltimaHoraExec = CAST(REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') AS INT);

        -- Recupera as últimas execuções
        SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = 'OCRD');
        SET @UltimaHora = (SELECT ISNULL(MAX(HoraExecutado), 0) FROM ZPN_INTEGRAPCI WHERE ObjType ='OCRD');


DECLARE @ClientesPCI TABLE
(
    RowNumber INT IDENTITY(1,1),
    clienteid VARCHAR(250),     
	CardCodePCI varchar(100),
	BPLId int,
    gestatus INT,                    
    gedataacao DATETIME,             
    empresaid VARCHAR(250),                   
    razaosocial VARCHAR(255),        
    nomefantasia VARCHAR(255),       
    documentoprincipal VARCHAR(50),  
    documentoadicional VARCHAR(50),  
    inicioatividade DATETIME,        
    codigo INT          
);

-- Omitir a coluna RowNumber do INSERT
INSERT INTO @ClientesPCI
(
    clienteid,    
	CardCodePCI,
	BPLId,
    gestatus,                    
    gedataacao,              
    empresaid,                   
    razaosocial,        
    nomefantasia,       
    documentoprincipal,  
    documentoadicional,  
    inicioatividade,     
    codigo              
)
SELECT 
    ISNULL(CRD8.U_IdPCI, ''),
	ocrd.CardCode,
	crd8.BPLId,
    1,
    GETDATE(),
    obpl.U_IdPCI,
    ocrd.CardName,
    ocrd.CardFName,
    CASE WHEN ISNULL(CRD7.TaxId0, '') <> '' THEN CRD7.TaxId0 ELSE CRD7.TaxId4 END,
    CASE WHEN ISNULL(CRD7.TaxId1, '') <> '' THEN CRD7.TaxId5 ELSE CRD7.TaxId4 END,
    ISNULL(ocrd.DateFrom, GETDATE()),
    ISNULL(ocrd.DocEntry, 0)
FROM    
    CRD7
    INNER JOIN OCRD ON OCRD.CardCode = CRD7.CardCode AND OCRD.CardType = 'C'
    INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP, '') <> 'Y' 
    INNER JOIN OBPL ON OBPL.BPLId = CRD8.BPLId AND OBPL.U_EnviaPCI = 'Y'
WHERE
    OCRD.CardType = 'C' AND
    ISNULL(CRD7.Address, '') = '' AND
    (
        ocrd.CardCode = @CardCode OR 
        (
            ISNULL(@CardCode, '') = '' AND 
            (
                OCRD.CreateDate >= @UltimaData AND OCRD.CreateTS >= @UltimaHora OR 
                OCRD.UpdateDate >= @UltimaData AND OCRD.UpdateTS >= @UltimaHora
            )
        )
    );


set @RowNumber = 1;

set @RowCount = (select count(1) from @ClientesPCI);

while (@RowNumber <= @RowCount)
begin

	SELECT 
		@clienteid = clienteid,   
		@CardCodePCI = CardCodePCI,
		@BPLId  = BPLId,
		@gestatus = gestatus,                    
		@gedataacao = gedataacao,             
		@empresaid = empresaid,                   
		@razaosocial = razaosocial,        
		@nomefantasia = nomefantasia,       
		@documentoprincipal = documentoprincipal,  
		@documentoadicional = documentoadicional,  
		@inicioatividade = inicioatividade,        
		@codigo = codigo
	FROM @ClientesPCI
	WHERE RowNumber =@RowNumber;

	if (isnull(@clienteid,'') = '') begin

		SELECT @clienteid = isnull(max((cast(clienteid as varchar(250)))),'') from [LINKZCLOUD].[zsistema_producao].[dbo].cliente where documentoprincipal = @documentoprincipal and @razaosocial = @razaosocial; 

		if (@clienteid = '') 
		begin
			set @clienteid = newid();
		end;
	end;


	exec [LINKZCLOUD].[zsistema_producao].[dbo].ZPN_PCI_InsereAtualizaCliente
													@clienteid,      
													@gestatus, 
													@gedataacao,
													@empresaid,
													@razaosocial,
													@nomefantasia,
													@documentoprincipal,
													@documentoadicional,
													@inicioatividade,
													@codigo;


    update ocrd set U_IdPCI =@clienteid where CardCode = @CardCodePCI and ISNULL(U_IdPCI,'') = '';
	update CRD8 set U_IdPCI =@clienteid where BPLId = @BPLId AND CardCode = @CardCodePCI and ISNULL(U_IdPCI,'') = '';


	set @RowNumber= @RowNumber+1;
end;


  
  
  IF (ISNULL(@CardCode,'') = '')
  BEGIN
       INSERT INTO ZPN_INTEGRAPCI (ObjType, DataExecutado, HoraExecutado)
       VALUES ('OCRD', @UltimaDataExec, @UltimaHoraExec);
  END

END TRY
BEGIN CATCH
   -- Captura do erro
    DECLARE 
        @ErrorNumber INT = ERROR_NUMBER(),
        @ErrorSeverity INT = ERROR_SEVERITY(),
        @ErrorState INT = ERROR_STATE(),
        @ErrorProcedure NVARCHAR(128) = ERROR_PROCEDURE(),
        @ErrorLine INT = ERROR_LINE(),
        @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();

    -- Inserir o log de erro na tabela ErrorLog
    INSERT INTO ZPN_LogImportacaoPCI (ErrorNumber, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorMessage, HostName, ApplicationName, UserName)
    VALUES (@ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage, HOST_NAME(), APP_NAME(), SYSTEM_USER);
    
    -- Opcional: Re-lançar o erro se necessário
    -- THROW; 

END CATCH;
	
end ;




