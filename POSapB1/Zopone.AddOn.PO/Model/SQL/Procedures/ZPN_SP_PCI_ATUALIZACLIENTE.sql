CREATE PROCEDURE ZPN_SP_PCI_ATUALIZACLIENTE
(
	@CardCode varchar(50)
)
AS 
BEGIN


---DECLARE @CardCode varchar(50);

--ZPN_SP_ATUALIZACLIENTEPCI ''


INSERT INTO 
    [LINKZCLOUD].[zsistema_aceite].[dbo].[cliente]
	(
		 [clienteid]
		,[gestatus]
		,[gedataacao]
		,[gecontaidacao]
		,[empresaid]
		,[razaosocial]
		,[nomefantasia]
		,[documentoprincipal]
		,[documentoadicional]
		,[inicioatividade]
		,[codigo]
	)

SELECT 
	NEWID(),
	1,
	GETDATE(),
	null,
	obpl.U_IdPCI,
	ocrd.CardName,
	ocrd.CardFName,
	CASE WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 ELSE CRD7.TaxId4 end,
	CASE WHEN ISNULL(CRD7.TaxId1,'') <> '' THEN CRD7.TaxId5 ELSE CRD7.TaxId4 end,
	isnull(ocrd.DateFrom, getdate()) ,
	isnull(ocrd.U_IdZSistemas,0)
	
FROM	
	CRD7
    INNER JOIN OCRD ON OCRD."CardCode" = CRD7."CardCode" and OCRD."CardType" = 'C'
	INNER JOIN CRD8 ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
	INNER JOIN OBPL ON OBPL.BPLId    = CRD8.BPLId and obpl."U_EnviaPCI" = 'Y'
	LEFT  JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[cliente] CLI
        ON CLI.[empresaid] = OBPL.U_IdPCI AND 
           CRD8.U_IdPCI = CAST(CLI.[clienteid] AS VARCHAR(50)) COLLATE SQL_Latin1_General_CP1_CI_AS
WHERE
	CLI.clienteid IS NULL AND 
	(isnull(@CardCode,'') = '' or ocrd.CardCode = @CardCode) and
	ISNULL(CRD7.Address, '') = '' AND
	ISNULL(CRD8.U_IdPCI,'') = '' ;
	

UPDATE CLI
SET
	CLI.gestatus = 1,
	CLI.gedataacao = GETDATE(),
	CLI.gecontaidacao = NULL,
	CLI.empresaid = OBPL.U_IdPCI,
	CLI.razaosocial = OCRD.CardName,
	CLI.nomefantasia = OCRD.CardFName,
	CLI.documentoprincipal = CASE 
								WHEN ISNULL(CRD7.TaxId0, '') <> '' THEN CRD7.TaxId0 
								ELSE CRD7.TaxId4 
							 END,
	CLI.documentoadicional = CASE 
								WHEN ISNULL(CRD7.TaxId1, '') <> '' THEN CRD7.TaxId5 
								ELSE CRD7.TaxId4 
							 END,
	CLI.inicioatividade = ISNULL(OCRD.DateFrom, GETDATE()),
	CLI.codigo = ISNULL(OCRD.U_IdZSistemas, 0)
FROM 
	CRD7
	INNER JOIN OCRD 
		ON OCRD."CardCode" = CRD7."CardCode" AND OCRD."CardType" = 'C'
	INNER JOIN CRD8 
		ON CRD8.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP, '') <> 'Y' 
	INNER JOIN OBPL 
		ON OBPL.BPLId = CRD8.BPLId AND OBPL."U_EnviaPCI" = 'Y'
	INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[cliente] CLI
		ON CLI.[empresaid] = OBPL.U_IdPCI AND 
		   CASE 
			   WHEN ISNULL(CRD7.TaxId0, '') <> '' THEN CRD7.TaxId0 
			   ELSE CRD7.TaxId4 
		   END COLLATE SQL_Latin1_General_CP1_CI_AS = CLI.[documentoprincipal] COLLATE SQL_Latin1_General_CP1_CI_AS
WHERE
	(isnull(@CardCode, '') = '' OR OCRD.CardCode = @CardCode) AND
	ISNULL(CRD7.Address, '') = '' ;


	
UPDATE 
	CRD8	
		SET CRD8.U_IdPCI = CLI.clienteid
FROM	
    CRD8
    INNER JOIN OCRD ON OCRD."CardCode" = CRD8."CardCode" AND OCRD."CardType" = 'C'
    INNER JOIN CRD7 ON CRD7.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
    INNER JOIN OBPL ON OBPL.BPLId = CRD8.BPLId AND OBPL."U_EnviaPCI" = 'Y'
    INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].[cliente] CLI
        ON 
			CLI.[empresaid] = OBPL.U_IdPCI AND 
			CASE 
               WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 
               ELSE CRD7.TaxId4 
           END COLLATE SQL_Latin1_General_CP1_CI_AS = CLI.[documentoprincipal] COLLATE SQL_Latin1_General_CP1_CI_AS
WHERE
	(isnull(@cardCode,'') = '' or  ocrd.CardCode = @CardCode) and 
    ISNULL(CRD7.Address, '') = '' AND
    ISNULL(CRD8.U_IdPCI,'') = '' ;



	
	
end ;

