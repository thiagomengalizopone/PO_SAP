﻿create function ZPN_FN_RetornaObraPOHuawei
(
	@SiteInfo VARCHAR(100)
)
returns varchar(50)
as 
begin 
--DECLARE @SiteInfo VARCHAR(100) = '200039414385000<!>20240713165919<!>CEPAC01_KEY@CEPAC01_001_SETOR_REMOTO_5G_RF_00A07724258020_CE';
DECLARE @SiteInfoPesquisa VARCHAR(100);
DECLARE @Obra VARCHAR(100);
DECLARE @Posicao INT;

-- Encontrar a posição do caractere '@'
SET @Posicao = CHARINDEX('@', @SiteInfo);

 SET @SiteInfoPesquisa = @SiteInfo;
-- Se '@' for encontrado, extrair a parte da string após '@'
IF @Posicao > 0 
BEGIN
    SET @SiteInfoPesquisa = SUBSTRING(@SiteInfo, @Posicao + 1, LEN(@SiteInfo) - @Posicao);
END


set @Obra = '';

DECLARE @count INT = (SELECT COUNT(1) FROM "@ZPN_OPRJ" WHERE @SiteInfoPesquisa LIKE U_IdSite + '%');


IF @count = 1 
BEGIN
    set @Obra = (SELECT "Code" FROM "@ZPN_OPRJ" WHERE @SiteInfoPesquisa LIKE U_IdSite + '%');
END

return @Obra;

end;