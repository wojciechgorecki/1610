namespace MalaUkladnica.SAP
{
    /// <summary>
    /// Klasa zawierajaca informacje zwrotne z akcji SAP: wycofanie kontenera z buforu stacji.
    /// </summary>
    public class BackContainerBufforReturn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackContainerBufforReturn"/> class.
        /// Konstruktor klasy, inicjalizuje zmienne wartosciami pustymi i -1.
        /// </summary>
        public BackContainerBufforReturn()
        {
            this.FVO_RETURN = string.Empty;
            this.ReturnCode = -1;
            this.Error = string.Empty;
        }

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
