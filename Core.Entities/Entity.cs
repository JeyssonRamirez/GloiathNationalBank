using System;

namespace Core.Entities
{
    public abstract class Entity
    {
        /// <summary>
        /// Identificador interno
        /// Es la clave primaria de la base de datos
        /// Es un campo autogenerado por el gestor de base de datos
        /// Índice para realizar búsquedas
        /// </summary>       
        public long Id { get; set; }


        /// <summary>
        /// Fecha en que se crea el registro en la BD
        /// Este campo es para almacenar la fecha y hora UTC en el que se creó el registro 
        /// </summary>
        public DateTime RegistrationDate { get; set; }


        /// <summary>
        /// Permite establecer el estado en el que se encuentra la entidad
        /// </summary>
        public StatusType Status { get; set; }


    }
}
