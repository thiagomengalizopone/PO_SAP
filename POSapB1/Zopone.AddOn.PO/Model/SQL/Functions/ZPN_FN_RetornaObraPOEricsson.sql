create function ZPN_FN_RetornaObraPOEricsson
(
	@SiteID VARCHAR(100),
	@Obra varchar(100)
)
returns varchar(50)
as 
begin 

DECLARE @Site varchar(50);

set @Site = (
SELECT 
	ISNULL(MAX("Code"),'')
FROM
	"@ZPN_OPRJ" 
	INNER JOIN OOAT ON OOAT.AbsID = U_CodContrato
	INNER JOIN OCRD ON OCRD.CardCode = OOAT.BpCode AND OCRD.CARDNAME LIKE '%ERIC%'
WHERE 
	(
		("@ZPN_OPRJ"."Code" like '%' + @Obra +'%'  AND ISNULL(@Obra,'') <> '' )
		OR 
		U_IdSite like '%' + @SiteID +'%' OR 
		"@ZPN_OPRJ".Name like '%' + @SiteID +'%' OR 
		U_Local like '%' + @SiteID +'%')
	);


return @Site;


end;
