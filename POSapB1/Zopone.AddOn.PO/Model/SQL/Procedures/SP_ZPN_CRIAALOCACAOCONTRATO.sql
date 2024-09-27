create PROCEDURE SP_ZPN_CRIAALOCACAOCONTRATO
(
	@AbsId INT
)
AS 
BEGIN

	SET @AbsId = ISNULL(@AbsId,-1);

	INSERT INTO "@ZPN_ALOCON"
           ([Code]
           ,[Name]
           ,[DocEntry]
           ,[Canceled]
           ,[Object]
           ,[LogInst]
           ,[CreateDate]
           ,[CreateTime]           
		)
	SELECT 
		AbsId,
		AbsId,
		AbsId, 
		'N',
		'ZPN_ALOCON',
		0,
		GETDATE(),
		FORMAT(GETDATE(), 'HHmm')
	FROM	
		OOAT
	WHERE
		(@AbsId = -1 OR OOAT.AbsId = @AbsId) and
		OOAT.AbsId not in (SELECT "Code" FROM "@ZPN_ALOCON" WHERE "Code" = OOAT.AbsId );


		

END;


