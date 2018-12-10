using FileHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RiverLinkReporter.models
{
    /// <value>
    /// Defines plaza used for the transaction
    /// </value>
    public enum Plazas
    {
        //NB/SB for direction of travel
        LincolnSB,
        LincolnNB,
        KennedySB,
        KennedyNB,
        EastEndSB,
        EastEndNB,
        None
    }
    /// <value>
    /// Defines the status of the transaction
    /// </value>
    public enum TransactionStatuses
    {
        Paid,
        Unpaid
    }
    /// <value>
    /// Defines the type of transaction that was made
    /// </value>
    public enum TransactionTypes
    {
        Payment,
        Toll,
        Discount,
        Fee,
        None
    }
    /// <summary>
    /// Main <c>Transaction</c> class where all properties are set
    /// </summary>
    [IgnoreFirst(1)] 
    [DelimitedRecord("|")]
    public class FlatFileTransaction
    {
        /// <value>
        /// Gets and sets the transaction id associated with the transaction
        /// </value>
        [Key]
        [Required]
        public int Transaction_Id { get; set; }
        /// <value>
        /// Gets and sets the date and time the transaction was made
        /// </value>
        [Required]
        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
        [DisplayFormat(DataFormatString = "yyyyMMddHHmmss")]
        public DateTime TransactionDate { get; set; }
        /// <value>
        /// Gets and sets the date and time the transaction was posted on the website
        /// </value>
        [Required]
        [FieldConverter(ConverterKind.Date, "yyyyMMddHHmmss")]
        [DisplayFormat(DataFormatString = "yyyyMMddHHmmss")]
        public DateTime? PostedDate { get; set; }
        /// <value>
        /// Gets and sets the status of the transaction
        /// </value>
        public string TransactionStatus { get; set; }
        /// <value>
        /// Gets and sets the plaza used for the transaction
        /// </value>
        /// 
        public string Plaza { get; set; }
        /// <value>
        /// Gets and sets the vehicle used for the transaction
        /// </value>
        [FieldHidden]
        public virtual FlatFileVehicle Vehicle { get; set; }
        /// <value>
        /// Gets and sets the journal id associated with the transaction
        /// </value>
        public virtual int Journal_Id { get; set; }
        /// <value>
        /// Gets and sets all related journal ids to the transaction
        /// </value>
        /// <remarks>
        /// Each transaction can multiple journal ids associated to it
        /// </remarks>
        /// <example>
        /// Transactions with type "Payment" can have multiple associated journal ids
        /// with transactions of type "Toll"
        /// </example>
        [FieldConverter(typeof(RelatedJournalConverter))]
        public virtual List<int> RelatedJournal_Id { get; set; }
        /// <value>
        /// Gets and sets the transponder associated with the transaction
        /// </value>
        public virtual FlatFileTransponder Transponder { get; set; }

        public int TransponderNumber { get; set; }
        /// <value>
        /// Gets and sets the type of the transaction
        /// </value>
        public string TransactionType { get; set; }
        /// <value>
        /// Gets and sets the amount of money associated with the transaction
        /// </value>
        [Required]
        public virtual double Amount { get; set; }
        /// <value>
        /// Gets and sets the description of the transaction
        /// </value>
        [MaxLength(50)]
        public virtual string TransactionDescription { get; set;}
        /// <value>
        /// Gets and sets the lane associated with the transaction
        /// </value>
        public virtual int Lane { get; set; }
        /// <value>
        /// Gets and sets the License plate number that was used for the transaction
        /// </value>
        [MaxLength(20)]
        public virtual string PlateNumber { get; set; }

        public Int16 VehicleClass_Id { get; set; }

        public int TransactionNumber { get; set; }
    }
}
