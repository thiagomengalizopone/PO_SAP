using sap.dev.core;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.Helpers
{
    internal class UtilProjetos
    {


        public static void SalvarProjeto(string CodeObra, string NomeObra, string BplName= "")
        {

            CompanyService oCmpSrv = Globals.Master.Connection.Database.GetCompanyService();

            ProjectsService projectService = (ProjectsService)oCmpSrv.GetBusinessService(ServiceTypes.ProjectsService);

            Project project = (Project)projectService.GetDataInterface(ProjectsServiceDataInterfaces.psProject);

            project.Code = CodeObra;
            project.Name = NomeObra;
            project.UserFields.Item("U_BPLName").Value = BplName;


            projectService.AddProject(project);

        }

    }
}
