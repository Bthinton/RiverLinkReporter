using FileHelpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiverLinkReporter.models
{
    /// <value>
    /// Defines type of transponder
    /// </value>
    public enum TransponderTypes
    {
        EZPass, 
        Sticker
    }
    /// <value>
    /// Defines status of transponder
    /// </value>
    public enum TransponderStatuses
    {
        Valid,
        Invalid
    }
    /// <summary>
    /// Main <c>Transponder</c> class where all properties are set
    /// </summary>
    [DelimitedRecord("|")]
    public class FlatFileTransponder
    {
        /// <value>
        /// Gets and sets the transponder id
        /// </value>
        [FieldHidden]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Transponder_Id { get; set; }

        public string TransponderNumber { get; set; }
        /// <value>
        /// Gets and sets the type of transponder
        /// </value>
        [Required]
        [Display(Name = "Transponder Type")]
        public TransponderTypes TransponderType { get; set; }
        /// <value>
        /// Gets and sets the vehicle used with the transponder
        /// Field</value>
        [FieldHidden]
        public virtual ICollection<FlatFileVehicle> Vehicles { get; set; }
        /// <value>
        /// Gets and sets the transaction made with the transponder
        /// </value>
        [FieldHidden]
        public virtual ICollection<FlatFileTransaction> Transactions { get; set; }

        public string PlateNumber { get; set; }
    }
}
