using Interop.ErpBS800;
using Interop.StdBE800;
using Interop.StdPlatBS800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Lib_Primavera
{
    public class PriEngine
    {

        public static StdPlatBS Platform { get; set; }
        public static ErpBS Engine { get; set; }

        public static bool InitializeCompany(string Company, string User, string Password)
        {

            StdBSConfApl objAplConf = new StdBSConfApl();
            StdPlatBS my_platform = new StdPlatBS();
            ErpBS MotorLE = new ErpBS();

            EnumTipoPlataforma objTipoPlataforma = new EnumTipoPlataforma();
            objTipoPlataforma = EnumTipoPlataforma.tpProfissional;

            objAplConf.Instancia = "Default";
            objAplConf.AbvtApl = "GCP";
            objAplConf.PwdUtilizador = Password;
            objAplConf.Utilizador = User;

            StdBETransaccao objStdTransac = new StdBETransaccao();

            // Opem platform.
            my_platform.AbrePlataformaEmpresaIntegrador(ref Company, ref objStdTransac, ref objAplConf, ref objTipoPlataforma);

            // Is plt initialized?
            if (my_platform.Inicializada)
            {

                // Retuns the ptl.
                Platform = my_platform;

                bool blnModoPrimario = true;

                // Open Engine
                MotorLE.AbreEmpresaTrabalho(EnumTipoPlataforma.tpProfissional, ref Company, ref User, ref Password, ref objStdTransac, "Default", ref blnModoPrimario);

                // Returns the engine.
                Engine = MotorLE;

                return true;
            }
            else
            {
                return false;
            }


        }
    }
}