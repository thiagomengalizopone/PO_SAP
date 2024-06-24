using sap.dev.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sap.dev.data;

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
                DBCreation.CriarCampoUsuario("OOAT", "CodigoRH", "Código RH", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, false, null);


                var valoresValidosSituacaoContrato = new List<Tuple<string, string>>();
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("A",  "Andamento"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("CO", "Concluída"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("E",  "Embargada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("N",  "Não Iniciada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("P",  "Paralisada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("R",  "RFI"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("CA", "Cancelada"));
                valoresValidosSituacaoContrato.Add(new Tuple<string, string>("I",  "Início Programado"));

                var valoresValidosEDF = new List<Tuple<string, string>>();
                valoresValidosEDF.Add(new Tuple<string, string>("NS", "Não esta sujeita a matrícula de obra"));
                valoresValidosEDF.Add(new Tuple<string, string>("ET", "Obra de Construção Cívil - Empreitada Total"));
                valoresValidosEDF.Add(new Tuple<string, string>("EP", "Obra de Construção Cívil - Empreitada Parcial"));


                DBCreation.CriarTabelaUsuario("ZPN_CLASSOB", "Cadastro Obra", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                DBCreation.CriarCampoUsuario("@ZPN_CLASSOB", "Posicao", "Posição", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, false, null);
                DBCreation.CriarUDO("ZPN_CLASSOB", "ZPN_CLASSOB", "ZPN_CLASSOB", SAPbobsCOM.BoUDOObjType.boud_MasterData);


                DBCreation.CriarTabelaUsuario("ZPN_OPRJ", "Cadastro Obra", SAPbobsCOM.BoUTBTableType.bott_MasterData) ;

                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "IdSite", "Id Site", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, false, null);
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
                DBCreation.CriarCampoUsuario("@ZPN_OPRJ", "PercConc", "Perc Conc", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, 5, false, null) ;


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



                List<ChildTables> TabelasFilhasCandidato = new List<ChildTables>();
                TabelasFilhasCandidato.Add(new ChildTables("ZPN_OPRJ_CAND", "ZPN_OPRJ_CAND"));

                DBCreation.CriarUDO("ZPN_OPRJ", "ZPN_OPRJ", "ZPN_OPRJ", SAPbobsCOM.BoUDOObjType.boud_MasterData, TabelasFilhasCandidato);

            }
            else if (Globals.Master.CurrentVersion < 2024060301)
            {
                

            }


        }


    }
}
