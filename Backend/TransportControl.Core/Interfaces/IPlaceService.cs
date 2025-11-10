using TransportControl.Core.Entities;

namespace TransportControl.Core.Interfaces;

/// <summary>
/// Interfaz para el servicio de lugares
/// </summary>
public interface IPlaceService
{
    /// <summary>
    /// Obtiene todos los lugares con paginación
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>Lista de lugares</returns>
    Task<IEnumerable<Place>> GetPlacesAsync(int page = 1, int pageSize = 10);

    /// <summary>
    /// Obtiene la cantidad total de lugares
    /// </summary>
    /// <returns>Cantidad total de lugares</returns>
    Task<int> GetPlacesCountAsync();

    /// <summary>
    /// Obtiene un lugar por su ID
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>Lugar encontrado o null</returns>
    Task<Place?> GetPlaceByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo lugar
    /// </summary>
    /// <param name="place">Lugar a crear</param>
    /// <returns>Lugar creado</returns>
    Task<Place> CreatePlaceAsync(Place place);

    /// <summary>
    /// Actualiza un lugar existente
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <param name="place">Lugar con los datos actualizados</param>
    /// <returns>Lugar actualizado</returns>
    Task<Place> UpdatePlaceAsync(int id, Place place);

    /// <summary>
    /// Elimina un lugar
    /// </summary>
    /// <param name="id">ID del lugar a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeletePlaceAsync(int id);

    /// <summary>
    /// Obtiene lugares que pueden ser origen
    /// </summary>
    /// <returns>Lista de lugares origen</returns>
    Task<IEnumerable<Place>> GetOriginPlacesAsync();

    /// <summary>
    /// Obtiene lugares que pueden ser destino
    /// </summary>
    /// <returns>Lista de lugares destino</returns>
    Task<IEnumerable<Place>> GetDestinationPlacesAsync();

    /// <summary>
    /// Obtiene todos los lugares activos
    /// </summary>
    /// <returns>Lista de lugares activos</returns>
    Task<IEnumerable<Place>> GetActivePlacesAsync();

    /// <summary>
    /// Verifica si existe un lugar con el código especificado
    /// </summary>
    /// <param name="code">Código del lugar</param>
    /// <param name="excludeId">ID del lugar a excluir de la búsqueda (para actualizaciones)</param>
    /// <returns>True si existe un lugar con ese código</returns>
    Task<bool> ExistsByCodeAsync(string code, int? excludeId = null);
}