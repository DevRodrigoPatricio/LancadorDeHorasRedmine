using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancarHoras
{
    public class Enums
    {
        public enum TipoConfigBD
        {
            NaoDefinido,
            Definido,
        }
        public enum TipoIntegracao
        {
            Manual,
            Automatica,
        }

        public enum TituloAlerta
        {
            [Display(Name = "Aviso")]
            AVISO = 1,
            [Display(Name = "Confirmação")]
            CONFIRM = 2,
            [Display(Name = "Inconsistência")]
            INCONSIS = 3
        }

        public enum BotaoAlerta
        {
            MB_OK = 0,
            MB_OKCANCEL = 1,
            MB_ABORT_RETRY_IGNORE = 2,
            MB_YESNOCANCEL = 3,
            MB_YESNO = 4,
            MB_RETRYCANCEL = 5
        }

        public enum IconeAlerta
        {
            None = 0,
            Hand = 16,
            Stop = 16,
            Error = 16,
            Question = 32,
            Exclamation = 48,
            Warning = 48,
            Asterisk = 64,
            Information = 64
        }

    }
}
