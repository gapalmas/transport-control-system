using TransportControl.Core.Entities;

namespace TransportControl.Core.Interfaces;

/// <summary>
/// Interfaz para el servicio de operadores
/// </summary>
public interface IOperatorService
{
    /// <summary>
    /// Obtiene todos los operadores
    /// </summary>
    /// <returns>Lista de operadores</returns>
    Task<IEnumerable<Operator>> GetOperatorsAsync();

    /// <summary>
    /// Obtiene un operador por su ID
    /// </summary>
    /// <param name="id">ID del operador</param>
    /// <returns>Operador encontrado o null</returns>
    Task<Operator?> GetOperatorByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo operador
    /// </summary>
    /// <param name="operatorEntity">Operador a crear</param>
    /// <returns>Operador creado</returns>
    Task<Operator> CreateOperatorAsync(Operator operatorEntity);

    /// <summary>
    /// Actualiza un operador existente
    /// </summary>
    /// <param name="operatorEntity">Operador con los datos actualizados</param>
    /// <returns>Operador actualizado</returns>
    Task<Operator> UpdateOperatorAsync(Operator operatorEntity);

    /// <summary>
    /// Elimina un operador
    /// </summary>
    /// <param name="id">ID del operador a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    Task<bool> DeleteOperatorAsync(int id);

    /// <summary>
    /// Obtiene operadores activos
    /// </summary>
    /// <returns>Lista de operadores activos</returns>
    Task<IEnumerable<Operator>> GetActiveOperatorsAsync();
}