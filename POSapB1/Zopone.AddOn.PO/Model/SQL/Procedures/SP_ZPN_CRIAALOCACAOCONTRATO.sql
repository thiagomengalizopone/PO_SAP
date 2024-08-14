CREATE PROCEDURE SP_ZPN_CRIAALOCACAOCONTRATO
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


		

	INSERT INTO "@ZPN_ALOCONI"
           ([Code]
           ,[LineId]
           ,[Object]
           ,[LogInst]
           ,[U_CodAloc]
           ,[U_DescAloc]           
		   ,[U_PC]
		)
	SELECT 
		AbsId,
		 ROW_NUMBER() OVER(PARTITION BY OOAT.AbsId ORDER BY OOAT.AbsId) + (SELECT isnull(MAX(ALCI.[LineId]),0)+1 FROM "@ZPN_ALOCONI" ALCI WHERE ALCI."Code" = OOAT.AbsId ),
		 'ZPN_ALOCON',
		 0,
		 ALOC.Code,
		 ALOC.U_Desc,
		 'N'		
	FROM	
		OOAT
		CROSS JOIN [@ZPN_ALOCA] ALOC
	WHERE	
		(@AbsId = -1 OR OOAT.AbsId = @AbsId) and
		OOAT.AbsId not in (SELECT ALOCI."Code" FROM "@ZPN_ALOCONI" ALOCI WHERE ALOCI."Code" = OOAT.AbsId and ALOCI.U_CodAloc = ALOC."Code")

	order by OOAT.AbsId;

END;


