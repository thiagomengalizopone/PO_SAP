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

/*
	INSERT INTO "@ZPN_ALOCONI"
        SELECT 
            OOAT.AbsID,
            1 + ISNULL(
                    (SELECT MAX("LineId") 
                     FROM "@ZPN_ALOCONI" 
                     WHERE "Code" = OOAT.AbsID), 0) + 
            (ROW_NUMBER() OVER (PARTITION BY OOAT.AbsID ORDER BY OOAT.AbsID ASC)) AS 'LineId',
            'ZPN_ALOCON',
            NULL,
            "@ZPN_ALOCA"."Code",
            "@ZPN_ALOCA".U_Desc,
            'Y'
        FROM 
            SQL_TOPENG.TOPENG.DBO.[CONTRATO_ETAPA]
        INNER JOIN 
            SQL_TOPENG.TOPENG.DBO.ETAPA ON ETAPA.ETA_001 = CONTRATO_ETAPA.ETA_001
        INNER JOIN 
            SQL_TOPENG.TOPENG.DBO.CONTRATO ON CONTRATO.CTR_001 = CONTRATO_ETAPA.CTR_001
        INNER JOIN 
            OOAT ON OOAT.descript = CONTRATO.CTR_002 COLLATE SQL_Latin1_General_CP850_CI_AS
        INNER JOIN 
            "@ZPN_ALOCA" ON "@ZPN_ALOCA"."Code" = CAST(CONTRATO_ETAPA.ETA_001 AS VARCHAR(100))
        WHERE 
            (@AbsId = -1 OR OOAT.AbsId = @AbsId) and
            OOAT.AbsID NOT IN (
                SELECT ALCI."Code"
                FROM "@ZPN_ALOCONI" ALCI
                WHERE ALCI.Code = OOAT.AbsID 
                  AND ALCI.U_CodAloc = "@ZPN_ALOCA"."Code"
            );
            */

END;


