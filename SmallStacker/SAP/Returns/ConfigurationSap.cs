using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallStacker.SAP.Returns
{
    public class ConfigurationSap
    {
        private char fvi_no_lagp;
        private string userName;
        private string pernr;
        private string mode;

        public char _FVI_NO_LAGP
        {
            get { return fvi_no_lagp; }
            set
            {
                fvi_no_lagp = value;
            }
        }

        /// <summary>
        /// zmienna przechowujaca nazwe użytkownika pobierana z domeny.
        /// </summary>
        public string _userName
        {
            get { return userName; }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// zmienna przechowujaca numer ID uzytkownika z SAP
        /// </summary>
        public string _pernr
        {
            get { return pernr; }
            set
            {
                pernr = value;
            }
        }

        /// <summary>
        /// zmienna przechowująca tryb dostępu do SAP PROD lub DEV
        /// </summary>
        public string _mode
        {
            get { return mode; }
            set
            {
                mode = value;
            }
        }

        public ConfigurationSap(char _fvi_NO_LAGP,
                                string _UserName,
                                string _Pernr,
                                string _Mode)
        {
            _FVI_NO_LAGP = _fvi_NO_LAGP;
            _userName = _UserName;
            _pernr = _Pernr;
            _mode = _Mode;
        }
    }
}
