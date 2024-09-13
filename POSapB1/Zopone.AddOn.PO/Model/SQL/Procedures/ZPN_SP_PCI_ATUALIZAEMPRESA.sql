CREATE PROCEDURE ZPN_SP_PCI_ATUALIZAEMPRESA
AS
BEGIN
	UPDATE OBPL
		set obpl.U_IdPCI = EMP_PCI.empresaid
      
	  FROM 
			[192.168.8.241,15050].[Zopone].[dbo].[Empresas] EMP_Z
			INNER JOIN [LINKZCLOUD].[zsistema_aceite].[dbo].EMPRESA EMP_PCI ON EMP_PCI.CODIGOZ = EMP_Z.[emp_id]
			INNER JOIN OBPL ON OBPL.BPLId = EMP_Z.[BPL_ID]
	WHERE
		isnull(obpl.u_enviaPCI,'N') = 'Y' AND 
		EMP_Z.[emp_bd] = 'SBO_ZOPONE_ENGENHARIA' and 
		isnull(OBPL.U_IdPCI,'') <> cast(EMP_PCI.empresaid as varchar(50));

		

END;
