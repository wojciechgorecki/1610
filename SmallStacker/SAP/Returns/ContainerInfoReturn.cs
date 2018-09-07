using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallStacker.SAP
{
    public class ContainerInfoReturn
    {
        public ContainerInfoReturn()
        {
            FVO_RETURN = string.Empty;
            FVO_LOC = string.Empty;
            FVO_ER = string.Empty;
            ReturnCode = -1;
            Error = string.Empty;
        }

        //public ContainerData(string FVO_RETURN, string FVO_LOC, string FVO_ER)
        //{
        //    this.FVO_RETURN = FVO_RETURN;
        //    this.FVO_LOC = FVO_LOC;
        //    this.FVO_ER = FVO_ER;
        //}

        public string FVO_LOC;
        public string FVO_ER;
        /// <summary>
        /// Gets or sets Error zwracane podczas wycofania kontenera z bufora stacji.
        /// </summary>
        /// <value>
        /// Wartosc "Error" zwracane podczas wycofania kontenera z bufora stacji.
        /// </value>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets FVO_RETURN zwracane podczas wycofania kontenera z bufora stacji.
        /// </summary>
        /// <value>
        /// Wartosc "FVO_RETURN" zwracane podczas wycofania kontenera z bufora stacji.
        /// </value>
        public string FVO_RETURN { get; set; }

        /// <summary>
        /// Gets or sets ReturnCode zwracane podczas wycofania kontenera z bufora stacji.
        /// </summary>
        /// <value>
        /// Wartosc "ReturnCode" zwracane podczas wycofania kontenera z bufora stacji.
        /// </value>
        public int ReturnCode = -1;
    }
}
