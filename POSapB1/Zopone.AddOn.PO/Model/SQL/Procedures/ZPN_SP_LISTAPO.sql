
CREATE PROCEDURE ZPN_SP_LISTAPO
(
	@CampoPesquisa varchar(50)
)
AS


SELECT 
	T0."DocEntry" "Id PO", 
	T0."DocNum" "Código PO", 
	T0."U_NroPedido" "Número Pedido", 
	T0."U_NroCont" "Número Contrato", 
	OBPL."BplName" "Filial",
	T0."U_DataVenc" "Data Vencimento",  
	T1."U_PrjCode" "Código Obra",
	OPRJ."PrjName" "Obra"

FROM 
	"@ZPN_ORDR"  T0 
	INNER JOIN "@ZPN_RDR1" T1 ON T1."DocEntry"	= T0."DocEntry"
	INNER JOIN OBPL ON OBPL."BplID"				= T0."U_BplID"
	INNER JOIN OPRJ ON OPRJ."PrjCode"			= T1."U_PrjCode"
WHERE	
	cast(T0."DocEntry" as varchar(10))		like '%' + @CampoPesquisa + '%' OR 
	cast(T0."DocNum"  as varchar(10))		like '%' + @CampoPesquisa + '%' OR  
	T0."U_NroPedido"						like '%' + @CampoPesquisa + '%' OR  
	T0."U_NroCont"							like '%' + @CampoPesquisa + '%' OR  
	OBPL."BplName"							like '%' + @CampoPesquisa + '%' OR  
	cast(T0."U_DataVenc"  as varchar(10))	like '%' + @CampoPesquisa + '%' OR  
	T1."U_PrjCode"							like '%' + @CampoPesquisa + '%' OR  
	OPRJ."PrjName"							like '%' + @CampoPesquisa + '%' ;






