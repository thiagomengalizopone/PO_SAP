Create procedure  ZPN_SP_CODIGOCENTROCUSTO
AS 
BEGIN
	SELECT max(cast(PrcCode as int))+1 "PrcCode" FROM OPRC where isnumeric(PrcCode) = 1
END;