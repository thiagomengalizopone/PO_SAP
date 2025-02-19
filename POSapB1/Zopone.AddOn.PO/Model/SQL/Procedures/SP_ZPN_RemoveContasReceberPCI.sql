create PROCEDURE SP_ZPN_RemoveContasReceberPCI 
(
   @DocEntry int
)
as 
begin

DECLARE @U_IdPCI varchar(250);



delete from ZPN_ALOCACAOPARCELANF where "DocEntry" = @DocEntry;


DECLARE cursor_inv6 CURSOR FOR
SELECT
    inv6.U_IdPCI
FROM    
    OINV
    INNER JOIN INV6 ON INV6.DocEntry = OINV.DocEntry 
WHERE 
    OINV.DocEntry = @DocEntry;


OPEN cursor_inv6;


FETCH NEXT FROM cursor_inv6 INTO @U_IdPCI;


WHILE @@FETCH_STATUS = 0
BEGIN
	
	exec linkzcloud.zsistema_producao.dbo.[ZPN_PCI_DeletaContaReceber] @U_IdPCI;


    FETCH NEXT FROM cursor_inv6 INTO @U_IdPCI;
END


CLOSE cursor_inv6;
DEALLOCATE cursor_inv6;

end;
