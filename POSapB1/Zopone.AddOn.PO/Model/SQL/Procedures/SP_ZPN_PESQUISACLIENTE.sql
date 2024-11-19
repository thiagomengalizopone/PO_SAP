create PROCEDURE SP_ZPN_PESQUISACLIENTE
(
    @CampoPesquisa VARCHAR(250)
)
AS 
BEGIN
    -- Trim whitespace from the input parameter
    SET @CampoPesquisa = TRIM(@CampoPesquisa);

    SELECT 
        T0."CardCode" as "Código Cliente",
        T0."CardName" as "Nome Cliente"
    FROM 
        OCRD T0
    WHERE
        T0."CardType" = 'C' AND 
        ((T0."CardCode"		like '%' + @CampoPesquisa + '%' ) OR 
        (T0."CardName"		like '%' + @CampoPesquisa + '%' ) );
	  

END
