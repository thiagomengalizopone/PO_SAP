

alter PROCEDURE ZPN_SP_GeraProjetoTabelaB1
(
	@UserCode varchar(50)
)
as 
BEGIN
	declare @userId int;

	set @userId = (select max(userid) from ousr where user_Code = @UserCode);


INSERT INTO [@ZPN_OPRJ]
           ([Code]
           ,[Name]
           ,[DocEntry]
           ,[Canceled]
           ,[Object]
           ,[LogInst]
           ,[UserSign]
           ,[Transfered]
           ,[CreateDate]
           ,[CreateTime]
           ,[UpdateDate]
           ,[UpdateTime]
           ,[DataSource]
		)
SELECT
	OPRJ."PrjCode",
	OPRJ."PrjName", 
	(SELECT isnull(max("DocEntry"),1) FROM "@ZPN_OPRJ")  + ROW_NUMBER() OVER(order by OPRJ."PrjCode"),
	'N',
	'ZPN_OPRJ',
	0,
	@UserID,
	'N',
	GETDATE(),
	FORMAT(GETDATE(), 'HHmm'),
	GETDATE(),
	FORMAT(GETDATE(), 'HHmm'),
	'O'
FROM
	OPRJ 
WHERE 
	"PrjCode" NOT IN 
	(SELECT "Code" FROM "@ZPN_OPRJ" WHERE "Code" = OPRJ."PrjCode")
ORDER BY 
	OPRJ."PrjCode";

END;



