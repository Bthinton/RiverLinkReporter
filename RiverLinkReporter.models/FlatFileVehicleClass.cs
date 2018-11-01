using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RiverLinkReport.Models
{
    /// <value>
    /// Defines the classification of a vehicle
    /// </value>
    public enum Classifications
    {
        Class1,
        Class2,
        Class3,
        None
    }
    /// <value>
    /// Defines the price type of the class
    /// </value>
    public enum PriceTypes
    {
        Transponder,
        RegisteredPlate,
        UnregisteredPlate,
        None
    }
    /// <summary>
    /// The main <c>VehicleClass</c> class that all properties are set to
    /// </summary>
    public class FlatFileVehicleClass
    {
        /// <value>
        /// Gets and sets the Vehicle Class' id
        /// </value>
        [Key]
        [Required]
        public Int16 VehicleClass_Id { get; set; }
        /// <value>
        /// Gets and sets the description of the <c>VehicleClass</c>
        /// </value>
        [StringLength(32)]
        public string VehicleDescription { get; set; }
        /// <value>
        /// Gets and sets the price for the vehicle class
        /// </value>
        [Required]
        public double Price { get; set; }
        /// <value>
        /// Gets and sets price type of the vehicle class
        /// </value>
        public PriceTypes PriceType { get; set; }
        /// <value>
        /// Gets and sets vehicle's associated with the vehicle class
        /// </value>
        public virtual ICollection<FlatFileVehicle> Vehicles { get; set; }
        /// <value>
        /// Gets and sets the classfication of the vehicle class
        /// </value>
        [Required]
        public Classifications Classification { get; set; }
    }
}
