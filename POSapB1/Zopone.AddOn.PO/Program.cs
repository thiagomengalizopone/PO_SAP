using sap.dev.core;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Zopone.AddOn.PO
{
    class Program
    {
        [STAThread]
        static void Main(string[] aArguments)
        {
            try
            {

                System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException);

                AppDomain currentDomain = AppDomain.CurrentDomain;

                AddOnMain.Inicializar(aArguments);
                System.Windows.Forms.Application.Run();
            }
            catch (ReflectionTypeLoadException ex)
            {
                string errorMessage = string.Empty;
                try
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        sb.AppendLine(exSub.Message);
                        if (exSub is FileNotFoundException exFileNotFound)
                        {
                            if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                            {
                                sb.AppendLine("Fusion Log:");
                                sb.AppendLine(exFileNotFound.FusionLog);
                            }
                        }
                        sb.AppendLine();
                    }
                    errorMessage = sb.ToString();
                }
                catch
                {
                    errorMessage = ex.Message;
                }
                Util.GravarLog(sap.dev.core.EnumList.EnumAddOn.AddOnUtilitarios, sap.dev.core.EnumList.TipoMensagem.Erro, errorMessage, ex);

                System.Windows.Forms.MessageBox.Show("Erro ao iniciar o Add-On: " + errorMessage);
                System.Windows.Forms.Application.Exit();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao iniciar o Add-On: " + e.Message);

                try
                {
                    Util.GravarLog(sap.dev.core.EnumList.EnumAddOn.AddOnUtilitarios, sap.dev.core.EnumList.TipoMensagem.Erro, "Erro ao iniciar AddOn", e);

                }
                finally
                {
                    System.Windows.Forms.Application.Exit();
                }
            }
        }
    }
}
