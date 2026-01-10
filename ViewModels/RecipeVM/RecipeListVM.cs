namespace lab1_gr1.ViewModels.RecipeVM
{
    /// <summary>
    /// ViewModel reprezentujący podstawowe informacje o przepisie do wyświetlania w listach.
    /// </summary>
    public class RecipeListVM
    {
        /// <summary>
        /// Identyfikator przepisu.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa przepisu.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Opcjonalny opis przepisu.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data utworzenia przepisu.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Identyfikator użytkownika, który utworzył przepis.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Nazwa użytkownika, który utworzył przepis.
        /// </summary>
        public string UserName { get; set; }
    }
}
