using FileHelpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverLinkReport.Models
{
    /// <summary>
    /// Main <c>Vehicle</c> class where all properties are set
    /// </summary>
    [IgnoreFirst(1)]
    [DelimitedRecord("|")]
    public class Vehicle
    {
        [FieldHidden]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Vehicle_Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(20)]
        public string PlateNumber { get; set; }
        /// <value>
        /// Gets and sets the vehicle's make
        /// </value>
        [StringLength(32)]
        public string Make { get; set; }
        /// <value>
        /// Gets and sets the vehicle's model
        /// </value>
        [StringLength(32)]
        public string Model { get; set; }
        /// <value>
        /// Gets and sets the year of the vehicle
        /// </value>
        public int Year { get; set; }
        /// <value>
        /// Gets and sets the State of the vehicle
        /// </value>
        //[Required]
        [StringLength(2)]
        public string VehicleState { get; set; }
        /// <value>
        /// Gets and sets the vehicle's status
        /// </value>
        public string VehicleStatus { get; set; }
        /// <value>
        /// Gets and sets the vehicle's class
        /// </value>

        public string VehicleClass { get; set; }

        public string Transponder { get; set; }

        public string TransponderType { get; set; }
    }
}
