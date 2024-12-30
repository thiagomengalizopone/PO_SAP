CREATE PROCEDURE [ZPN_SP_PCI_ATUALIZACONTRATO]
(
	@AbsIDParam INT
)
AS 
BEGIN

BEGIN TRY
DECLARE @AbsID INT;

declare @RouCount int;
declare @RouNum int;


		DECLARE @UltimaData date;
		DECLARE @UltimaHora int;
		DECLARE @UltimaDataExec DATE;
        DECLARE @UltimaHoraExec INT;

declare 	 @contratoid varchar(200),
	 @gestatus INT,
	 @gedataacao DATETIME,
	 @referencia VARCHAR(200),
	 @descricao VARCHAR(max),
	 @filialid VARCHAR(200),
	 @clienteid VARCHAR(200),
	 @iniciocontrato DATETIME,
	 @terminocontrato DATETIME,
	 @datacadastro DATETIME,
	 @codigo INT;
	 

	 
        -- Captura a data e hora da execução atual
        SET @UltimaDataExec = GETDATE();
        SET @UltimaHoraExec = CAST(REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') AS INT);

        -- Recupera as últimas execuções
        SET @UltimaData = (SELECT ISNULL(MAX(DataExecutado), '2024-01-01') FROM ZPN_INTEGRAPCI WHERE ObjType = 'OOAT');
        SET @UltimaHora = (SELECT ISNULL(MAX(HoraExecutado), 0) FROM ZPN_INTEGRAPCI WHERE ObjType ='OOAT');


	declare @Contratos table 
	(
	     RowNumber INT IDENTITY(1,1),
		 AbsId int,
		 contratoid varchar(200),
		 gestatus INT,
		 gedataacao DATETIME,
		 referencia VARCHAR(200),
		 descricao VARCHAR(max),
		 filialid VARCHAR(200),
		 clienteid VARCHAR(200),
		 iniciocontrato DATETIME,
		 terminocontrato DATETIME,
		 datacadastro DATETIME,
		 codigo INT
	 );


     INSERT INTO @Contratos
	 (
		 AbsId,
		 contratoid,
		 gestatus,
		 gedataacao,
		 referencia,
		 descricao,
		 filialid ,
		 clienteid,
		 iniciocontrato,
		 terminocontrato,
		 datacadastro,
		 codigo
	 )
	 SELECT
		ooat.absid,
		isnull(OOAT.U_IdPCI,''),
		1,
		GETDATE(),
		OOAT.Descript,
		OOAT.Remarks,
		OPRC.U_IdPCI,
		CRD8.U_IdPCI,
		OOAT.StartDate,
		OOAT.TermDate,
		GETDATE(),
		ISNULL(OOAT.AbsID,0)
	 FROM 
		OOAT
		INNER JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
		INNER JOIN OBPL ON OBPL.BPLId  = OPRC.U_MM_Filial
		INNER JOIN OCRD ON OCRD.CardCode = ooat.BpCode
		INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
						   and CRD8.BPLId = OBPL.BPLId
	WHERE
		(
			@AbsIDParam = OOAT.AbsId 
			or 
			(
				ISNULL(@AbsIDParam ,0) = 0 AND 
				(
					(ooat.CreateDate >= @UltimaData   
					OR 
					(
						ooat.UpdtDate >= @UltimaData   
					)
				)
			)
		
		) and
		ISNULL(OOAT.Descript,'') <> '' );

		set @RouNum = 1;

		select @RouCount = (select count(1) From @Contratos );

		while (@RouNum <= @RouCount)
		begin 
			
			
			select 
				 @AbsID = AbsId,
				 @contratoid = contratoid,
				 @gestatus =  gestatus,
				 @gedataacao = gedataacao,
				 @referencia = referencia,
				 @descricao = descricao,
				 @filialid =  filialid ,
				 @clienteid = clienteid,
				 @iniciocontrato = iniciocontrato,
				 @terminocontrato = terminocontrato,
				 @datacadastro = datacadastro,
				 @codigo = codigo

			from 
				@Contratos	
			where 
				RowNumber = @RouNum;
			

			
			if (@contratoid = '')
			begin 
				select @contratoid = isnull(max(cast(contratoid as varchar(250))),'') from [LINKZCLOUD].[zsistema_producao].[dbo].contrato where [referencia] = @referencia;

				if (@contratoid = '') begin
					set @contratoid = newid();
				end;	
			end;

	
		   exec [LINKZCLOUD].[zsistema_producao].[dbo].[ZPN_PCI_InsereAtualizaContrato]
				@contratoid,
				@gestatus,
				@gedataacao,
				@referencia,
				@descricao,
				@filialid,
				@clienteid,
				@iniciocontrato,
				@terminocontrato,
				@datacadastro,
				@codigo;


			UPDATE OOAT
					SET 
						U_IdPCI = @contratoid
				FROM 
					OOAT
				WHERE
					ISNULL(OOAT.U_IdPCI,'') = '' 
					AND @AbsID = OOAT.AbsId ;


			set @RouNum = @RouNum+1;


		end;

		IF (ISNULL(@AbsIDParam,0) = 0)
	  BEGIN  

		   INSERT INTO ZPN_INTEGRAPCI (ObjType, DataExecutado, HoraExecutado)
		   VALUES ('OOAT', @UltimaDataExec, @UltimaHoraExec);
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

END;

