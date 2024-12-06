create VIEW [dbo].[ZPN_VW_RET_OBRA_SAP]
AS
SELECT 
T2.U_IdSenior AS numEmp, 
T1.DocEntry AS codFil,
T1.Code AS razSoc,
T1.Name AS nomFil,
ISNULL(T1.U_Rua, '') AS endFil,
ISNULL(T1.U_Numero, '') AS endNum,
'0001' AS codPai,
ISNULL(T1.U_Estado, '') AS codEst,
ISNULL(T3.IbgeCode, '') AS codCid, 
--ISNULL(T1.U_Bairro, '') AS codBai,              
'0066' AS codBai,
ISNULL(T1.U_CEP, '') AS codCep,
'O' AS tipFil, --O = Obra //Tipo Filial (M,F,O,T, C, D)
'2062' AS natEst, --Natureza Jurídica RAIS // 2062 - Sociedade Empresária Limitada
'1' AS tipIns,
T2.TaxIdNum AS numCgc,                  
'209864170114' AS insEst, --inscrição estadual, taxid1?         
'4120400' AS atiIrf, --CNAE Fiscal             
'4120400' AS atiRai, --Código Nacional de Atividade Econômica
'2' AS motEnc, --Motivo Encerramento Atividades //1 - Encerrou Atividades | 2 - Continua em Atividade | 3 - Trocou Escritório Contábil
'N' AS autExt, --Autorização de Horas Extras
'M' AS fecHEx, --Tipo de Fechamento das Horas Extras //M - Mensal | S - Semanal
'N' AS junHor, --Juntar Horas Diurnas/Noturnas cfe Turno do Horário //N - Não | S - Sim | T - Apenas 3° Turno
'N' AS parPat, --Participante PAT
'N' AS sitOrc, --Orçamento fechado p/manutenção
'P' AS ctlFic, --Controle Ficha Registro //C - Controle Centralizado | P - Controle Próprio
'3' AS pgMDsr,
'11' AS fusMar,
                           
T2.BPLName AS    NOMEFIL,  
ISNULL(T4.U_IdSenior, T6.U_IdSenior) AS CDSRPN,                        
ISNULL(T4.CardCode, T6.CardCode) AS CARDCD,                        
ISNULL(T1.U_EnSen, 'N') AS ENVIOUSENIOR    
FROM [@ZPN_OPRJ] T1 
INNER JOIN OBPL T2 ON T1.U_BPLId = T2.BPLId 
LEFT JOIN OCNT T3 ON T1.U_Cidade = T3.AbsId 
LEFT JOIN OCRD T4 ON T1.U_CardCode = T4.CardCode
LEFT JOIN OOAT T5 ON T5.AbsID = T1.U_CodContrato
LEFT JOIN OCRD T6 ON T6.cARDcODE = T5.BpCode

