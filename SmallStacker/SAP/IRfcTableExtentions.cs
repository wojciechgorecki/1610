namespace SmallStacker.SAP
{
    using System;
    using System.Data;
    using global::SAP.Middleware.Connector;

    /// Reference:
    /// <see cref="http://antswift.wordpress.com/2011/12/22/irfctable-to-net-datatable-extention-method/"/>
    /// <see cref="http://stackoverflow.com/questions/5300049/step-by-step-tutorial-to-use-sap-net-connector-with-vs-2008"/>s

    /// <summary>
    /// Initializes a new instance of the <see cref="IRfcTableExtentions"/> class.
    /// Klasa pomocnicza tłumacząca tabele SAP na table .NET DataTable
    /// </summary>
    public static class IRfcTableExtentions
    {
        /// <summary>
        /// Konwersja tabel SAP na tabele .NET DataTable
        /// </summary>
        /// <param name="sapTable">Tabela Sap do konwersji.</param>
        /// <param name="name">Nazwa tworzonej i zwracanej tabeli .NET DataTable</param>
        /// <returns>adoTable</returns>
        public static DataTable ToDataTable(this IRfcTable sapTable, string name)
        {
            DataTable adoTable = new DataTable(name);

            // Tworzenie ADO.Net tabeli
            for (int element = 0; element < sapTable.ElementCount; element++)
            {
                RfcElementMetadata metadata = sapTable.GetElementMetadata(element);
                adoTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }

            // Stworzenie wierszy tabeli ADO.Net na podstawie tabeli SAP
            foreach (IRfcStructure row in sapTable)
            {
                DataRow ldr = adoTable.NewRow();
                for (int element = 0; element < sapTable.ElementCount; element++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(element);

                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            ldr[metadata.Name] = row.GetString(metadata.Name).Substring(0, 4) + row.GetString(metadata.Name).Substring(5, 2) + row.GetString(metadata.Name).Substring(8, 2);
                            break;
                        case RfcDataType.BCD:
                            ldr[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            ldr[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }

                adoTable.Rows.Add(ldr);
            }

            return adoTable;
        }

        /// <summary>
        /// Metoda sprawdza typ obiektu <see cref="RfcDataType"/>
        /// </summary>
        /// <param name="rfcDataType">typ <see cref="RfcDataType"/></param>
        /// <returns>Zwraca typeof() typów prostych, zależnie od parametru </returns>
        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                case RfcDataType.CHAR:
                    return typeof(string);
                case RfcDataType.STRING:
                    return typeof(string);
                case RfcDataType.BCD:
                    return typeof(decimal);
                case RfcDataType.INT2:
                    return typeof(int);
                case RfcDataType.INT4:
                    return typeof(int);
                case RfcDataType.FLOAT:
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }
    }
}
