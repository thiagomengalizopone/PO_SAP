UPDATE 
	CRD8	
		SET CRD8.U_IdPCI = CLI.clienteid
FROM	
    CRD8
    INNER JOIN OCRD ON OCRD."CardCode" = CRD8."CardCode" AND OCRD."CardType" = 'C'
    INNER JOIN CRD7 ON CRD7.CardCode = OCRD.CardCode AND ISNULL(CRD8.DisabledBP,'') <> 'Y' 
    INNER JOIN OBPL ON OBPL.BPLId = CRD8.BPLId AND OBPL."U_EnviaPCI" = 'Y'
    INNER JOIN [LINKZCLOUD].[zsistema_producao].[dbo].[cliente] CLI
        ON 
			CLI.[empresaid] = OBPL.U_IdPCI AND 
			CASE 
               WHEN ISNULL(CRD7.TaxId0,'') <> '' THEN CRD7.TaxId0 
               ELSE CRD7.TaxId4 
           END COLLATE SQL_Latin1_General_CP1_CI_AS = CLI.[documentoprincipal] COLLATE SQL_Latin1_General_CP1_CI_AS
WHERE
    ISNULL(CRD7.Address, '') = '' AND
    ISNULL(CRD8.U_IdPCI,'') = '' ;