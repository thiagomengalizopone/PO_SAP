
CREATE PROCEDURE SP_ZPN_ATUALIZAPROJETONF(@DocEntry int)
as 
BEGIN


	update DRF1 SET U_Project = Project WHERE DocEntry = @DocEntry and isnull(Project,'') <> ''; 

	UPDATE DRF6
		SET U_Project = DRF1.Project
	FROM
		DRF6 
		INNER JOIN DRF1 ON DRF1."DocEntry" = DRF6."DocEntry"
	where 
		 isnull(DRF1.Project,'') <> '' AND
		DRF6.DocEntry = @DocEntry;
end;