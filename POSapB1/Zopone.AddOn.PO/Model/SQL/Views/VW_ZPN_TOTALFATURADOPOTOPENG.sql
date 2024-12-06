CREATE VIEW VW_ZPN_TOTALFATURADOPOTOPENG
AS
select top 100 
	po_001 "po_number", 
	sum(NFSE_005) "TotalFaturado" 
from 
	SQL_TOPENG.TOPENG.DBO.NFE_Servico 
where
	NFSE_039 is null 

group by 
	po_001