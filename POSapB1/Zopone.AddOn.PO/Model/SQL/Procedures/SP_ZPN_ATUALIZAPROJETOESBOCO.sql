
create procedure SP_ZPN_ATUALIZAPROJETOESBOCO(@DocEntry INT)
as 
begin

	UPDATE 
		DRF6 
	SET 
		U_Project = DRF1.Project
	FROM 
		DRF6 
		INNER JOIN ODRF ON ODRF.DocEntry = DRF6.DocEntry
		INNER JOIN DRF1 ON DRF1.DocEntry = ODRF.DocEntry 
	WHERE 
		ISNULL(DRF1.Project,'') <> '' AND ISNULL(DRF6.U_Project,'') = '' 
		AND DRF6.DocEntry = @DocEntry;

end;