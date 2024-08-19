﻿using sap.dev.core;
using sap.dev.data;
using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace Zopone.AddOn.PO.Model.SAP
{
    public class MetaData
    {
        public static void CreateMetaData()
        {
            var valoresValidosSimNao = new List<Tuple<string, string>>();
            valoresValidosSimNao.Add(new Tuple<string, string>("Y", "Sim"));
            valoresValidosSimNao.Add(new Tuple<string, string>("N", "Não"));

            if (Globals.Master.CurrentVersion < 2024070101)
            {

                #region Campos AddOn WMS
                var valoresValidosTipoDocumento = new List<Tuple<string, string>>();
                valoresValidosTipoDocumento.Add(new Tuple<string, string>("TR", "TR"));
                valoresValidosTipoDocumento.Add(new Tuple<string, string>("TA", "TA"));

                DBCreation.CriarCampoUsuario("ORDR", "ZPN_TipoDocto", "Tipo Documento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, false, null, valoresValidosTipoDocumento);

                #endregion


                var valoresValidosSituacaoContrato = new List<Tuple<string, string>>();
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("1", "Andamento"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("2", "Concluída"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("3", "Embargada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("4", "Não Iniciada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("5", "Paralisada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("6", "RFI"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("7", "Cancelada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("8", "Início Programado"));

                var valoresValidosEDF = new List<Tuple<string, string>>();
                valoresValidosEDF.Add(new Tuple<string, string>("NS", "Não esta sujeita a matrícula de obra"));
                valoresValidosEDF.Add(new Tuple<string, string>("ET", "Obra de Construção Cívil - Empreitada Total"));
                valoresValidosEDF.Add(new Tuple<string, string>("EP", "Obra de Construção Cívil - Empreitada Parcial"));

                #region Contrato
                DBCreation.CriarCampoUsuario("OOAT", "CodigoRH", "Código RH", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                
                DBCreation.CriarCampoUsuario("OOAT", "Regional", "Código Regional", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);

                #endregion

                #region DOCUMENTOS
                var valoresValidosStatusFaturamento = new List<Tuple<string, string>>();
                valoresValidosStatusFaturamento.Add(new Tuple<string, string>("A", "À Faturar"));
                valoresValidosStatusFaturamento.Add(new Tuple<string, string>("F", "Liberado para Faturamento"));

                DBCreation.CriarCampoUsuario("ORDR", "NroCont", "Número Contrato Cliente", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("ORDR", "IdPO", "ID Po Z Sistemas", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 11, false, null);
                
                DBCreation.CriarCampoUsuario("RDR1", "manSiteInfo", "manufactureSiteInfo", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "Candidato", "Candidato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "CardCode", "Projeto", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "CardName", "Projeto Descrição", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "Item", "Item", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "ItemFat", "Item Faturamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "Parcela", "Parcela", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "Tipo", "Tipo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "DataLanc", "Data Lançamento", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "DataFat", "Data Faturamento", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "NroNF", "Número NF", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "DataSol", "Data Solicitação", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "Bloqueado", "Bloqueado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, "N", valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("RDR1", "itemDescription", "itemDescription", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false);
                
                DBCreation.CriarCampoUsuario("RDR1", "StatusFat", "Status Faturamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, "A", valoresValidosStatusFaturamento);

                #endregion

                #region ALOCAÇÃO OBRA                
                DBCreation.CriarTabelaUsuario("ZPN_ALOCON", "Cadastro Contrato Aloc.", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarTabelaUsuario("ZPN_ALOCONI", "Cadastro Contrato Aloc I", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCONI", "CodAloc", "Código Alocação", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCONI", "DescAloc", "Descrição Item", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCONI", "PC", "PC", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, "N", valoresValidosSimNao);


                List<ChildTables> TabelasFilhasAlocacao = new List<ChildTables>();
                TabelasFilhasAlocacao.Add(new ChildTables("ZPN_ALOCONI", "ZPN_ALOCONI"));

                DBCreation.CriarUDO("ZPN_ALOCON", "ZPN_ALOCON", "ZPN_ALOCON", SAPbobsCOM.BoUDOObjType.boud_MasterData, TabelasFilhasAlocacao);
                #endregion

                #region PARAMETROS PO

                var valoresTipoDocumentoPO = new List<Tuple<string, string>>();
                valoresTipoDocumentoPO.Add(new Tuple<string, string>("E", "Esboço Pedido de Venda"));
                valoresTipoDocumentoPO.Add(new Tuple<string, string>("P", "Pedido de Venda"));

                DBCreation.CriarTabelaUsuario("ZPN_CONFPO", "Cadastro Configurações PO", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "TipoDoc", "Tipo Documento PO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresTipoDocumentoPO);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "CardCodeH", "Código PN Hauey", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "CardNameH", "Descrição PN Hauey", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "CardCodeE", "Código PN Ericsson", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "CardNameE", "Descrição PN Ericsson", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "ItemCode", "Código Item", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_CONFPO", "ItemName", "Descrição Item", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarUDO("ZPN_CONFPO", "ZPN_CONFPO", "ZPN_CONFPO", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                #endregion

                #region Alocação
                var valoresValidosClassificacao = new List<Tuple<string, string>>();
                valoresValidosClassificacao.Add(new Tuple<string, string>("C", "Contrato"));
                valoresValidosClassificacao.Add(new Tuple<string, string>("O", "Obra"));

                var valoreSValidosTipo = new List<Tuple<string, string>>();
                valoreSValidosTipo.Add(new Tuple<string, string>("A", "Analítico"));
                valoreSValidosTipo.Add(new Tuple<string, string>("S", "Sintético"));

                DBCreation.CriarTabelaUsuario("ZPN_ALOCA", "Cadastro Alocação - Etapa Obra", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "Classif", "Classificação", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosClassificacao);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "Ref", "Referência", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "Desc", "Descrição", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "Tipo", "Tipo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoreSValidosTipo);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "ItensRec", "Itens do Recebimento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "ItensPed", "Itens do Pedido", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "ItensFat", "Itens de Faturamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "ExporEtap", "Exportar Etapa", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "Perc", "Percentual", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, 40, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "EtapaRec", "Etapa Recebimento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "EtapaRecD", "Etapa Recebimento Desc", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "EtapaFat", "Etapa Faturamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 40, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "EtapaFatD", "Etapa Faturamento Desc", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "BplID", "Filial", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);

                DBCreation.CriarUDO("ZPN_ALOCA", "ZPN_ALOCA", "ZPN_ALOCA", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                #endregion

                #region Classificação da Obra
                DBCreation.CriarTabelaUsuario("ZPN_CLASSOB", "Cadastro Obra", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarCampoUsuario("@ZPN_CLASSOB", "Posicao", "Posição", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);
                DBCreation.CriarUDO("ZPN_CLASSOB", "ZPN_CLASSOB", "ZPN_CLASSOB", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                #endregion

                #region Classificação da Obra
                DBCreation.CriarTabelaUsuario("ZPN_DETENT", "Detentora", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarUDO("ZPN_DETENT", "ZPN_DETENT", "ZPN_DETENT", SAPbobsCOM.BoUDOObjType.boud_MasterData);
                #endregion

                #region Cadastro de Obra 
                DBCreation.CriarTabelaUsuario("ZPN_OPRJ", "Cadastro Obra", SAPbobsCOM.BoUTBTableType.bott_MasterData);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "IdSite", "Id Site", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Equip", "Equipamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Detent", "Detentora", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "IdDetent", "Id Detentora", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Regional", "Regional", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);


                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "CodContrato", "Código Contrato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "DescContrato", "Descrição Contrato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "BPLId", "Filial", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "NroRH", "Obra no RH", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "CodCliente", "Código Cliente", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "ARQ", "Acceptance Request", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "ValorPrev", "Valor Previsto Fat", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "PlanejObra", "Planejamento Obra", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "TotalFrete", "Total de Frete", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 100, false, null);


                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Pais", "Estado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Estado", "Estado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Cidade", "Cidade Código", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "TipoLog", "Tipo Logradouro", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Rua", "Rua", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Numero", "Numero", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Complemento", "Complemento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "CEP", "CEP", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Bairro", "Bairro", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Latitude", "Latitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Longitude", "Longitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Altitude", "Longitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Situacao", "Situação Contrato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null, valoresValidosSituacaoContrato);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Status", "Situação Contrato", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Tipo", "Tipo", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "PercConc", "Perc Conc", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, 5, false, null);


                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "PrevIni", "Previsão Início", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "PrevTerm", "Previsão Término", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "RelIni", "Realizado Início", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "RelTerm", "Realizado Término", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Habitese", "Habitese", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Alvara", "Alvara", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "CEI", "CEI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "EFDReinf", "EFD Reinf", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null, valoresValidosEDF);


                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "NUSolP", "Nõa Utiliz. Sol. Pag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "NURI", "Nõa Utiliz. Sol. Pag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null, valoresValidosSimNao);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "VisPCI", "Nõa Utiliz. Sol. Pag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, false, null, valoresValidosSimNao);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Emp", "Empreendimento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "ClassOb", "Classificação Obra", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Medicao", "Medição", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "Obs", "Observação", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);


                DBCreation.CriarTabelaUsuario("ZPN_OPRJ_CAND", "Cadastro Obra - Candidato", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Identif", "Identificação", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Tipo", "Identificação", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Nome", "Identificação", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Detentora", "Detentora", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "IdDetentora", "Id Detentora", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Pais", "Estado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Estado", "Estado", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Cidade", "Cidade Código", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "TipoLog", "Tipo Logradouro", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Rua", "Rua", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Numero", "Numero", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Complemento", "Complemento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "CEP", "CEP", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Bairro", "Bairro", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Latitude", "Latitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Longitude", "Longitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Altitude", "Longitude", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Codigo", "Código", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null) ;
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "Equip", "Equipamento", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);



                List<ChildTables> TabelasFilhasCandidato = new List<ChildTables>();
                TabelasFilhasCandidato.Add(new ChildTables("ZPN_OPRJ_CAND", "ZPN_OPRJ_CAND"));

                DBCreation.CriarUDO("ZPN_OPRJ", "ZPN_OPRJ", "ZPN_OPRJ", SAPbobsCOM.BoUDOObjType.boud_MasterData, TabelasFilhasCandidato);
                #endregion

                #region Campos PCI

                

                DBCreation.CriarCampoUsuario("OBPL", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("OBPL", "EnviaPCI", "Enviar para PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, false, null, valoresValidosSimNao);
                
                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ_CAND", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                

                DBCreation.CriarCampoUsuario("OCRD", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("CRD8", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("OCRD", "IdZSistemas", "Campo ID Z Sistemas", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("OPRC", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("OPRC", "IdZSistemas", "Campo ID Z Sistemas", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("OOAT", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("OOAT", "IdZSistemas", "Campo ID Z Sistemas", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_DETENT", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("ORDR", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);
                DBCreation.CriarCampoUsuario("RDR1", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                DBCreation.CriarCampoUsuario("@ZPN_CLASSOB", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);


                DBCreation.CriarCampoUsuario("@ZPN_ALOCA", "IdPCI", "Campo ID PCI", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 60, false, null);

                #endregion

            }
            else if (Globals.Master.CurrentVersion < 2024060301)
            {


            }


        }


    }
}
