using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverLinkReport.Models
{
    /// <value>
    /// Defines status of the Vehicle
    /// </value>

    public enum VehicleStatuses
    {
        Active,
        Inactive
    }
    /// <summary>
    /// Main <c>Vehicle</c> class where all properties are set
    /// </summary>
    [IgnoreFirst(1)] 
    [DelimitedRecord("|")]
    public class FlatFileVehicle
    {
        /// <value>
        ///  Gets and sets the ID of the vehicle
        /// </value>
        [FieldHidden]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Vehicle_Id { get; set; }
        /// <value>
        /// Gets and sets the vehicle's license plate number
        /// </value>
        [StringLength(20)]
        [Display(Name = "Plate Number")]
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
        [Display(Name = "Vehicle State")]
        [StringLength(2)]
        public string VehicleState { get; set; }
        /// <value>
        /// Gets and sets the vehicle's status
        /// </value>
        [Display(Name = "Vehicle Status")]
        public string VehicleStatus { get; set; }
        /// <value>
        /// Gets and sets the vehicle's class
        /// </value>
        public FlatFileVehicleClass VehicleClass { get; set; } 

        [Display(Name = "Vehicle Class")]
        [FieldHidden]
        public Classifications Classification { get; set; }
        /// <value>
        /// Gets and sets the transaction that was made with the vehicle
        /// </value>
        [FieldHidden]
        public virtual ICollection<FlatFileTransaction> Transactions { get; set; }
        /// <value>
        /// Gets and sets the transponders associated with the vehicle
        /// </value>
        [FieldHidden]
        public virtual ICollection<FlatFileTransponder> Transponders { get; set; }
    }
}
