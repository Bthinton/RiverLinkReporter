using FileHelpers;
using System;
using System.ComponentModel.DataAnnotations;

//Sort same as transaction file
//"attach" any real 

namespace RiverLinkReport.Models
{
    /// <summary>
    /// Main <c>Transaction</c> class where all properties are set
    /// </summary>
    [IgnoreFirst(1)]
    [DelimitedRecord("|")]
    public class Transaction
    {
        [Key]
        public int TransactionNumber { get; set; }

        public Guid UserId { get; set; }
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

        public string TransactionStatus { get; set; }

        /// <summary>
        /// Gets or sets the plaza.
        /// </summary>
        /// <value>
        /// The plaza.
        /// </value>
        public string Plaza { get; set; }

        /// <value>
        /// Gets and sets the journal id associated with the transaction
        /// </value>
        public virtual int Journal_Id { get; set; }

        /// <value>
        /// Gets and sets the transponder associated with the transaction
        /// </value>
        [FieldHidden]
        public virtual string Transponder { get; set; }

        public int TransponderNumber { get; set; }

        public string TransactionType { get; set; }

        /// <value>
        /// Gets and sets the amount of money associated with the transaction
        /// </value>
        [Required]
        public virtual Double Amount { get; set; }

        /// <value>
        /// Gets and sets the description of the transaction
        /// </value>
        [MaxLength(50)]
        public virtual string TransactionDescription { get; set; }

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
    }
}
