namespace SmallStacker.SAP.Returns
{
    /// <summary>
    /// Klasa zawierająca zmienne pomocnicze, do których przypisywane są wartości zwracane podczas wysyłania kontenera.
    /// </summary>
    public class SendingContainerReturn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendingContainerReturn"/> class.
        /// Konstruktor klasy, tworzy puste instancje zmienych.
        /// </summary>
        public SendingContainerReturn()
        {
            this.FVO_RETURN = string.Empty;
            this.Error = string.Empty;
        }

        // pozostałosci po starej aplikacji-nie uzywane
        // public ContainerData(string FVO_RETURN, string FVO_LOC, string FVO_ER)
        // {
        //    this.FVO_RETURN = FVO_RETURN;
        //    this.FVO_LOC = FVO_LOC;
        //    this.FVO_ER = FVO_ER;
        // }

        /// <summary>
        /// Gets or sets Error zwracane podczas wysyłania kontenera.
        /// </summary>
        /// <value>
        /// Wartosc "Error" zwracane podczas wysyłania kontenera.
        /// </value>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets FVO_RETURN zwracane podczas wysyłania kontenera.
        /// </summary>
        /// <value>
        /// Wartosc "FVO_RETURN" zwracane podczas wysyłania kontenera.
        /// </value>
        public string FVO_RETURN { get; set; }

        /// <summary>
        /// Gets or sets ReturnCode zwracane podczas wysyłania kontenera.
        /// </summary>
        /// <value>
        /// Wartosc "ReturnCode" zwracane podczas wysyłania kontenera.
        /// </value>
        public int ReturnCode = -1;
    }
}
