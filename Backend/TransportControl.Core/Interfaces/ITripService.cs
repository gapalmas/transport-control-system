using TransportControl.Core.Entities;

namespace TransportControl.Core.Interfaces;

/// <summary>
/// Interfaz para el servicio de viajes
/// </summary>
public interface ITripService
{
    /// <summary>
    /// Obtiene todos los viajes con paginación
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>Lista de viajes</returns>
    Task<IEnumerable<Trip>> GetTripsAsync(int page = 1, int pageSize = 10);

    /// <summary>
    /// Obtiene un viaje por su ID
    /// </summary>
    /// <param name="id">ID del viaje</param>
    /// <returns>Viaje encontrado o null</returns>
    Task<Trip?> GetTripByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo viaje
    /// </summary>
    /// <param name="trip">Viaje a crear</param>
    /// <returns>Viaje creado</returns>
    Task<Trip> CreateTripAsync(Trip trip);

    /// <summary>
    /// Actualiza un viaje existente
    /// </summary>
    /// <param name="trip">Viaje con los datos actualizados</param>
    /// <returns>Viaje actualizado</returns>
    Task<Trip> UpdateTripAsync(Trip trip);

    /// <summary>
    /// Actualiza solo el estado de un viaje
    /// </summary>
    /// <param name="tripId">ID del viaje</param>
    /// <param name="status">Nuevo estado</param>
    /// <param name="actualStartDateTime">Fecha de inicio actual (opcional)</param>
    /// <param name="actualEndDateTime">Fecha de fin actual (opcional)</param>
    /// <param name="notes">Notas adicionales (opcional)</param>
    /// <returns>Viaje actualizado</returns>
    Task<Trip> UpdateTripStatusAsync(int tripId, TripStatus status, DateTime? actualStartDateTime = null, DateTime? actualEndDateTime = null, string? notes = null);

    /// <summary>
    /// Elimina un viaje
    /// </summary>
    /// <param name="id">ID del viaje a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteTripAsync(int id);

    /// <summary>
    /// Obtiene viajes por estado
    /// </summary>
    /// <param name="status">Estado de los viajes</param>
    /// <returns>Lista de viajes con el estado especificado</returns>
    Task<IEnumerable<Trip>> GetTripsByStatusAsync(TripStatus status);

    /// <summary>
    /// Valida si un viaje puede ser creado o actualizado
    /// </summary>
    /// <param name="trip">Viaje a validar</param>
    /// <returns>Lista de errores de validación (vacía si es válido)</returns>
    Task<IEnumerable<string>> ValidateTripAsync(Trip trip);
}