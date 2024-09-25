alter PROCEDURE ZPN_SP_PCI_ATUALIZACONTRATO
(
	@AbsID INT
)
AS 
BEGIN




declare 	 @contratoid varchar(200),
	 @gestatus INT,
	 @gedataacao DATETIME,
	 @gecontaidacao VARCHAR(200),
	 @referencia VARCHAR(200),
	 @descricao VARCHAR(max),
	 @filialid VARCHAR(200),
	 @clienteid VARCHAR(200),
	 @iniciocontrato DATETIME,
	 @terminocontrato DATETIME,
	 @datacadastro DATETIME,
	 @codigo INT;



	UPDATE OOAT
		SET 
			U_IdPCI = newid()
	FROM 
		OOAT
	WHERE
		ISNULL(OOAT.U_IdPCI,'') = '' AND
		(@AbsID = OOAT.AbsId OR ISNULL (@AbsID,0) = 0 ) ;


     
	 SELECT top 1
		@contratoid = OOAT.U_IdPCI,
		@gestatus = 1,
		@gedataacao = GETDATE(),
		@gecontaidacao = NULL,
		@referencia = OOAT.Descript,
		@descricao = OOAT.Remarks,
		@filialid = OPRC.U_IdPCI,
		@clienteid = CRD8.U_IdPCI,
		@iniciocontrato = OOAT.StartDate,
		@terminocontrato = OOAT.TermDate,
		@datacadastro = GETDATE(),
		@codigo = ISNULL(OOAT.U_IdZSistemas,0)
	 FROM 
		OOAT
		INNER JOIN OPRC ON OPRC.PrcCode = OOAT.U_Regional 
		INNER JOIN OBPL ON OBPL.BPLId  = OPRC.U_MM_Filial
		INNER JOIN OCRD ON OCRD.CardCode = ooat.BpCode
		INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
						   and CRD8.BPLId = OBPL.BPLId
	WHERE
		ISNULL(OOAT.U_IdPCI,'') <> '' and 
		(@AbsID = OOAT.AbsId OR ISNULL (@AbsID,0) = 0 ) and
		ISNULL(OOAT.Descript,'') <> '' ;


	if (isnull(@contratoid,'') <> '') 
	begin 
	   exec [LINKZCLOUD].[zsistema_aceite].[dbo].[ZPN_PCI_InsereAtualizaContrato]
			@contratoid,
			@gestatus,
			@gedataacao,
			@gecontaidacao,
			@referencia,
			@descricao,
			@filialid,
			@clienteid,
			@iniciocontrato,
			@terminocontrato,
			@datacadastro,
			@codigo;
	end;









END;