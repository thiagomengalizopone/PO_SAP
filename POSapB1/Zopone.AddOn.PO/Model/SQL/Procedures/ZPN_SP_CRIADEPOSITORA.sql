CREATE PROCEDURE ZPN_SP_CRIADEPOSITORA
AS 
BEGIN



	SELECT	
		cast(OBPL.Bplid as varchar(5)) + '-RA' "WhsCode",
		'RA-' + OBPL.BplName "WhsName", 
		obpl.BPLId "BPLId"
	FROM
		OBPL
	WHERE
		cast(OBPL.Bplid as varchar(5)) + '-RA' NOT IN
		(
			SELECT 
				OWHS.WHSCODE
			FROM
				OWHS
			WHERE 
				OWHS.WHSCODE = cast(OBPL.Bplid as varchar(5)) + '-RA'
		);
END;
	