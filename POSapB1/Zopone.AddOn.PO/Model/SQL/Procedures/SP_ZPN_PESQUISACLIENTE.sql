alter PROCEDURE SP_ZPN_PESQUISACLIENTE
(
    @CampoPesquisa VARCHAR(250)
)
AS 
BEGIN
    -- Trim whitespace from the input parameter
    SET @CampoPesquisa = TRIM(@CampoPesquisa);

    SELECT 
        T0."CardCode" as "Código Cliente",
        T0."CardName"  + ' ' + 
		CASE 
			WHEN isnull(T1.TaxId0,'') <> '' THEN T1.TaxId0
			ELSE isnull(T1.TaxId4,'') 
		END as "Nome Cliente"
    FROM 
        OCRD T0
        LEFT JOIN CRD7 T1 ON T1."CardCode" = T0."CardCode"  and isnull(t1.Address   ,'') = ''
    WHERE
        T0."CardType" = 'C' AND 
        (
			(T0."CardCode"		like '%' + @CampoPesquisa + '%') OR 
			(T0."CardName"		like '%' + @CampoPesquisa + '%')  OR
			(T1.TaxId0			like '%' + @CampoPesquisa + '%') or 
			(T1.TaxId4			like '%' + @CampoPesquisa + '%') 
		);
	  

END

