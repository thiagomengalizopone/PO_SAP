using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopone.AddOn.PO.UtilAddOn
{
    public class AprovacaoDocumento
    {
        public class TextoAprovacao
        {
            public int DocEntry { get; set; }
            public int ObjType { get; set; }
            public string MensagemAprovacao { get; set; }
        }

        public static List<TextoAprovacao> textoAprovacao = new List<TextoAprovacao>();

    }
}
