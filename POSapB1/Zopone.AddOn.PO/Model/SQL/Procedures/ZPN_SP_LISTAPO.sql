
CREATE PROCEDURE ZPN_SP_LISTAPO
(
	@CampoPesquisa varchar(50)
)
AS


SELECT 
	T0."DocEntry"							"Id PO", 
	T0."DocNum"								"Código PO", 
	T0.NumAtCard							"Número Pedido", 
	T0."U_NroCont"							"Número Contrato", 
	OBPL."BplName"							"Filial",
	cast(T0.DocDueDate  as varchar(10))		"Data Vencimento",  
	OPRJ."PrjCode"							"Código Obra",
	OPRJ."PrjName"							"Obra",
	'P'										"Documento"

FROM 
	ORDR  T0 
	INNER JOIN RDR1 T1 ON T1."DocEntry"	= T0."DocEntry"
	INNER JOIN OBPL	   ON OBPL."BplID"				= T0.BPLId
	INNER JOIN OPRJ    ON OPRJ."PrjCode"			= T1.Project
WHERE	
	cast(T0."DocEntry" as varchar(10))		like '%' + @CampoPesquisa + '%' OR 
	cast(T0."DocNum"  as varchar(10))		like '%' + @CampoPesquisa + '%' OR  
	T0.NumAtCard							like '%' + @CampoPesquisa + '%' OR  
	T0."U_NroCont"							like '%' + @CampoPesquisa + '%' OR  
	OBPL."BplName"							like '%' + @CampoPesquisa + '%' OR  
	cast(T0.DocDueDate  as varchar(10))	    like '%' + @CampoPesquisa + '%' OR  
	OPRJ."PrjCode"							like '%' + @CampoPesquisa + '%' OR  
	OPRJ."PrjName"							like '%' + @CampoPesquisa + '%' 

union all 



SELECT 
	T0."DocEntry"							"Id PO", 
	T0."DocNum"								"Código PO", 
	T0.NumAtCard							"Número Pedido", 
	T0."U_NroCont"							"Número Contrato", 
	OBPL."BplName"							"Filial",
	cast(T0.DocDueDate  as varchar(10))		"Data Vencimento",  
	OPRJ."PrjCode"							"Código Obra",
	OPRJ."PrjName"							"Obra",
	'D'										"Documento"

FROM 
	ODRF  T0 
	INNER JOIN DRF1 T1 ON T1."DocEntry"	= T0."DocEntry"
	INNER JOIN OBPL	   ON OBPL."BplID"				= T0.BPLId
	INNER JOIN OPRJ    ON OPRJ."PrjCode"			= T1.Project
WHERE	
	T0."ObjType"							= '17' and
	cast(T0."DocEntry" as varchar(10))		like '%' + @CampoPesquisa + '%' OR 
	cast(T0."DocNum"  as varchar(10))		like '%' + @CampoPesquisa + '%' OR  
	T0.NumAtCard							like '%' + @CampoPesquisa + '%' OR  
	T0."U_NroCont"							like '%' + @CampoPesquisa + '%' OR  
	OBPL."BplName"							like '%' + @CampoPesquisa + '%' OR  
	cast(T0.DocDueDate  as varchar(10))	    like '%' + @CampoPesquisa + '%' OR  
	OPRJ."PrjCode"							like '%' + @CampoPesquisa + '%' OR  
	OPRJ."PrjName"							like '%' + @CampoPesquisa + '%' ;


