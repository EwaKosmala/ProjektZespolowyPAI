using lab1_gr1.Models;
using AutoMapper;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Klasa bazowa dla wszystkich serwisów aplikacji.
    /// Zapewnia dostęp do kontekstu bazy danych oraz mechanizmu mapowania obiektów.
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Kontekst bazy danych aplikacji.
        /// </summary>
        protected readonly MyDBContext _dbContext;

        /// <summary>
        /// Mapper AutoMapper służący do mapowania modeli domenowych na ViewModel i odwrotnie.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="BaseService"/>.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych.</param>
        /// <param name="mapper">Obiekt AutoMapper.</param>
        protected BaseService(MyDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
