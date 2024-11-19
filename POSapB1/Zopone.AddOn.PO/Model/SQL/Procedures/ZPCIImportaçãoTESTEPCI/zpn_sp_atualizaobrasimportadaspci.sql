
declare @COUNT INT;

declare @posicao INT;
DECLARE @CodigoObra varchar(20);
declare @IdPCI varchar(100);


SET @COUNT = 
(
SELECT 
	COUNT(1)
FROM
	"@ZPN_OPRJ" OBRASAP
WHERE
	U_IdPci is null
	);


set @posicao = 0;
set @CodigoObra = '';

while (@posicao < @COUNT)
begin

	SET @CodigoObra = (select min("Code")  FROM "@ZPN_OPRJ" WHERE U_IdPCI is null AND "Code" > @CodigoObra);


	set @IdPCI = (select isnull(max(cast(obraid as varchar(100))),'')  from [LINKZCLOUD].[zsistema_aceite].[dbo].OBRA where OBRA.referencia = @CodigoObra);

	update "@ZPN_OPRJ" set U_IdPCI = @IdPCI where Code = @CodigoObra;

	set @posicao= @posicao+1;
end;







