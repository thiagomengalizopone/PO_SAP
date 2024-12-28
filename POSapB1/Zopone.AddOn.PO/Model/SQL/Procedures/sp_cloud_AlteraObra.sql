

CREATE PROCEDURE [dbo].[sp_cloud_AlteraObra] (
	@emp_id int,
    @referencia varchar(20),
    @endereco varchar(200),
    @numero varchar(10),
    @complemento varchar(50),
    @bairro varchar(40),
    @cidade int,
    @cep varchar(9),
    @latitude varchar(50),
    @longitude varchar(50),
    @altitude float,
    @status int,
    @tipo varchar(8),
    @equipamento varchar(8)
)
AS
  declare @banco varchar(200) = 'TOPENG'
  declare @sql nvarchar(max)
  declare @params nvarchar(max)
  declare @EMP_001 int = (SELECT EMP_001 from Empresas where emp_id = @emp_id)

  IF @emp_id = 8 
  BEGIN
    SET @banco = 'TOPCHILE'
  END

  SET @sql = N'
      UPDATE LINK.' + @banco + '.dbo.OBRAS
      SET OBR_014 = @endereco,
          OBR_015 = @numero,
          OBR_016 = @complemento,
          OBR_017 = @bairro,
          CID_001 = @cidade,
          OBR_019 = @cep,
          OBR_046 = @altitude,
          OBR_047 = @latitude,
          OBR_048 = @longitude,
          OBR_011 = @status,
          OBR_051 = @tipo,
          OBR_052 = @equipamento
      WHERE EMP_001 = @EMP_001
	  AND OBR_002 = @referencia
  '
  SET @params = N'
    @endereco varchar(50),
    @numero varchar(10),
    @complemento varchar(50),
    @bairro varchar(40),
    @cidade int,
    @cep varchar(9),
    @altitude float,
    @latitude varchar(50),
    @longitude varchar(50),
    @status int,
    @tipo varchar(8),
    @equipamento varchar(8),
    @EMP_001 int,
    @referencia varchar(20)
  '
exec sp_executesql @sql, @params, @endereco, @numero, @complemento, @bairro, @cidade, @cep, @altitude, @latitude, @longitude, @status, @tipo, @equipamento, @EMP_001, @referencia
--print(@sql)
GO


