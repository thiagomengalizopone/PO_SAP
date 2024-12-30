



create view vw_sp_temp_importarcontratos
as
  SELECT top 100
      CONT.contratoid,
      cont.referencia,
      cont.descricao,
      cont.iniciocontrato,
      CRD8.CardCode

  FROM
      [LINKZCLOUD].[zsistema_producao].[dbo].CONTRATO CONT
      INNER JOIN OPRC ON OPRC.U_IdPCI = CONT.FILIALID
      INNER JOIN CRD8 ON CRD8.U_IdPCI  = CONT.CLIENTEID
  where
	 cont.referencia collate SQL_Latin1_General_CP1_CI_AS not in 
		(
			SELECT DESCRIPT FROM OOAT WHERE DESCRIPT =  cont.referencia collate SQL_Latin1_General_CP1_CI_AS
		)
      and cont.iniciocontrato >= '2019-01-01'